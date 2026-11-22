using UnityEngine;
using UnityEngine.UI;

// Wheel look for a vertical ScrollRect: the item in the middle of the viewport is full size,
// items toward the top and bottom edges get smaller.
public class ScrollCenterScaler : MonoBehaviour
{
    [SerializeField] ScrollRect _scrollRect;
    [Tooltip("Optional. Padded so the first and last items can scroll to the center.")]
    [SerializeField] VerticalLayoutGroup _contentLayout;

    [Tooltip("Scale of an item at the edge of the falloff distance.")]
    [SerializeField, Range(0.1f, 1f)] float _edgeScale = 0.75f;
    [Tooltip("Distance from the center, in viewport heights, where the edge scale is reached. 0.5 = viewport edge.")]
    [SerializeField, Range(0.1f, 1f)] float _falloff = 0.5f;
    [Tooltip("How the size drops from center (0) to edge (1).")]
    [SerializeField] AnimationCurve _curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [Tooltip("Open the list scrolled to its middle item.")]
    [SerializeField] bool _startCentered = true;

    readonly Vector3[] _corners = new Vector3[4];
    int _paddedChildCount = -1;
    bool _centeredOnce;

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

        foreach (RectTransform item in content)
        {
            if (!item.gameObject.activeSelf) continue;

            float itemCenterY = item.TransformPoint(item.rect.center).y;
            float t = _curve.Evaluate(Mathf.Clamp01(Mathf.Abs(itemCenterY - centerY) / (height * _falloff)));
            item.localScale = Vector3.one * Mathf.Lerp(1f, _edgeScale, t);
        }
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
