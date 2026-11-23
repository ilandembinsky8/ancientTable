using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CTGameController : MonoBehaviour
{
    [Header("Specimen selection")]
    [SerializeField] private CTSpecimen[] specimens;
    [SerializeField] private Button[] specimenButtons;
    [SerializeField] private Button activateButton;
    [SerializeField] private Image specimenImage;

    [Header("Scanner")]
    [SerializeField] private CTScanner scanner;
    [SerializeField] private Collider2D[] regionColliders;
    [Min(0.01f)]
    [SerializeField] private float scanSeconds = 2f;
    [Min(0.01f)]
    [SerializeField] private float decaySeconds = 2f;
    [SerializeField] private GameObject scanningPanel;
    [SerializeField] private Image scanPreview;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text progressLabel;

    [Header("Results")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Image resultImage;
    [SerializeField] private TMP_Text resultTitle;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private string scrollTitle = "Scroll";
    [SerializeField] private string coalTitle = "Coal";

    private CTScanRegion[] _regions;
    private int _selectedIndex = -1;
    private bool _scanning;

    private void Awake()
    {
        _regions = new CTScanRegion[regionColliders.Length];
        for (int i = 0; i < _regions.Length; i++)
            _regions[i] = new CTScanRegion();

        for (int i = 0; i < specimenButtons.Length; i++)
        {
            int index = i;
            specimenButtons[i].onClick.AddListener(() => SelectSpecimen(index));
        }
        activateButton.onClick.AddListener(StartScan);
        scanner.RegionContactChanged += OnRegionContactChanged;
        ResetToSelection();
    }

    public void ResetToSelection()
    {
        _selectedIndex = -1;
        ResetScan();
    }

    public void SelectSpecimen(int index)
    {
        if (index < 0 || index >= specimens.Length)
            return;

        _selectedIndex = index;
        ResetScan();
    }

    public void StartScan()
    {
        if (_selectedIndex < 0 || _scanning || _regions.Length == 0)
            return;

        ResetScan();
        CTSpecimen specimen = specimens[_selectedIndex];
        SetImage(specimenImage, specimen.specimenImage);
        SetImage(scanPreview, specimen.scanPreview);
        scanningPanel.SetActive(true);
        activateButton.interactable = false;
        _scanning = true;
        scanner.SetScanning(true);
    }

    private void ResetScan()
    {
        _scanning = false;
        scanner.SetScanning(false);
        scanner.ResetPosition();
        StopAllCoroutines();
        foreach (CTScanRegion region in _regions)
        {
            region.Progress = 0f;
            region.Routine = null;
        }

        SetImage(specimenImage, null);
        scanningPanel.SetActive(false);
        resultPanel.SetActive(false);
        activateButton.interactable = _selectedIndex >= 0;
        UpdateProgress();
    }

    private void OnRegionContactChanged(Collider2D collider, bool touching)
    {
        if (!_scanning)
            return;

        int index = Array.IndexOf(regionColliders, collider);
        if (index < 0)
            return;

        CTScanRegion region = _regions[index];
        if (region.Progress >= 1f)
            return;

        if (region.Routine != null)
        {
            StopCoroutine(region.Routine);
            region.Routine = null;
        }

        if (touching || region.Progress > 0f)
            region.Routine = StartCoroutine(ChangeRegionProgress(region, touching));
    }

    private IEnumerator ChangeRegionProgress(CTScanRegion region, bool touching)
    {
        float target = touching ? 1f : 0f;
        float duration = Mathf.Max(0.01f, touching ? scanSeconds : decaySeconds);

        while (region.Progress != target)
        {
            yield return null;
            region.Progress = Mathf.MoveTowards(
                region.Progress, target, Time.unscaledDeltaTime / duration);
            if (UpdateProgress())
            {
                region.Routine = null;
                ShowResult();
                yield break;
            }
        }

        region.Routine = null;
    }

    private bool UpdateProgress()
    {
        float total = 0f;
        bool completed = _regions.Length > 0;
        foreach (CTScanRegion region in _regions)
        {
            total += region.Progress;
            completed &= region.Progress >= 1f;
        }

        float progress = _regions.Length > 0 ? total / _regions.Length : 0f;
        progressSlider.value = progress;
        int percent = completed ? 100 : Mathf.Min(99, Mathf.FloorToInt(progress * 100f));
        progressLabel.text = $"{percent}%";
        return completed;
    }

    private void ShowResult()
    {
        _scanning = false;
        scanner.SetScanning(false);
        scanningPanel.SetActive(false);
        CTSpecimen specimen = specimens[_selectedIndex];
        SetImage(resultImage, specimen.resultImage);
        resultTitle.text = specimen.isScroll ? scrollTitle : coalTitle;
        resultText.text = specimen.resultText;
        resultPanel.SetActive(true);
        activateButton.interactable = true;
    }

    private static void SetImage(Image image, Sprite sprite)
    {
        image.sprite = sprite;
        image.enabled = sprite != null;
    }
}
