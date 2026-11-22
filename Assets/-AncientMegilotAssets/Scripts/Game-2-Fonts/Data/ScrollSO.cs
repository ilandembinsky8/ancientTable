using UnityEngine;

// One scroll the player can pick to decipher.
[CreateAssetMenu(fileName = "Scroll", menuName = "Ancient Megilot/Game 2/Scroll")]
public class ScrollSO : ScriptableObject
{
    [Tooltip("e.g. \"איגרת בר כוכבא\".")]
    public string title;

    [Tooltip("e.g. \"מערת קומראן 4\".")]
    public string location;

    [Tooltip("Catalog id in logical order, e.g. \"Hev 57\".")]
    public string catalogId;

    [Tooltip("Scroll image shown in the main area.")]
    public Sprite image;

    [Tooltip("The script the player must choose to win.")]
    public ScriptTypeSO correctScript;

    public bool IsCorrect(ScriptTypeSO script) => script != null && script == correctScript;
}
