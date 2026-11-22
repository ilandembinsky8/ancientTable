using UnityEngine;

public class Game2Manager : GameManagerBase
{
    [Header("Game 2")]
    [SerializeField] GameObject leftContainer;
    [SerializeField] GameObject translationGameContainer;
    [SerializeField] GameObject leftKeyboard;
    [SerializeField] GameObject keyboardOverlay;
    [SerializeField] GameObject endScreenPopup;


    Game2Identify game2Identify;
    bool identifying;

    private void Start()
    {
        GameStart();
        game2Identify = _gameMainPrefab.GetComponent<Game2Identify>();
        game2Identify.GetHighlightsButton().onClick.AddListener(StartTranslateSegment);
    }

    public void GameStart()
    {
        _defaultMainIntroductionContainer.gameObject.SetActive(true);

        leftContainer.SetActive(false);
        _gameMainPrefab.SetActive(false);
        keyboardOverlay.SetActive(false);
        endScreenPopup.SetActive(false);
    }

    public void StartIdentify() //show scroll, magnifier and font selector
    {
        _defaultMainIntroductionContainer.gameObject.SetActive(false);
        leftContainer.SetActive(true);
        _gameMainPrefab.SetActive(true);

        identifying = true;
    }

    public void StartTranslate() //show legend and segments to translate
    {
        game2Identify.ShowHighlights();
        //ShowLegend();
        //translationGameContainer.SetActive(true);
    }

    public void StartTranslateSegment() //pull keyboard overlay and focus on segment
    {
        keyboardOverlay.SetActive(true);
    }

    public void EndScreen() //lower keyboard
    {
        keyboardOverlay.SetActive(false);
        endScreenPopup.SetActive(true);
    }

    void ShowLegend()
    {
        leftKeyboard.SetActive(true);
    }

    void ShowKeyboard()
    {

    }
}
