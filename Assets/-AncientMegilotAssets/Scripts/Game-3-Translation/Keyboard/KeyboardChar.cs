using TMPro;
using UnityEngine;

public class KeyboardChar : MonoBehaviour
{
    [Header("Label")]
    [SerializeField] private string label;
    [SerializeField] private TextMeshProUGUI charText;

    public string Label => label;

    public void SetFont(TMP_FontAsset font)
    {
        if (charText != null && font != null)
            charText.font = font;
    }

    public void SetFontSize(float size)
    {
        if (charText != null && size > 0)
            charText.fontSize = size;
    }

    private void OnValidate()
    {
        if (charText == null)
            charText = GetComponentInChildren<TextMeshProUGUI>(true);

        if (charText != null)
            charText.text = label;
    }
}