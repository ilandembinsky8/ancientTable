using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class KeyboardManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<KeyboardChar> keyboardChars;

    [Header("Settings")]
    [SerializeField] private TMP_FontAsset keyboardFont;

    private void OnValidate()
    {
        if (keyboardFont == null || keyboardChars == null)
            return;

        foreach (KeyboardChar character in keyboardChars)
        {
            if (character != null)
                character.SetFont(keyboardFont);
        }
    }
}
