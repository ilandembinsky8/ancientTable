using Unity.VisualScripting;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] GameObject[] gamePrefabs;

    [Header("Games To Load In Quadrant")]
    [SerializeField] int[] gameIDs;

    [Header("Quadrants")]
    [SerializeField] GameObject[] quadrants;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            if (gameIDs[i] > 0 && gameIDs[i] < 7)
                LoadGameToQuadrant(gameIDs[i], i);
        }
    }

    void LoadGameToQuadrant(int gameID, int quadrantNum)
    {
        Instantiate(gamePrefabs[gameID-1], quadrants[quadrantNum].transform);
    }

    void UnloadQuadrant(int quadrant)
    {

    }
}
