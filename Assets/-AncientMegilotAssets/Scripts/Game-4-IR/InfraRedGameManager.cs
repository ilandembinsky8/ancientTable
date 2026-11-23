using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InfraRedGameManager : MonoBehaviour
{
    private enum ScanState
    {
        Selecting,
        Wavelength,
        Exposure,
        Won
    }

    [Header("Scroll Data")]
    [SerializeField] private ScrollScanData[] scrolls;

    [Header("References")]
    [SerializeField] private ScanInputController scanInput;
    [SerializeField] private Image scrollImage;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private GameObject[] stageCheckmarks;

    [Header("Screen State")]
    [SerializeField] private GameObject selectionPrompt;
    [SerializeField] private GameObject scanControls;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button[] scrollSelectionButtons;

    [Header("Scanning")]
    [SerializeField, Min(0.1f)]
    private float confirmationDuration = 0.5f;

    [Header("Events")]
    [SerializeField] private UnityEvent onScrollCompleted = new UnityEvent();

    private ScrollScanData _currentScroll;
    private int _currentStageIndex;
    private float _timeInsideTarget;
    private ScanState _state = ScanState.Selecting;
    private bool _exposureReady;


    private void Start()
    {
        RestartGame();
    }

    public void RestartGame()
    {
        _state = ScanState.Selecting;
        _currentScroll = null;
        _currentStageIndex = 0;
        _timeInsideTarget = 0f;
        _exposureReady = false;

        ClearCheckmarks();

        scrollImage.sprite = null;
        scrollImage.enabled = false;

        selectionPrompt.SetActive(true);
        scanControls.SetActive(false);
        winPanel.SetActive(false);

        SetScrollSelectionEnabled(true);
        instructionText.text = "בחרו מגילה לסריקה";
    }

    private void SetScrollSelectionEnabled(bool interactable)
    {
        foreach (Button button in scrollSelectionButtons)
        {
            if (button != null)
                button.interactable = interactable;
        }
    }

    public void SelectScroll(int index)
    {
        if (_state != ScanState.Selecting)
            return;

        if (index < 0 || index >= scrolls.Length)
            return;

        ScrollScanData selectedScroll = scrolls[index];

        if (!IsValidScroll(selectedScroll))
        {
            Debug.LogError("Scroll data or checkmark setup is invalid.", this);
            return;
        }

        _currentScroll = selectedScroll;
        _currentStageIndex = 0;
        _timeInsideTarget = 0f;
        _state = ScanState.Wavelength;

        ClearCheckmarks();

        scrollImage.sprite = _currentScroll.initialImage;
        scrollImage.preserveAspect = true;
        scrollImage.enabled = true;

        selectionPrompt.SetActive(false);
        scanControls.SetActive(true);
        winPanel.SetActive(false);

        SetScrollSelectionEnabled(false);
        ShowCurrentInstruction();
    }

    private void Update()
    {
        if (_currentScroll == null)
            return;

        bool matches;

        if (_state == ScanState.Wavelength)
        {
            matches = _currentScroll.stages[_currentStageIndex]
                .Matches(scanInput.Wavelength);
        }
        else if (_state == ScanState.Exposure)
        {
            float exposure = scanInput.Exposure;

            bool insideTarget =
                exposure >= _currentScroll.exposureMin &&
                exposure <= _currentScroll.exposureMax;

            // First observe the knob outside the target.
            // Then the player can enter it to confirm.
            if (!_exposureReady)
            {
                if (!insideTarget)
                    _exposureReady = true;

                _timeInsideTarget = 0f;
                return;
            }

            matches = insideTarget;
        }
        else
        {
            return;
        }

        if (!matches)
        {
            _timeInsideTarget = 0f;
            return;
        }

        _timeInsideTarget += Time.deltaTime;

        if (_timeInsideTarget < confirmationDuration)
            return;

        _timeInsideTarget = 0f;

        if (_state == ScanState.Wavelength)
            CompleteCurrentStage();
        else
            CompleteScroll();
    }

    private void CompleteCurrentStage()
    {
        stageCheckmarks[_currentStageIndex].SetActive(true);
        _currentStageIndex++;

        if (_currentStageIndex >= _currentScroll.stages.Length)
        {
            _state = ScanState.Exposure;
            _exposureReady = false;
            _timeInsideTarget = 0f;
        }

        ShowCurrentInstruction();
    }

    private void CompleteScroll()
    {
        _state = ScanState.Won;

        scrollImage.sprite = _currentScroll.readableImage;
        instructionText.text = "הסריקה הושלמה!";

        scanControls.SetActive(false);
        winPanel.SetActive(true);

        onScrollCompleted.Invoke();
    }

    private void ShowCurrentInstruction()
    {
        if (_state == ScanState.Exposure)
        {
            instructionText.text = "כוונו את החשיפה כדי לחשוף את הכתב";
            return;
        }

        ScanStage stage = _currentScroll.stages[_currentStageIndex];
        instructionText.text = $"כוונו את אורך הגל: {stage.stageName}";
    }

    private void ClearCheckmarks()
    {
        foreach (GameObject checkmark in stageCheckmarks)
        {
            if (checkmark != null)
                checkmark.SetActive(false);
        }
    }

    private bool IsValidScroll(ScrollScanData data)
    {
        if (data == null || data.initialImage == null || data.readableImage == null ||
            data.stages == null || data.stages.Length != 4 || stageCheckmarks == null || stageCheckmarks.Length != 4)
        {
            return false;
        }

        for (int i = 0; i < data.stages.Length; i++)
        {
            ScanStage stage = data.stages[i];

            if (stage == null || stageCheckmarks[i] == null || stage.wavelengthMin > stage.wavelengthMax)
                return false;
        }

        if (data.exposureMin > data.exposureMax)
            return false;

        return true;
    }

    private void OnDisable()
    {
        _timeInsideTarget = 0f;
    }
}