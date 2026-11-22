using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Fills a set card on Start from the PuzzleSetSO whose id matches _setId.
// Hook a Button's onClick to Select(); wire On Selected to e.g. PuzzleBoard.LoadSet.
public class PuzzleSetCard : MonoBehaviour
{
    [Serializable]
    public class ScrollLine
    {
        [Tooltip("Shows numberAndNameEnglish. Use a left-to-right text object.")]
        public TextMeshProUGUI id;
        [Tooltip("Shows \" - \" + nameHebrew.")]
        public TextMeshProUGUI description;
    }

    [SerializeField] UnityEvent<PuzzleSetSO> _onSelected;

    [Header("Data")]
    [SerializeField] PuzzleContentSO _content;
    [Tooltip("PuzzleSetSO.id, e.g. \"Murabbaat\".")]
    [SerializeField] string _setId;

    [Header("Texts (optional)")]
    [SerializeField] TextMeshProUGUI _titleText;
    [Tooltip("One line per scroll in the set, top to bottom.")]
    [SerializeField] List<ScrollLine> _lines = new List<ScrollLine>();
    [SerializeField] TextMeshProUGUI _piecesText;
    [Tooltip("{0} is replaced with the piece count.")]
    [SerializeField] string _piecesFormat = "{0} חלקים";

    public PuzzleSetSO Set { get; private set; }

    void Start()
    {
        Load(_setId);
    }

    public void Select()
    {
        if (Set != null) _onSelected.Invoke(Set);
    }

    // Can also be called right after Instantiate to give a spawned card its id.
    public void Load(string setId)
    {
        _setId = setId;
        Set = _content != null ? _content.GetSet(setId) : null;

        if (Set == null)
        {
            Debug.LogWarning($"[{nameof(PuzzleSetCard)}] No puzzle set with id '{setId}' on '{name}'.", this);
            return;
        }

        if (_titleText != null) _titleText.text = Set.title;

        for (int i = 0; i < _lines.Count; i++)
        {
            ScrollData data = i < Set.scrolls.Count && Set.scrolls[i] != null ? Set.scrolls[i].Data : null;
            ScrollLine line = _lines[i];
            if (line.id != null) line.id.text = data != null ? data.numberAndNameEnglish : "";
            if (line.description != null) line.description.text = data != null ? " - " + data.nameHebrew : "";
        }

        if (_piecesText != null)
        {
            string count = Set.PieceCount.ToString();
            // TMP's right-to-left mode reverses digits, so feed them reversed to show them correctly.
            if (_piecesText.isRightToLeftText) count = Reverse(count);
            _piecesText.text = string.Format(_piecesFormat, count);
        }
    }

    static string Reverse(string value)
    {
        char[] chars = value.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}
