using System;
using System.IO.Ports;
using UnityEngine;

public class ArduinoManager : MonoBehaviour
{
    [Header("Serial Port Settings")]
    [SerializeField] private string portName = "COM9";
    [SerializeField] private int potentiometerValue;

    //properties
    public static ArduinoManager Instance { get; private set; }// Singleton instance
    public int PotentiometerValue => potentiometerValue;
    public float NormalizedValue => potentiometerValue / 1023f;

    //private variables
    private SerialPort _port;
    private string _receivedText = "";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);//it's on comment because it will all be on the same scene for now.
    }

    private void OnEnable()
    {
        _receivedText = "";

        try
        {
            _port = new SerialPort(portName, 9600);
            _port.Open();
            Debug.Log("Arduino connected!");
        }
        catch (Exception exception)
        {
            Debug.LogError("Could not connect: " + exception.Message);
            enabled = false;
        }
    }

    private void OnDisable()
    {
        if (_port == null)
            return;

        try
        {
            _port.Dispose();
        }
        catch (Exception exception)
        {
            Debug.LogWarning("Could not close serial port: " + exception.Message);
        }
        finally
        {
            _port = null;
        }
    }

    private void Update()
    {
        if (_port == null || !_port.IsOpen)
            return;

        try
        {
            _receivedText += _port.ReadExisting();

            int newlineIndex;

            while ((newlineIndex = _receivedText.IndexOf('\n')) >= 0)
            {
                string line = _receivedText.Substring(0, newlineIndex).Trim();
                _receivedText = _receivedText.Substring(newlineIndex + 1);

                if (int.TryParse(line, out int value))
                {
                    potentiometerValue = value;
                }
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("Arduino read failed: " + exception.Message);
            enabled = false;
        }
    }
}
