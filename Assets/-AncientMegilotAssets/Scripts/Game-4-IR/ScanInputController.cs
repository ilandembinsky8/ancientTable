using UnityEngine;
using UnityEngine.UI;

public class ScanInputController : MonoBehaviour
{
    [SerializeField] private Slider wavelengthSlider;
    [SerializeField] private ExposureKnob exposureKnob;
    [SerializeField] private bool useArduino;

    [SerializeField, Range(0.001f, 0.1f)]
    private float arduinoMovementThreshold = 0.01f;

    public float Wavelength => wavelengthSlider.normalizedValue;
    public float Exposure => exposureKnob.Value;

    private bool hasArduinoBaseline;
    private float arduinoBaseline;

    private void Update()
    {
        ArduinoManager arduino = ArduinoManager.Instance;

        if (!useArduino || arduino == null || !arduino.isActiveAndEnabled || !arduino.HasReading)
        {
            hasArduinoBaseline = false;
            return;
        }

        float reading = Mathf.Clamp01(arduino.NormalizedValue);

        // Remember the physical position while the mouse controls the knob.
        if (exposureKnob.IsDragging)
        {
            arduinoBaseline = reading;
            hasArduinoBaseline = true;
            return;
        }

        // Synchronize once when Arduino input first becomes available.
        if (!hasArduinoBaseline)
        {
            arduinoBaseline = reading;
            hasArduinoBaseline = true;
            exposureKnob.SetValue(reading);
            return;
        }

        // Ignore small fluctuations from the physical potentiometer.
        if (Mathf.Abs(reading - arduinoBaseline) >=
            arduinoMovementThreshold)
        {
            arduinoBaseline = reading;
            exposureKnob.SetValue(reading);
        }
    }
}