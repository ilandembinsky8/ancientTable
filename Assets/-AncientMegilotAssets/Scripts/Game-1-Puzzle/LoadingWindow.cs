using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LoadingWindow : MonoBehaviour
{
    [SerializeField] private GameObject _visualParentObject;
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private LoadingBar _loadingBar;
    [SerializeField] private Button _closeButton;

}