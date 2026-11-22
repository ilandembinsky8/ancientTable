using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class IntroductionHandler : MonoBehaviour
{
    const string INTRODUCTION_HEB_BUTTON_TEXT = "בחרו מקבץ להרכבה";
    const string INTRODUCTION_ENG_BUTTON_TEXT = "text tba";
    const string SELECTED_INTRODUCTION_HEB_BUTTON_TEXT = "התחילו במלאכה";
    const string SELECTED_INTRODUCTION_ENG_BUTTON_TEXT = "text tba";

    [SerializeField] private TextMeshProUGUI _introductionText;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Button _introductionButton;

    public void OnOptionSelected()
    {
        _buttonText.text = SELECTED_INTRODUCTION_HEB_BUTTON_TEXT;
    }
    public void ResetInstructions()
    {
        _buttonText.text = INTRODUCTION_HEB_BUTTON_TEXT;
    }

    public void SubscribeToIntroductionButtonPressed(UnityAction action)
    {
        _introductionButton.onClick.AddListener(action);
    }
    public void UnSubscribeFromIntroductionButtonPressed(UnityAction action)
    {
        _introductionButton.onClick.RemoveListener(action);
    }

}