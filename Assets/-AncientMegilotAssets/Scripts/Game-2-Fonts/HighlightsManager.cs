using NUnit.Framework;
using System;
using UnityEngine;

public class HighlightsManager : MonoBehaviour
{
    [SerializeField] Highlight highlightPrefab;

    [SerializeField] Vector2[] highlightPositions;

    private void Start()
    {
        Init(highlightPositions);
    }

    void Init(Vector2[] pos)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            Instantiate(highlightPrefab, transform).SetTextAndPos(i + 1, pos[i]);
        }
    }
}
