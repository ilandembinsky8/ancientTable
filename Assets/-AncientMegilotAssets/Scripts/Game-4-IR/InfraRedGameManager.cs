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

    private ScrollScanData currentScroll;
    private int currentStageIndex;
    private float timeInsideTarget;
    private ScanState state = ScanState.Selecting;



    private void Start()
    {
        RestartGame();
    }

    public void RestartGame()
    {
        state = ScanState.Selecting;
        currentScroll = null;
        currentStageIndex = 0;
        timeInsideTarget = 0f;

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
        if (state != ScanState.Selecting)
            return;

        if (index < 0 || index >= scrolls.Length)
            return;

        ScrollScanData selectedScroll = scrolls[index];

        if (!IsValidScroll(selectedScroll))
        {
            Debug.LogError("Scroll data or checkmark setup is invalid.", this);
            return;
        }

        currentScroll = selectedScroll;
        currentStageIndex = 0;
        timeInsideTarget = 0f;
        state = ScanState.Wavelength;

        ClearCheckmarks();

        scrollImage.sprite = currentScroll.initialImage;
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
        if (currentScroll == null)
            return;

        bool matches;

        if (state == ScanState.Wavelength)
        {
            matches = currentScroll.stages[currentStageIndex]
                .Matches(scanInput.Wavelength);
        }
        else if (state == ScanState.Exposure)
        {
            float exposure = scanInput.Exposure;

            matches = exposure >= currentScroll.exposureMin &&
                      exposure <= currentScroll.exposureMax;
        }
        else
        {
            return;
        }

        if (!matches)
        {
            timeInsideTarget = 0f;
            return;
        }

        timeInsideTarget += Time.deltaTime;

        if (timeInsideTarget < confirmationDuration)
            return;

        timeInsideTarget = 0f;

        if (state == ScanState.Wavelength)
            CompleteCurrentStage();
        else
            CompleteScroll();
    }

    private void CompleteCurrentStage()
    {
        stageCheckmarks[currentStageIndex].SetActive(true);
        currentStageIndex++;

        if (currentStageIndex >= currentScroll.stages.Length)
            state = ScanState.Exposure;

        ShowCurrentInstruction();
    }

    private void CompleteScroll()
    {
        state = ScanState.Won;

        scrollImage.sprite = currentScroll.readableImage;
        instructionText.text = "הסריקה הושלמה!";

        scanControls.SetActive(false);
        winPanel.SetActive(true);

        onScrollCompleted.Invoke();
    }

    private void ShowCurrentInstruction()
    {
        if (state == ScanState.Exposure)
        {
            instructionText.text = "כוונו את החשיפה כדי לחשוף את הכתב";
            return;
        }

        ScanStage stage = currentScroll.stages[currentStageIndex];
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
        timeInsideTarget = 0f;
    }
}