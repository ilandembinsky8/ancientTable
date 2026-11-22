using TMPro;
using UnityEngine;

// One writing system in the "identify the script" list (e.g. Square Hebrew, Phoenician, Greek).
[CreateAssetMenu(fileName = "ScriptType", menuName = "Ancient Megilot/Game 2/Script Type")]
public class ScriptTypeSO : ScriptableObject
{
    [Tooltip("Unique id used to look this script type up, e.g. \"square-hebrew\".")]
    public string id;

    [Tooltip("Name shown in the list, e.g. \"כתב עברי מרובע\".")]
    public string displayName;

    [Tooltip("Sample letters drawn with DisplayFont, e.g. \"ABCDE\".")]
    public string sampleText;

    [Tooltip("Font that draws the sample letters in this script.")]
    public TMP_FontAsset displayFont;

    // Replaced by sampleText + displayFont. Kept for reference in case images are used again.
    // [Tooltip("Image of the sample letters.")]
    // public Sprite sampleSprite;
}
