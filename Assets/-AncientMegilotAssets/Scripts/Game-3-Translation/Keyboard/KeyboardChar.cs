using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class KeyboardChar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private char keyboardChar;
    [SerializeField] private TextMeshProUGUI charText;

    public void SetFont(TMP_FontAsset font)
    {
        if (charText == null || font == null)
            return;

        charText.font = font;
    }

    public void SetFontSize(float size)
    {
        if (charText == null || size <= 0)
            return;

        charText.fontSize = size;
    }


    private void OnValidate()
    {
        if (charText == null)
            charText = GetComponentInChildren<TextMeshProUGUI>();
        charText.text = keyboardChar.ToString();
    }
}
