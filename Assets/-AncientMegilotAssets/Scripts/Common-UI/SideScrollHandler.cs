using System.Collections.Generic;
using UnityEngine;
using System;

public class SideScrollHandler : MonoBehaviour
{
    public Action OnAnySideOptionSelected;// add ID for the option select card
    private List<OptionSelectBase> _optionButtons = new List<OptionSelectBase>();

    private void Start()// remove adding buttons from here to when the buttons actually spawn.
    {
        foreach (OptionSelectBase optionButton in gameObject.GetComponentsInChildren<OptionSelectBase>())
        {
            AddOptionButton(optionButton);
            optionButton.SubscribeToActiveOptionButton(OnAnyOptionButtonPressed);
        }
        Debug.Log($"<color=yellow>SideScrollHandler: Initialized with {_optionButtons.Count} option buttons</color>");
    }
    private void OnDisable()
    {
        foreach (OptionSelectBase optionButton in _optionButtons)
        {
            optionButton.UnSubscribeFromActiveOptionButton(OnAnyOptionButtonPressed);
        }
        _optionButtons.Clear();
    }

    public void AddOptionButton(OptionSelectBase optionButton)
    {
        _optionButtons.Add(optionButton);
    }

    private void OnAnyOptionButtonPressed() { OnAnySideOptionSelected?.Invoke(); }

}