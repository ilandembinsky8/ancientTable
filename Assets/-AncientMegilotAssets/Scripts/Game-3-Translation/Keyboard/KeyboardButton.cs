using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(KeyboardChar))]
public class KeyboardButton : MonoBehaviour
{
    [SerializeField] private KeyboardManager keyboard;
    [SerializeField] private KeyAction action;

    //private variables
    private Button _button;
    private KeyboardChar _character;

    private void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();

        if (_character == null)
            _character = GetComponent<KeyboardChar>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    private void HandleClick()
    {
        if (keyboard == null)
            return;

        switch (action)
        {
            case KeyAction.Character:
                keyboard.Append(_character.Label);
                break;

            case KeyAction.Space:
                keyboard.Append(" ");
                break;

            case KeyAction.Delete:
                keyboard.DeleteLast();
                break;

            case KeyAction.Detect:
                keyboard.CheckAnswer();
                break;
        }
    }
}