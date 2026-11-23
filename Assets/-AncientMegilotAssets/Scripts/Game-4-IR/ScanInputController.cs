using UnityEngine;
using UnityEngine.UI;

public class ScanInputController : MonoBehaviour
{
    [SerializeField] private Slider wavelengthSlider;
    [SerializeField] private ExposureKnob exposureKnob;
    [SerializeField] private bool useArduino;

    //properties
    public float Wavelength => wavelengthSlider.normalizedValue;
    public float Exposure => exposureKnob.Value;


    private void Update()
    {
        ArduinoManager arduino = ArduinoManager.Instance;
        bool arduinoAvailable = useArduino && arduino != null && arduino.isActiveAndEnabled && arduino.HasReading;
        exposureKnob.AllowMouseInput = !arduinoAvailable;

        if (arduinoAvailable)
            exposureKnob.SetValue(arduino.NormalizedValue);
    }
}