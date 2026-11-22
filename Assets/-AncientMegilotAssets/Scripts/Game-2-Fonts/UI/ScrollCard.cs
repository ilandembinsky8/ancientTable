using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Fills a scroll card on Start from the ScrollSO whose catalogId matches _scrollId.
// Hook a Button's onClick to Select(); wire On Selected to whatever should react (e.g. FontScrollHandler.ShowScroll).
public class ScrollCard : MonoBehaviour
{
    [SerializeField] UnityEvent<ScrollSO> _onSelected;

    [SerializeField] Game2ContentSO _content;
    [Tooltip("catalogId of the scroll to show, e.g. \"Hev 57\".")]
    [SerializeField] string _scrollId;

    [Header("Texts (optional)")]
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _locationText;
    [Tooltip("Use a left-to-right text object: TMP's RTL mode reverses Latin letters and digits.")]
    [SerializeField] TextMeshProUGUI _catalogIdText;

    public ScrollSO Scroll { get; private set; }

    void Start()
    {
        Load(_scrollId);
    }

    // Can also be called right after Instantiate to give a spawned card its id.
    public void Load(string scrollId)
    {
        _scrollId = scrollId;
        Scroll = _content != null ? _content.GetScroll(scrollId) : null;

        if (Scroll == null)
        {
            Debug.LogWarning($"[{nameof(ScrollCard)}] No scroll with id '{scrollId}' on '{name}'.", this);
            return;
        }

        SetText(_titleText, Scroll.title);
        SetText(_locationText, Scroll.location);
        SetText(_catalogIdText, Scroll.catalogId);
    }

    public void Select()
    {
        if (Scroll != null) _onSelected.Invoke(Scroll);
    }

    static void SetText(TextMeshProUGUI textObj, string value)
    {
        if (textObj != null) textObj.text = value;
    }
}
