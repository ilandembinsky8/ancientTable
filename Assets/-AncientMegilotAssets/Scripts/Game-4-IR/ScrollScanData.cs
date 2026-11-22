using UnityEngine;

[CreateAssetMenu(menuName = "Scrolls/Scan Data")]
public class ScrollScanData : ScriptableObject
{
    public string scrollTitle;
    public Sprite thumbnail;
    public Sprite initialImage;
    public Sprite readableImage;

    public ScanStage[] stages = new ScanStage[4];

    [Header("Final Exposure Target")]
    [Range(0f, 1f)] public float exposureMin = 0.4f;
    [Range(0f, 1f)] public float exposureMax = 0.6f;
}