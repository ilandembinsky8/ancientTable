using UnityEngine;

public class Game2Manager : GameManagerBase
{
    [Header("Game 2")]
    [SerializeField] GameObject leftContainer;
    [SerializeField] GameObject translationGameContainer;
    [SerializeField] GameObject leftKeyboard;


    bool test;

    public void StartIdentify()
    {
        _defaultMainIntroductionContainer.gameObject.SetActive(false);
        leftContainer.SetActive(true);
        _gameMainPrefab.SetActive(true);
    }

    public void StartTranslate()
    {
        ShowLegend();
        translationGameContainer.SetActive(true);
    }

    public void EndScreen()
    {

    }

    void ShowLegend()
    {

    }

    void ShowKeyboard()
    {

    }
}
