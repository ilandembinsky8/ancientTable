using System.Collections.Generic;
using UnityEngine;

// One scroll, shared by all games: its text data lives in a JSON file (ScrollData fields),
// images live here because JSON can't hold them.
[CreateAssetMenu(fileName = "Scroll", menuName = "Ancient Megilot/Scroll Data")]
public class ScrollDataSO : ScriptableObject
{
    [Tooltip("JSON with the ScrollData fields.")]
    [SerializeField] TextAsset _json;

    [Tooltip("Full scroll image.")]
    public Sprite image;

    [Tooltip("Puzzle piece images, in the same order as ScrollData.pieces.")]
    public List<Sprite> pieceSprites = new List<Sprite>();

    ScrollData _data;

    // Parsed from the JSON on first access.
    public ScrollData Data
    {
        get
        {
            if (_data == null && _json != null) _data = JsonUtility.FromJson<ScrollData>(_json.text);
            return _data;
        }
    }

    // Re-read the JSON after it was changed in the Inspector.
    void OnValidate()
    {
        _data = null;
    }
}
