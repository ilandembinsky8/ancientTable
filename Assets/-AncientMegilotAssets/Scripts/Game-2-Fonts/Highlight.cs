using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Highlight : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numText;

    public void SetTextAndPos(int n, Vector2 pos)
    {
        numText.text = n.ToString();
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(pos.x, -pos.y);
    }
}
