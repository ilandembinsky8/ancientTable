using UnityEngine;
using UnityEngine.UI;

public class Game2Identify : MonoBehaviour
{
    [SerializeField] Image scrollGraphic;
    [SerializeField] Sprite[] scrollSprites;
    [SerializeField] HighlightsManager highlightsObject;



    private void Start()
    {
        Init(0, default); //temporary for initializing scroll data
    }

    public void Init(int scroll, Vector2[] highlights)
    {
        scrollGraphic.sprite = scrollSprites[scroll];

        //highlightsObject.SetPositions(highlightPosArray);
    }

    public void ShowHighlights()
    {
        highlightsObject.gameObject.SetActive(true);

    }

    public Button GetHighlightsButton()
    {
        return highlightsObject.GetComponent<Button>();
    }
}
