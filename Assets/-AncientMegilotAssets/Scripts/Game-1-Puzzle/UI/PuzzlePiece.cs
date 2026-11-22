using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// One draggable scroll piece. Created and set up by PuzzleBoard.
// When dropped close enough to its spot it moves into the board and locks there.
public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform _rect;
    [SerializeField] Image _image;

    PuzzleBoard _board;
    RectTransform _dragLayer;
    Vector3 _grabOffset;

    public string PieceId { get; private set; }
    // Spot inside the board content (local position).
    public Vector3 Target { get; private set; }
    public float TargetRotation { get; private set; }
    public bool IsPlaced { get; private set; }
    public RectTransform Rect => _rect;

    public void Setup(PuzzleBoard board, RectTransform dragLayer, string pieceId, Sprite sprite,
                      Vector3 start, float startRotation, Vector3 target, float targetRotation)
    {
        _board = board;
        _dragLayer = dragLayer;
        PieceId = pieceId;
        _image.sprite = sprite;
        _rect.anchorMin = _rect.anchorMax = _rect.pivot = new Vector2(0.5f, 0.5f);
        _rect.sizeDelta = sprite.rect.size; // 1:1, like the Figma
        _rect.localPosition = start;
        _rect.localEulerAngles = new Vector3(0f, 0f, startRotation);
        Target = target;
        TargetRotation = targetRotation;
        IsPlaced = false;
        _image.raycastTarget = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;
        _rect.SetAsLastSibling(); // draw above the other loose pieces
        if (ToLayer(eventData, out Vector2 pointer)) _grabOffset = _rect.localPosition - (Vector3)pointer;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;
        if (ToLayer(eventData, out Vector2 pointer)) _rect.localPosition = (Vector3)pointer + _grabOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;
        _board.TryPlace(this);
    }

    // Moves into the board content at its spot and stops reacting to drags.
    public void Lock(RectTransform boardContent)
    {
        IsPlaced = true;
        _rect.SetParent(boardContent, false);
        _rect.localPosition = Target;
        _rect.localEulerAngles = new Vector3(0f, 0f, TargetRotation);
        _image.raycastTarget = false;
    }

    bool ToLayer(PointerEventData eventData, out Vector2 local)
    {
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(_dragLayer, eventData.position, eventData.pressEventCamera, out local);
    }
}
