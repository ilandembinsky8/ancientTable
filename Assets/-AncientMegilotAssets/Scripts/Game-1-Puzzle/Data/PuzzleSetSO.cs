using System.Collections.Generic;
using UnityEngine;

// One puzzle set (מקבץ): a title and the scrolls whose pieces are assembled together.
[CreateAssetMenu(fileName = "PuzzleSet", menuName = "Ancient Megilot/Game 1/Puzzle Set")]
public class PuzzleSetSO : ScriptableObject
{
    [Tooltip("Same as ScrollData.puzzleId of its scrolls, e.g. \"Murabbaat\".")]
    public string id;

    [Tooltip("Card title, e.g. \"מערת מורבעת\".")]
    public string title;

    [Tooltip("Scrolls in this set, top to bottom on the board.")]
    public List<ScrollDataSO> scrolls = new List<ScrollDataSO>();

    public int PieceCount
    {
        get
        {
            int count = 0;
            foreach (ScrollDataSO scroll in scrolls)
                if (scroll != null) count += scroll.pieceSprites.Count;
            return count;
        }
    }
}
