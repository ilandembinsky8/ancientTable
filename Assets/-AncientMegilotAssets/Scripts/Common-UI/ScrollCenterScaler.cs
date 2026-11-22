using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Picker wheel for a vertical ScrollRect (put it on the same object as the ScrollRect):
// - the item in the middle of the viewport is full size, items toward the edges get smaller
// - when scrolling stops, the nearest item snaps to the center
// - the centered item gets Unity's Select event (ISelectHandler), the previous one gets Deselect
public class ScrollCenterScaler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IScrollHandler
{
    [SerializeField] ScrollRect _scrollRect;
    [Tooltip("Optional. Padded so the first and last items can scroll to the center.")]
    [SerializeField] VerticalLayoutGroup _contentLayout;

    [Header("Size")]
    [Tooltip("Scale of an item at the edge of the falloff distance.")]
    [SerializeField, Range(0.1f, 1f)] float _edgeScale = 0.75f;
    [Tooltip("Distance from the center, in viewport heights, where the edge scale is reached. 0.5 = viewport edge.")]
    [SerializeField, Range(0.1f, 1f)] float _falloff = 0.5f;
    [Tooltip("How the size drops from center (0) to edge (1).")]
    [SerializeField] AnimationCurve _curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Snap & Select")]
    [Tooltip("Open the list scrolled to its middle item.")]
    [SerializeField] bool _startCentered = true;
    [SerializeField] bool _snapToCenter = true;
    [Tooltip("How fast the list slides into place.")]
    [SerializeField] float _snapSpeed = 12f;
    [Tooltip("Snapping starts once the scroll speed drops below this.")]
    [SerializeField] float _snapVelocity = 60f;
    [Tooltip("Wait this long after the last drag or wheel input before snapping.")]
    [SerializeField] float _snapDelay = 0.15f;

    readonly Vector3[] _corners = new Vector3[4];
    int _paddedChildCount = -1;
    bool _centeredOnce;
    bool _dragging;
    bool _userMoved;
    float _lastInputTime;
    RectTransform _selected;

    public void OnBeginDrag(PointerEventData eventData) { _dragging = true; MarkInput(); }
    public void OnEndDrag(PointerEventData eventData) { _dragging = false; MarkInput(); }
    public void OnScroll(PointerEventData eventData) { MarkInput(); }

    void MarkInput()
    {
        _userMoved = true;
        _lastInputTime = Time.unscaledTime;
    }

    void LateUpdate()
    {
        RectTransform content = _scrollRect.content;
        RectTransform viewport = _scrollRect.viewport;

        // Items may be spawned at runtime, so re-pad whenever the item count changes.
        if (_contentLayout != null && content.childCount != _paddedChildCount)
        {
            PadEnds(content, viewport);
            _paddedChildCount = content.childCount;
        }

        if (_startCentered && !_centeredOnce && content.childCount > 0)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
            _scrollRect.verticalNormalizedPosition = 0.5f;
            _centeredOnce = true;
        }

        viewport.GetWorldCorners(_corners);
        float centerY = (_corners[0].y + _corners[1].y) * 0.5f;
        float height = _corners[1].y - _corners[0].y;
        if (height <= 0f) return;

        RectTransform nearest = null;
        float nearestOffset = float.MaxValue;

        foreach (RectTransform item in content)
        {
            if (!item.gameObject.activeSelf) continue;

            float offset = centerY - item.TransformPoint(item.rect.center).y;
            float t = _curve.Evaluate(Mathf.Clamp01(Mathf.Abs(offset) / (height * _falloff)));
            item.localScale = Vector3.one * Mathf.Lerp(1f, _edgeScale, t);

            if (Mathf.Abs(offset) < Mathf.Abs(nearestOffset))
            {
                nearest = item;
                nearestOffset = offset;
            }
        }

        if (_snapToCenter && nearest != null && CanSnap())
            SnapTowards(nearest, nearestOffset, content, viewport);
    }

    bool CanSnap()
    {
        return !_dragging
            && Time.unscaledTime - _lastInputTime >= _snapDelay
            && Mathf.Abs(_scrollRect.velocity.y) < _snapVelocity;
    }

    void SnapTowards(RectTransform nearest, float worldOffset, RectTransform content, RectTransform viewport)
    {
        float localOffset = viewport.InverseTransformVector(new Vector3(0f, worldOffset, 0f)).y;

        if (Mathf.Abs(localOffset) > 0.5f)
        {
            _scrollRect.velocity = Vector2.zero;
            float step = localOffset * Mathf.Min(1f, _snapSpeed * Time.unscaledDeltaTime);
            content.anchoredPosition += new Vector2(0f, step);
            return;
        }

        // Settled. Only select after the player has scrolled, so nothing is picked on open.
        if (_userMoved && nearest != _selected) SelectItem(nearest);
    }

    void SelectItem(RectTransform item)
    {
        BaseEventData data = new BaseEventData(EventSystem.current);
        if (_selected != null) ExecuteEvents.Execute(_selected.gameObject, data, ExecuteEvents.deselectHandler);
        _selected = item;
        ExecuteEvents.Execute(item.gameObject, data, ExecuteEvents.selectHandler);
    }

    // Top/bottom padding of half the viewport (minus half an item) lets the end items reach the center.
    void PadEnds(RectTransform content, RectTransform viewport)
    {
        if (content.childCount == 0) return;

        Canvas.ForceUpdateCanvases();
        float itemHeight = ((RectTransform)content.GetChild(0)).rect.height;
        int pad = Mathf.Max(0, Mathf.RoundToInt((viewport.rect.height - itemHeight) * 0.5f));
        _contentLayout.padding.top = pad;
        _contentLayout.padding.bottom = pad;
        LayoutRebuilder.MarkLayoutForRebuild(content);
    }
}
