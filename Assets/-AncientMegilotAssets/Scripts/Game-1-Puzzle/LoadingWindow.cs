using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LoadingWindow : MonoBehaviour
{
    [SerializeField] private GameObject _visualParentObject;
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private LoadingBar _loadingBar;
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        _loadingBar.SetActionOnLoadingComplete(() => _closeButton.gameObject.SetActive(true));
    }
    private void OnEnable()
    {
        _closeButton.onClick.AddListener(() => _visualParentObject.SetActive(false));
    }
    private void OnDisable()
    {
        _closeButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Activates the loading window and starts the loading process with a default time of 2 seconds. (exists overload for setting exact time)
    /// The loading bar will fill up over the specified time, and the close button will become active once the loading is complete.
    /// </summary>
    /// <param name="loadingText"></param>
    public void StartLoading(string loadingText)
    {
        _loadingText.text = loadingText;
        _visualParentObject.SetActive(true);
        _loadingBar.StartBarLoading(2f);
    }
    /// <summary>
    /// Activates the loading window and starts the loading process with the specified time.
    /// The loading bar will fill up over the specified time, and the close button will become active once the loading is complete.
    /// </summary>
    /// <param name="loadingTime"></param>
    /// <param name="loadingText"></param>
    public void StartLoading(float loadingTime, string loadingText)
    {
        _loadingText.text = loadingText;
        _visualParentObject.SetActive(true);
        _loadingBar.StartBarLoading(loadingTime);
    }

}