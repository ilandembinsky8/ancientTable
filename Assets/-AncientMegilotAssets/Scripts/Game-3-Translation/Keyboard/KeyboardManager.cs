using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private WordingManager wordingManager;

    [Header("Regular Letter Keys Only")]
    [SerializeField] private List<KeyboardChar> keyboardChars;
    [SerializeField] private TMP_FontAsset keyboardFont;
    [SerializeField] private float fontSize = 60;

    public void Append(string value)
    {
        if (wordingManager != null)
            wordingManager.Append(value);
    }

    public void DeleteLast()
    {
        if (wordingManager != null)
            wordingManager.DeleteLast();
    }

    public void Clear()
    {
        if (wordingManager != null)
            wordingManager.Clear();
    }

    public void CheckAnswer()
    {
        if (wordingManager != null)
            wordingManager.CheckAnswer();
    }

    private void OnValidate()
    {
        if (keyboardChars == null)
            return;

        foreach (KeyboardChar character in keyboardChars)
        {
            if (character == null)
                continue;

            character.SetFont(keyboardFont);
            character.SetFontSize(fontSize);
        }
    }
}