using UnityEngine;

public class FontIdentifier : MonoBehaviour
{
    Game2Manager game2;
    private void Start()
    {
        game2 = GetComponentInParent<Game2Manager>();
    }

    public void IdentifyCorrect()
    {
        game2.StartTranslate();
    }


}
