using System;
using UnityEngine;
using UnityEngine.UI;

public class NavigationBar : MonoBehaviour
{
    GameManagerBase gameManager;

    [SerializeField] Button homeButton;
    [SerializeField] Button helpButton;
    [SerializeField] Button changeLanguageButton;

    private void Start()
    {
        gameManager = GetComponentInParent<GameManagerBase>();

        homeButton.onClick.AddListener(gameManager.Home);
        helpButton.onClick.AddListener(gameManager.Help);
        changeLanguageButton.onClick.AddListener(gameManager.ChangeLanguage);
    }
}
