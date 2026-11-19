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

}