using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// The puzzle: LoadSet(set) shows the set's scroll outlines in the silhouette area and spawns
// their pieces in the spawn area. A piece dropped within Snap Distance of its spot locks in.
public class PuzzleBoard : MonoBehaviour
{
    [Header("Areas")]
    [Tooltip("Where pieces live and are dragged. Should cover the spawn and silhouette areas, without a layout group.")]
    [SerializeField] RectTransform _pieceLayer;
    [Tooltip("Where loose pieces start.")]
    [SerializeField] RectTransform _spawnArea;
    [Tooltip("Where the scroll outlines are shown.")]
    [SerializeField] RectTransform _silhouetteArea;
    [Tooltip("One outline image per scroll, top to bottom. Children of the silhouette area.")]
    [SerializeField] List<Image> _silhouettes = new List<Image>();

    [Header("Pieces")]
    [SerializeField] PuzzlePiece _piecePrefab;
    [Tooltip("How close (in UI units) a piece must be dropped to its spot to snap in.")]
    [SerializeField] float _snapDistance = 40f;
    [Tooltip("Space between stacked outlines.")]
    [SerializeField] float _silhouetteSpacing = 30f;

    [Header("Events")]
    [SerializeField] UnityEvent<PuzzleSetSO> _onSetLoaded;
    [SerializeField] UnityEvent _onPiecePlaced;
    [SerializeField] UnityEvent<PuzzleSetSO> _onCompleted;

    readonly List<PuzzlePiece> _pieces = new List<PuzzlePiece>();
    PuzzleSetSO _set;
    int _placed;

    public PuzzleSetSO CurrentSet => _set;

    public void LoadSet(PuzzleSetSO set)
    {
        ClearPieces();
        _set = set;
        _placed = 0;
        if (set == null) return;

        float scale = LayoutSilhouettes(set);
        var random = new System.Random(set.id != null ? set.id.GetHashCode() : 0);

        for (int s = 0; s < set.scrolls.Count && s < _silhouettes.Count; s++)
        {
            ScrollDataSO scroll = set.scrolls[s];
            ScrollData data = scroll != null ? scroll.Data : null;
            if (data == null) continue;

            Vector2 silhouetteCenter = ToLayer(_silhouettes[s].rectTransform);

            for (int p = 0; p < scroll.pieceSprites.Count && p < data.pieces.Count; p++)
            {
                Sprite sprite = scroll.pieceSprites[p];
                PieceData piece = data.pieces[p];
                Vector2 size = sprite.rect.size * scale;
                Vector2 target = silhouetteCenter + piece.endPosition * scale;
                Vector2 start = piece.startPosition != Vector2.zero
                    ? ToLayer(_spawnArea) + piece.startPosition * scale
                    : RandomPointIn(_spawnArea, size, random);

                PuzzlePiece instance = Instantiate(_piecePrefab, _pieceLayer);
                instance.name = piece.pieceId;
                instance.Setup(this, _pieceLayer, sprite, size, start, piece.startRotation, target, piece.endRotation);
                _pieces.Add(instance);
            }
        }

        _onSetLoaded.Invoke(set);
    }

    // Called by a piece when it is dropped.
    public void TryPlace(PuzzlePiece piece)
    {
        if (Vector2.Distance(piece.Rect.anchoredPosition, piece.Target) > _snapDistance) return;

        piece.Lock();
        _placed++;
        _onPiecePlaced.Invoke();
        if (_placed >= _pieces.Count) _onCompleted.Invoke(_set);
    }

    void ClearPieces()
    {
        foreach (PuzzlePiece piece in _pieces)
            if (piece != null) Destroy(piece.gameObject);
        _pieces.Clear();
    }

    // Stacks the outlines top to bottom, fitted into the silhouette area. Returns sprite-pixels -> UI scale.
    float LayoutSilhouettes(PuzzleSetSO set)
    {
        Rect area = _silhouetteArea.rect;
        int count = Mathf.Min(set.scrolls.Count, _silhouettes.Count);

        float maxWidth = 0f, totalHeight = 0f;
        for (int i = 0; i < count; i++)
        {
            Sprite outline = set.scrolls[i] != null ? set.scrolls[i].image : null;
            if (outline == null) continue;
            maxWidth = Mathf.Max(maxWidth, outline.rect.width);
            totalHeight += outline.rect.height;
        }
        if (maxWidth <= 0f) return 1f;

        float spacing = _silhouetteSpacing * Mathf.Max(0, count - 1);
        float scale = Mathf.Min(area.width / maxWidth, (area.height - spacing) / totalHeight);

        float y = area.center.y + (totalHeight * scale + spacing) * 0.5f;
        for (int i = 0; i < _silhouettes.Count; i++)
        {
            Image image = _silhouettes[i];
            Sprite outline = i < count && set.scrolls[i] != null ? set.scrolls[i].image : null;
            image.gameObject.SetActive(outline != null);
            if (outline == null) continue;

            Vector2 size = outline.rect.size * scale;
            RectTransform rt = image.rectTransform;
            image.sprite = outline;
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = size;
            rt.anchoredPosition = new Vector2(area.center.x, y - size.y * 0.5f);
            y -= size.y + _silhouetteSpacing;
        }

        return scale;
    }

    // Center of a rect, in the piece layer's local space.
    Vector2 ToLayer(RectTransform rt)
    {
        return _pieceLayer.InverseTransformPoint(rt.TransformPoint(rt.rect.center));
    }

    Vector2 RandomPointIn(RectTransform area, Vector2 pieceSize, System.Random random)
    {
        Rect r = area.rect;
        float halfW = Mathf.Max(0f, (r.width - pieceSize.x) * 0.5f);
        float halfH = Mathf.Max(0f, (r.height - pieceSize.y) * 0.5f);
        Vector2 local = r.center + new Vector2(((float)random.NextDouble() * 2f - 1f) * halfW,
                                               ((float)random.NextDouble() * 2f - 1f) * halfH);
        return _pieceLayer.InverseTransformPoint(area.TransformPoint(local));
    }
}
