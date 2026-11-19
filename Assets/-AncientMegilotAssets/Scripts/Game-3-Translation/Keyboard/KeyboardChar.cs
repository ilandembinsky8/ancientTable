using UnityEngine;
using TMPro;
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



    private void OnValidate()
    {
        if (charText == null)
            charText = GetComponentInChildren<TextMeshProUGUI>();
        charText.text = keyboardChar.ToString();
    }
}
