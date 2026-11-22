using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// One draggable scroll piece. Created and set up by PuzzleBoard.
// When dropped close enough to its target it snaps in and locks.
public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform _rect;
    [SerializeField] Image _image;

    PuzzleBoard _board;
    RectTransform _layer;
    Vector2 _grabOffset;

    public Vector2 Target { get; private set; }
    public float TargetRotation { get; private set; }
    public bool IsPlaced { get; private set; }
    public RectTransform Rect => _rect;

    public void Setup(PuzzleBoard board, RectTransform layer, Sprite sprite, Vector2 size,
                      Vector2 start, float startRotation, Vector2 target, float targetRotation)
    {
        _board = board;
        _layer = layer;
        _image.sprite = sprite;
        _rect.sizeDelta = size;
        _rect.anchoredPosition = start;
        _rect.localEulerAngles = new Vector3(0f, 0f, startRotation);
        Target = target;
        TargetRotation = targetRotation;
        IsPlaced = false;
        _image.raycastTarget = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;
        _rect.SetAsLastSibling(); // draw above the other pieces
        if (ToLayer(eventData, out Vector2 pointer)) _grabOffset = _rect.anchoredPosition - pointer;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;
        if (ToLayer(eventData, out Vector2 pointer)) _rect.anchoredPosition = pointer + _grabOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;
        _board.TryPlace(this);
    }

    // Snaps into the target and stops reacting to drags.
    public void Lock()
    {
        IsPlaced = true;
        _rect.anchoredPosition = Target;
        _rect.localEulerAngles = new Vector3(0f, 0f, TargetRotation);
        _rect.SetAsFirstSibling(); // placed pieces go under the loose ones
        _image.raycastTarget = false;
    }

    bool ToLayer(PointerEventData eventData, out Vector2 local)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(_layer, eventData.position, eventData.pressEventCamera, out local);
    }
}
