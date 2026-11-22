using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Fills a font item on Start from the ScriptTypeSO whose id matches _scriptTypeId.
// Selected by the wheel (ScrollCenterScaler) when it locks in the center, not by clicking:
// shows the selected sprite and fires On Selected (e.g. FontScrollHandler.CheckScript).
public class ScriptTypeButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] UnityEvent<ScriptTypeSO> _onSelected;

    [Header("Highlight")]
    [SerializeField] Image _background;
    [Tooltip("Sprite shown while this item is the selected one.")]
    [SerializeField] Sprite _selectedSprite;

    [Header("Data")]
    [SerializeField] Game2ContentSO _content;
    [Tooltip("id of the script type to show (ScriptTypeSO.id).")]
    [SerializeField] string _scriptTypeId;

    [Header("Texts (optional)")]
    [SerializeField] TextMeshProUGUI _nameText;
    [Tooltip("Shows sampleText in the script type's displayFont.")]
    [SerializeField] TextMeshProUGUI _sampleText;

    public ScriptTypeSO ScriptType { get; private set; }

    Sprite _normalSprite;

    void Awake()
    {
        if (_background != null) _normalSprite = _background.sprite;
    }

    void Start()
    {
        Load(_scriptTypeId);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetHighlight(true);
        if (ScriptType != null) _onSelected.Invoke(ScriptType);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetHighlight(false);
    }

    void SetHighlight(bool on)
    {
        if (_background != null) _background.sprite = on && _selectedSprite != null ? _selectedSprite : _normalSprite;
    }

    // Can also be called right after Instantiate to give a spawned button its id.
    public void Load(string scriptTypeId)
    {
        _scriptTypeId = scriptTypeId;
        ScriptType = _content != null ? _content.GetScriptType(scriptTypeId) : null;

        if (ScriptType == null)
        {
            Debug.LogWarning($"[{nameof(ScriptTypeButton)}] No script type with id '{scriptTypeId}' on '{name}'.", this);
            return;
        }

        if (_nameText != null) _nameText.text = ScriptType.displayName;

        if (_sampleText != null)
        {
            _sampleText.text = ScriptType.sampleText;
            if (ScriptType.displayFont != null) _sampleText.font = ScriptType.displayFont;
        }
    }
}
