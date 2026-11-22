using System;
using UnityEngine;

[Serializable]
public class ScanStage
{
    public string stageName;
    public Color indicatorColor;

    [Range(0f, 1f)] public float wavelengthMin;
    [Range(0f, 1f)] public float wavelengthMax;

    public bool Matches(float wavelength)
    {
        return wavelength >= wavelengthMin && wavelength <= wavelengthMax;
    }
}