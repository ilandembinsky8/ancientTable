using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Fills a font button on Start from the ScriptTypeSO whose id matches _scriptTypeId.
// Hook the Button's onClick to Select(); wire On Selected to whatever should react (e.g. FontScrollHandler.CheckScript).
public class ScriptTypeButton : MonoBehaviour
{
    [SerializeField] UnityEvent<ScriptTypeSO> _onSelected;

    [SerializeField] Game2ContentSO _content;
    [Tooltip("id of the script type to show (ScriptTypeSO.id).")]
    [SerializeField] string _scriptTypeId;

    [Header("Texts (optional)")]
    [SerializeField] TextMeshProUGUI _nameText;
    [Tooltip("Shows sampleText in the script type's displayFont.")]
    [SerializeField] TextMeshProUGUI _sampleText;

    public ScriptTypeSO ScriptType { get; private set; }

    void Start()
    {
        Load(_scriptTypeId);
    }

    public void Select()
    {
        if (ScriptType != null) _onSelected.Invoke(ScriptType);
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
