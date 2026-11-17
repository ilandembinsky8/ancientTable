using UnityEngine;
using TMPro;

public abstract class GameManagerBase : MonoBehaviour
{
    #region Serialized Fields
    [Header("Game Manager Base")]
    [SerializeField] protected GameObject _mainContainer;
    [SerializeField] protected GameObject _sideListContainer;
    [SerializeField] protected GameObject _defaultMainIntroductionContainer;
    [Header("Text Objects")]
    [SerializeField] protected TextMeshProUGUI _mainHeaderWhiteTextObj;
    [SerializeField] protected TextMeshProUGUI _mainHeaderYellowTextObj;
    [SerializeField] protected TextMeshProUGUI _sideHeaderYellowTextObj;
    [Header("Replace Prefab to fit correct game variant")]
    [SerializeField] protected GameObject _sideOptionSelectPrefab;
    [SerializeField] protected GameObject _thisGamePrefab;
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