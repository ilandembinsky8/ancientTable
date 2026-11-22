using System.Collections.Generic;
using UnityEngine;

// All Game 1 puzzle sets, in display order. Looked up by id.
[CreateAssetMenu(fileName = "PuzzleContent", menuName = "Ancient Megilot/Game 1/Content")]
public class PuzzleContentSO : ScriptableObject
{
    public List<PuzzleSetSO> sets = new List<PuzzleSetSO>();

    // Returns the set whose id matches, or null.
    public PuzzleSetSO GetSet(string id)
    {
        foreach (PuzzleSetSO set in sets)
            if (set != null && set.id == id) return set;
        return null;
    }
}
