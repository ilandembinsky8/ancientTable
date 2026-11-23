using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public class WordingManager : MonoBehaviour
{
    [Header("Content")]
    [SerializeField] private List<string> sentences = new List<string>();

    [Header("Display")]
    [SerializeField] private Transform slotsParent;
    [SerializeField] private LetterSlot slotPrefab;

    [Header("Completion")]
    [SerializeField, Min(0)] private float nextSentenceDelay = 0.8f;

    //properties
    public string CurrentSentence { get; private set; } = "";
    public IReadOnlyList<string> ExpectedCharacters => _expected;
    public IReadOnlyList<string> EnteredCharacters => _entered;

    //private variables
    private readonly List<LetterSlot> _slots = new List<LetterSlot>();
    private readonly List<string> _entered = new List<string>();

    private List<string> _expected = new List<string>();
    private int _sentenceIndex;
    private bool _transitioning;


    private void Start()
    {
        LoadSentence(0);
    }

    public void Append(string value)
    {
        if (_transitioning || _expected.Count == 0 || string.IsNullOrEmpty(value))
            return;

        foreach (string element in Split(value))
        {
            if (_entered.Count >= _expected.Count)
                break;

            _entered.Add(element);
        }

        RefreshDisplay();
        //CheckAnswer(); //We can keep it as a comment or enable it if we want to make it work automatically to check after each word if we did spell out like we should have or not
    }

    public void DeleteLast()
    {
        if (_transitioning || _entered.Count == 0)
            return;

        _entered.RemoveAt(_entered.Count - 1);
        RefreshDisplay();
    }

    public void Clear()
    {
        if (_transitioning)
            return;

        _entered.Clear();
        RefreshDisplay();
    }

    public void CheckAnswer()
    {
        if (_transitioning || _expected.Count == 0 || _entered.Count != _expected.Count)
            return;

        for (int i = 0; i < _expected.Count; i++)
        {
            if (_entered[i] != _expected[i])
                return;
        }

        _transitioning = true;
        StartCoroutine(AdvanceAfterDelay());
    }

    private IEnumerator AdvanceAfterDelay()
    {
        yield return new WaitForSeconds(nextSentenceDelay);
        LoadSentence(_sentenceIndex + 1);
    }

    private void LoadSentence(int requestedIndex)
    {
        foreach (LetterSlot slot in _slots)
        {
            slot.gameObject.SetActive(false);
            Destroy(slot.gameObject);
        }

        _slots.Clear();
        _entered.Clear();
        _expected.Clear();

        CurrentSentence = "";
        _transitioning = false;

        if (sentences == null || sentences.Count == 0)
            return;

        // Skip empty entries and wrap at the end.
        for (int offset = 0; offset < sentences.Count; offset++)
        {
            int index = (requestedIndex + offset) % sentences.Count;

            if (string.IsNullOrWhiteSpace(sentences[index]))
                continue;

            _sentenceIndex = index;
            CurrentSentence = sentences[index].Normalize(NormalizationForm.FormC);
            break;
        }

        if (CurrentSentence.Length == 0)
            return;

        _expected = Split(CurrentSentence);

        foreach (string element in _expected)
        {
            LetterSlot slot = Instantiate(slotPrefab, slotsParent);
            slot.Initialize(element == " ");
            _slots.Add(slot);
        }

        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            bool hasInput = i < _entered.Count;
            _slots[i].Display(hasInput ? _entered[i] : "", hasInput && _entered[i] == _expected[i]);
        }
    }

    private static List<string> Split(string value)
    {
        var result = new List<string>();
        var enumerator = StringInfo.GetTextElementEnumerator(value.Normalize(NormalizationForm.FormC));

        while (enumerator.MoveNext())
            result.Add(enumerator.GetTextElement());

        return result;
    }
}