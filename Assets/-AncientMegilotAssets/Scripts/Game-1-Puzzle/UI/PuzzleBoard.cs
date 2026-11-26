using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// The puzzle, laid out 1:1 like the Figma. Positions in the data are Figma pixels measured from the
// center of the 1920x1080 screen (_frame), y up:
// - pieces start at ScrollData.pieces[i].startPosition, stacked in PuzzleSetSO.pileOrder
// - outlines sit at PuzzleSetSO.outlinePositions inside the board content, which scrolls (ScrollUp/ScrollDown)
// - a piece dropped within Snap Distance of its spot moves into the board and locks there
public class PuzzleBoard : MonoBehaviour
{
    [Header("Areas")]
    [Tooltip("The 1920x1080 screen the Figma positions are measured on.")]
    [SerializeField] RectTransform _frame;
    [Tooltip("Where loose pieces live and are dragged. No layout group.")]
    [SerializeField] RectTransform _pieceLayer;
    [Tooltip("Visible part of the board. Should have a RectMask2D.")]
    [SerializeField] RectTransform _boardViewport;
    [Tooltip("Scrolls inside the viewport. Holds the outlines and the placed pieces.")]
    [SerializeField] RectTransform _boardContent;
    [Tooltip("One outline image per scroll, top to bottom. Children of the board content.")]
    [SerializeField] List<Image> _silhouettes = new List<Image>();

    [Header("Pieces")]
    [SerializeField] PuzzlePiece _piecePrefab;
    [Tooltip("How close (in UI units) a piece must be dropped to its spot to snap in.")]
    [SerializeField] float _snapDistance = 40f;

    [Header("Scrolling")]
    [Tooltip("How far one ScrollUp / ScrollDown moves the board.")]
    [SerializeField] float _scrollStep = 300f;

    [Header("Events")]
    [SerializeField] UnityEvent<PuzzleSetSO> _onSetLoaded;
    [SerializeField] UnityEvent _onPiecePlaced;
    [SerializeField] UnityEvent<PuzzleSetSO> _onCompleted;// split to _onCompletedPuzzleFirst & _onCompletedPuzzleAll / make a list

    readonly List<PuzzlePiece> _pieces = new List<PuzzlePiece>();
    PuzzleSetSO _set;
    int _placed;
    Vector3 _contentHome;
    bool _hasHome;
    float _scrollMin, _scrollMax; // allowed content y offset from home

    public PuzzleSetSO CurrentSet => _set;

    PuzzleSetSO _pending;

    // The game flow may pick a set while the board is still hidden; build it once the board is shown,
    // when its layout is final.
    void OnEnable()
    {
        if (_pending == null) return;
        PuzzleSetSO set = _pending;
        _pending = null;
        Canvas.ForceUpdateCanvases();
        Build(set);
    }

    public void LoadSet(PuzzleSetSO set)
    {
        if (!isActiveAndEnabled) { _pending = set; return; }
        Build(set);
    }

    void Build(PuzzleSetSO set)
    {
        ClearPieces();
        _set = set;
        _placed = 0;
        if (!_hasHome) { _contentHome = _boardContent.localPosition; _hasHome = true; }
        _boardContent.localPosition = _contentHome;
        if (set == null) return;

        for (int s = 0; s < _silhouettes.Count; s++)
        {
            ScrollDataSO scroll = s < set.scrolls.Count ? set.scrolls[s] : null;
            ScrollData data = scroll != null ? scroll.Data : null;
            Image outline = _silhouettes[s];
            bool used = data != null && scroll.image != null && s < set.outlinePositions.Count;
            outline.gameObject.SetActive(used);
            if (!used) continue;

            RectTransform rt = outline.rectTransform;
            outline.sprite = scroll.image;
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = scroll.image.rect.size;
            rt.localPosition = FromFigma(_boardContent, set.outlinePositions[s]);

            for (int p = 0; p < scroll.pieceSprites.Count && p < data.pieces.Count; p++)
            {
                PieceData piece = data.pieces[p];
                PuzzlePiece instance = Instantiate(_piecePrefab, _pieceLayer);
                instance.name = piece.pieceId;
                instance.Setup(this, _pieceLayer, piece.pieceId, scroll.pieceSprites[p],
                               FromFigma(_pieceLayer, piece.startPosition), piece.startRotation,
                               rt.localPosition + (Vector3)piece.endPosition, piece.endRotation);
                _pieces.Add(instance);
            }
        }

        StackLikePile(set.pileOrder);
        UpdateScrollLimits();
        _onSetLoaded.Invoke(set);
    }

    // Called by a piece when it is dropped.
    public void TryPlace(PuzzlePiece piece)
    {
        Vector3 inBoard = _boardContent.InverseTransformPoint(piece.Rect.position);
        if (Vector2.Distance(inBoard, piece.Target) > _snapDistance) return;

        piece.Lock(_boardContent);
        _placed++;
        _onPiecePlaced.Invoke();
        if (_placed >= _pieces.Count) _onCompleted.Invoke(_set);
    }

    // Shows more of the top of the board.
    public void ScrollUp() => ScrollBy(-_scrollStep);

    // Shows more of the bottom of the board.
    public void ScrollDown() => ScrollBy(_scrollStep);

    void ScrollBy(float delta)
    {
        float offset = Mathf.Clamp(_boardContent.localPosition.y - _contentHome.y + delta, _scrollMin, _scrollMax);
        _boardContent.localPosition = _contentHome + new Vector3(0f, offset, 0f);
    }

    // The board may scroll until the top and bottom outline edges reach the viewport edges.
    void UpdateScrollLimits()
    {
        float top = float.MinValue, bottom = float.MaxValue;
        Vector3[] corners = new Vector3[4];
        foreach (Image outline in _silhouettes)
        {
            if (!outline.gameObject.activeSelf) continue;
            outline.rectTransform.GetWorldCorners(corners);
            top = Mathf.Max(top, _boardViewport.InverseTransformPoint(corners[1]).y);
            bottom = Mathf.Min(bottom, _boardViewport.InverseTransformPoint(corners[0]).y);
        }

        Rect view = _boardViewport.rect;
        _scrollMin = top > view.yMax ? -(top - view.yMax) : 0f;       // content moves down to show the top
        _scrollMax = bottom < view.yMin ? view.yMin - bottom : 0f;    // content moves up to show the bottom
    }

    // Draws the pieces in the Figma's layer order (first id = bottom of the pile).
    void StackLikePile(List<string> order)
    {
        foreach (string id in order)
            foreach (PuzzlePiece piece in _pieces)
                if (piece.PieceId == id) piece.Rect.SetAsLastSibling();
    }

    void ClearPieces()
    {
        foreach (PuzzlePiece piece in _pieces)
            if (piece != null) Destroy(piece.gameObject);
        _pieces.Clear();
    }

    // A Figma position (pixels from the screen center, y up) as a local position in 'space'.
    Vector3 FromFigma(RectTransform space, Vector2 figma)
    {
        Vector3 local = space.InverseTransformPoint(_frame.TransformPoint(_frame.rect.center + figma));
        local.z = 0f;
        return local;
    }
}
