using UnityEngine;

public class Game1Manager : GameManagerBase
{
    private const string LOADING_FIRST_PIECE = "הפעולה שהשלמתם כאן בכמה שניות, דורשת במציאות שנים רבות של מחקר מאומץ.\r\nכיום, כלים מתקדמים של בינה מלאכותית )IA( מסייעים לחוקרים ומאיצים באופן דרמטי את תהליך הפענוח.";
    private const string LOADING_SECOND_PIECE = "הודות למלאכת ההרכבה, נחשפים לנגד עינינו טקסטים קדומים – וביניהם מזמורים אבודים וסודות היסטוריים שהיו חבויים שנים רבות";
    private const string POPUP_NEXT_BUTTON_TEXT = "המשיכו במלאכת ההרכבה";
    private const string POPUP_FINISHED_BUTTON_TEXT = "בחרו פאזל נוסף";

    [Header("Game 1 Variables")]
    [SerializeField] private LoadingWindow _loadingWindow;
    [SerializeField] private PopupWindow _popupWindow;

    private PuzzleBoard _puzzleBoard;
    private int _currentPuzzleIndex = 0;
    bool _wasFirstPiecePlaced = false, _wasSecondPiecePlaced = false;

    private void Awake()
    {
        _puzzleBoard = _gameMainPrefab.GetComponent<PuzzleBoard>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _puzzleBoard.SubscribeToAnyPiecePlaced(OnAnyPiecePlaced);
        _puzzleBoard.SubscribeToAnyPuzzleCompleted(OnAnyPuzzleCompleted);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _puzzleBoard.UnsubscribeFromAnyPiecePlaced(OnAnyPiecePlaced);
        _puzzleBoard.UnsubscribeFromAnyPuzzleCompleted(OnAnyPuzzleCompleted);
    }

    private void OnAnyPuzzleCompleted(ScrollDataSO completedPuzzleSO)
    {
        ScrollData completedPuzzleData = completedPuzzleSO.Data;
        string currentPopUpButtonText = "BTN text";

        if (_currentPuzzleIndex == 0)
        {
            currentPopUpButtonText = POPUP_NEXT_BUTTON_TEXT;
            _currentPuzzleIndex++;
        }
        else
        {
            _currentPuzzleIndex = 0;
            currentPopUpButtonText = POPUP_FINISHED_BUTTON_TEXT;
            _popupWindow.SetButtonAction(OnGameEnded);
        }

        _popupWindow.ActivatePopUp(currentPopUpButtonText, completedPuzzleData.info, completedPuzzleData.nameHebrew,
            completedPuzzleData.numberAndNameEnglish, completedPuzzleData.translation, completedPuzzleData.translationHeader,
            completedPuzzleData.handwritingType, completedPuzzleData.site, completedPuzzleData.script,
            completedPuzzleData.language, completedPuzzleData.material, completedPuzzleData.period);
    }
    private void OnAnyPiecePlaced(int arg0)
    {
        if (!_wasFirstPiecePlaced)
        {
            _wasFirstPiecePlaced = true;
            _loadingWindow.StartLoading(LOADING_FIRST_PIECE);
        }
        else if (_wasFirstPiecePlaced && !_wasSecondPiecePlaced)
        {
            _wasSecondPiecePlaced = true;
            _loadingWindow.StartLoading(LOADING_SECOND_PIECE);
        }
    }

}