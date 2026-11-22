using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterSlot : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI characterText;
    [SerializeField] private Image underline;
    [SerializeField] private LayoutElement layout;

    [Header("Colors")]
    [SerializeField] private Color correctColor;
    [SerializeField] private Color wrongColor;

    public void Initialize(bool isSpace)
    {
        characterText.text = "";
        characterText.richText = false;

        underline.gameObject.SetActive(!isSpace);
        //layout.preferredWidth = isSpace ? 24 : 44; //just default settings we did on the object itself already
    }

    public void Display(string value, bool correct)
    {
        characterText.text = value;
        characterText.color = correct ? correctColor : wrongColor;

        if (value == " " && !correct)
            characterText.text = "·";
    }
}