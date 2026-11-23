using System;
using UnityEngine;

[Serializable]
public class CTSpecimen
{
    public Sprite specimenImage;
    public bool isScroll;
    public Sprite scanPreview;
    public Sprite resultImage;
    [TextArea] public string resultText;
}
