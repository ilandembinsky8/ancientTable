using UnityEngine;
using TMPro;

public abstract class GameManagerBase : MonoBehaviour
{
    #region Serialized Fields
    [Header("Game Manager Base")]
    [SerializeField] protected GameObject _mainContainer;
    [SerializeField] protected NavigationBar _navigationBar;
    [SerializeField] protected SideScrollHandler _sideListContainer;
    [SerializeField] protected IntroductionHandler _defaultMainIntroductionContainer;

    [Header("Text Objects")]
    [SerializeField] protected TextMeshProUGUI _mainHeaderWhiteTextObj;
    [SerializeField] protected TextMeshProUGUI _mainHeaderYellowTextObj;
    [SerializeField] protected TextMeshProUGUI _sideHeaderYellowTextObj;

    [Header("Replace with your specific variant")]
    [Tooltip("Replace this prefab with the fitting variant.")]
    [SerializeField] protected OptionSelectBase _sideOptionSelectPrefab;
    [Tooltip("Replace this prefab with the actual game prefab that sits in the main container. This will replace _defaultMainIntroductionContainer once a level is selected and the button is pressed.")]
    [SerializeField] protected GameObject _gameMainPrefab;
    #endregion



    #region Navigation Methods
    public virtual void Home()
    {
        Debug.Log("'Home' Not Implemented");
    }

    public virtual void Help()
    {
        Debug.Log("'Help' Not Implemented");
    }

    public virtual void ChangeLanguage()
    {
        Debug.Log("'ChangeLanguage' Not Implemented");
    }
    #endregion

}