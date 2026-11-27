using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PopupWindow : MonoBehaviour
{
    [SerializeField] private GameObject _visualParentObject;
    [SerializeField] private TextMeshProUGUI _popupText;
    [SerializeField] private TextMeshProUGUI _ScrollNameHeb;
    [SerializeField] private TextMeshProUGUI _ScrollNameEng;
    [SerializeField] private TextMeshProUGUI _ScrollTranslation;
    [SerializeField] private TextMeshProUGUI _ScrollTranslationHeader;

    [SerializeField] private TextMeshProUGUI _ScrollLanguageType;
    [SerializeField] private TextMeshProUGUI _ScrollSiteLocation;
    [SerializeField] private TextMeshProUGUI _ScrollHandwriting;
    [SerializeField] private TextMeshProUGUI _ScrollLanguage;
    [SerializeField] private TextMeshProUGUI _ScrollMaterial;
    [SerializeField] private TextMeshProUGUI _ScrollPeriod;

    [SerializeField] private TextMeshProUGUI _continueButtonText;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _closeButton;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(DeactivatePopUp);
        _closeButton.onClick.AddListener(DeactivatePopUp);
    }
    private void OnDisable()
    {
        _continueButton.onClick.RemoveAllListeners();
        _closeButton.onClick.RemoveAllListeners();
    }

    public void SetButtonAction(UnityEngine.Events.UnityAction action) { _continueButton.onClick.AddListener(action); }

    public void ActivatePopUp(string continueButtonText, string popupText, string scrollNameHeb, string scrollNameEng, string scrollTranslation
        , string scrollTranslationHeader, string scrollLanguageType, string scrollSiteLocation, string scrollHandwriting
        , string scrollLanguage, string scrollMaterial, string scrollPeriod)
    {
        _continueButtonText.text = continueButtonText;
        _popupText.text = popupText;
        _ScrollNameHeb.text = scrollNameHeb;
        _ScrollNameEng.text = scrollNameEng;
        _ScrollTranslation.text = scrollTranslation;
        _ScrollTranslationHeader.text = scrollTranslationHeader;
        _ScrollLanguageType.text = scrollLanguageType;
        _ScrollSiteLocation.text = scrollSiteLocation;
        _ScrollHandwriting.text = scrollHandwriting;
        _ScrollLanguage.text = scrollLanguage;
        _ScrollMaterial.text = scrollMaterial;
        _ScrollPeriod.text = scrollPeriod;

        _visualParentObject.SetActive(true);
    }
    public void DeactivatePopUp() { _visualParentObject.SetActive(false); }


}