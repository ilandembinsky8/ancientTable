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
    public IReadOnlyList<string> ExpectedCharacters => expected;
    public IReadOnlyList<string> EnteredCharacters => entered;

    //private variables
    private readonly List<LetterSlot> slots = new List<LetterSlot>();
    private readonly List<string> entered = new List<string>();

    private List<string> expected = new List<string>();
    private int sentenceIndex;
    private bool transitioning;


    private void Start()
    {
        LoadSentence(0);
    }

    public void Append(string value)
    {
        if (transitioning || expected.Count == 0 || string.IsNullOrEmpty(value))
            return;

        foreach (string element in Split(value))
        {
            if (entered.Count >= expected.Count)
                break;

            entered.Add(element);
        }

        RefreshDisplay();
        //CheckAnswer(); //We can keep it as a comment or enable it if we want to make it work automatically to check after each word if we did spell out like we should have or not
    }

    public void DeleteLast()
    {
        if (transitioning || entered.Count == 0)
            return;

        entered.RemoveAt(entered.Count - 1);
        RefreshDisplay();
    }

    public void Clear()
    {
        if (transitioning)
            return;

        entered.Clear();
        RefreshDisplay();
    }

    public void CheckAnswer()
    {
        if (transitioning || expected.Count == 0 || entered.Count != expected.Count)
            return;

        for (int i = 0; i < expected.Count; i++)
        {
            if (entered[i] != expected[i])
                return;
        }

        transitioning = true;
        StartCoroutine(AdvanceAfterDelay());
    }

    private IEnumerator AdvanceAfterDelay()
    {
        yield return new WaitForSeconds(nextSentenceDelay);
        LoadSentence(sentenceIndex + 1);
    }

    private void LoadSentence(int requestedIndex)
    {
        foreach (LetterSlot slot in slots)
        {
            slot.gameObject.SetActive(false);
            Destroy(slot.gameObject);
        }

        slots.Clear();
        entered.Clear();
        expected.Clear();

        CurrentSentence = "";
        transitioning = false;

        if (sentences == null || sentences.Count == 0)
            return;

        // Skip empty entries and wrap at the end.
        for (int offset = 0; offset < sentences.Count; offset++)
        {
            int index = (requestedIndex + offset) % sentences.Count;

            if (string.IsNullOrWhiteSpace(sentences[index]))
                continue;

            sentenceIndex = index;
            CurrentSentence = sentences[index].Normalize(NormalizationForm.FormC);
            break;
        }

        if (CurrentSentence.Length == 0)
            return;

        expected = Split(CurrentSentence);

        foreach (string element in expected)
        {
            LetterSlot slot = Instantiate(slotPrefab, slotsParent);
            slot.Initialize(element == " ");
            slots.Add(slot);
        }

        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            bool hasInput = i < entered.Count;
            slots[i].Display(hasInput ? entered[i] : "", hasInput && entered[i] == expected[i]);
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