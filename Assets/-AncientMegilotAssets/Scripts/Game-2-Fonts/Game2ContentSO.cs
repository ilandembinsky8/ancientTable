using System.Collections.Generic;
using UnityEngine;

// All Game 2 content: screen texts, the script list (left) and the scroll list (right), in display order.
[CreateAssetMenu(fileName = "Game2Content", menuName = "Ancient Megilot/Game 2/Content")]
public class Game2ContentSO : ScriptableObject
{
    [Header("Main Header")]
    public string mainTitle;
    public string mainSubtitle;

    [Header("Script List (left)")]
    public string scriptListHeader;
    public string scriptListHint;

    [Header("Scroll List (right)")]
    public string scrollListHeader;

    [Header("Content")]
    public List<ScriptTypeSO> scriptTypes = new List<ScriptTypeSO>();
    public List<ScrollSO> scrolls = new List<ScrollSO>();
}
