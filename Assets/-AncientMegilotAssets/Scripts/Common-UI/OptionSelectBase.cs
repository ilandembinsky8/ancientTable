using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class OptionSelectBase : MonoBehaviour
{
    [Header("Option Select Base")]
    [SerializeField] protected TextMeshProUGUI _optionHeaderText;
    [SerializeField] protected Button _optionButtonPrefab;
    [SerializeField] protected TextMeshProUGUI _buttonText;

    public virtual void SubscribeToActiveOptionButton(UnityAction action)
    {
        _optionButtonPrefab.onClick.AddListener(action);
    }
    public virtual void UnSubscribeFromActiveOptionButton(UnityAction action)
    {
        _optionButtonPrefab.onClick.RemoveListener(action);
    }

}