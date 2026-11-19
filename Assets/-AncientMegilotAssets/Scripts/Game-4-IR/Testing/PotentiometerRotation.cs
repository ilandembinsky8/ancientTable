using UnityEngine;

public class PotentiometerRotation : MonoBehaviour
{
    [SerializeField] private float minimumAngle = 0f;
    [SerializeField] private float maximumAngle = 360f;

    private Quaternion _initialRotation;

    private void Awake()
    {
        _initialRotation = transform.localRotation;
    }

    private void Update()
    {
        float angle = Mathf.Lerp(minimumAngle, maximumAngle, ArduinoManager.Instance.NormalizedValue);
        transform.localRotation =_initialRotation * Quaternion.AngleAxis(angle, Vector3.forward);
    }
}