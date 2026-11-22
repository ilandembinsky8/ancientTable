using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Sits on Scroll-Container (Game-2-Identify-Container).
// ShowScroll(scroll): shows the chosen scroll. CheckScript(script): checks the player's choice against it.
// Both take one argument, so they can be wired straight from a UnityEvent in the Inspector.
public class FontScrollHandler : MonoBehaviour
{
    [Tooltip("Image that shows the scroll.")]
    [SerializeField] Image _scrollImage;

    [Header("Events")]
    [SerializeField] UnityEvent<ScrollSO> _onScrollShown;
    [SerializeField] UnityEvent<ScriptTypeSO> _onCorrect;
    [SerializeField] UnityEvent<ScriptTypeSO> _onWrong;

    public ScrollSO CurrentScroll { get; private set; }

    public void ShowScroll(ScrollSO scroll)
    {
        CurrentScroll = scroll;

        if (_scrollImage != null)
        {
            _scrollImage.sprite = scroll != null ? scroll.image : null;
            _scrollImage.enabled = _scrollImage.sprite != null;
        }

        _onScrollShown.Invoke(scroll);
    }

    public void CheckScript(ScriptTypeSO script)
    {
        if (CurrentScroll == null) return;

        if (IsCorrect(script)) _onCorrect.Invoke(script);
        else _onWrong.Invoke(script);
    }

    public bool IsCorrect(ScriptTypeSO script)
    {
        return CurrentScroll != null && CurrentScroll.IsCorrect(script);
    }
}
