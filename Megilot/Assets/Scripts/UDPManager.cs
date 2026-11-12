using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class UDPManager : MonoBehaviour
{
    #region UDP Settings
    [SerializeField] private int _fromControllerPort = 12000;
    static private UdpClient _client;
    static private Queue _pQueue = Queue.Synchronized(new Queue()); // holds the packet queue
    private IPEndPoint _endPoint;
    private Thread _listener;
    private volatile bool _isRunning;
    #endregion

    //[SerializeField] private GameManager _gameManager;

    private void Start()
    {
        if (_client == null)
            StartUDP(_fromControllerPort);
    }
    private void Update()
    {
        lock (_pQueue.SyncRoot)
        {
            if (_pQueue.Count > 0)
            {
                Debug.LogError("pQueue > 0");
                object o = _pQueue.Dequeue();
                string message = o as string;
                ProcessMessage(message);
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ProcessMessage("StartA");
        }
    }

    private void OnApplicationQuit()
    {
        EndUDP();
    }

    public void StartUDP(int i_fromControllerPort)
    {
        _fromControllerPort = i_fromControllerPort;
        _endPoint = new IPEndPoint(IPAddress.Any, _fromControllerPort);
        _client = new UdpClient(_endPoint);
        _isRunning = true;
        _listener = new Thread(new ThreadStart(Translater));
        _listener.IsBackground = true;
        _listener.Start();
        Debug.Log("Listening for Data...");
    }

    public void EndUDP()
    {
        _isRunning = false;

        if (_client != null)
        {
            _client.Close();
            _client.Dispose();
            _client = null; // Set to null to avoid any further use
        }

        if (_listener != null && _listener.IsAlive)
        {
            _listener.Join(); // Wait for the listener thread to exit gracefully
        }
    }

    private void Translater()
    {
        Byte[] data = new byte[0];
        while (_isRunning)
        {
            try
            {
                if (_client.Available > 0)
                {
                    data = _client.Receive(ref _endPoint);
                    string msg = Encoding.ASCII.GetString(data);
                    //Debug.LogError(msg);
                    Debug.LogError("UDP incoming " + msg);
                    _pQueue.Enqueue(msg);
                }
            }
            catch (SocketException ex)
            {
                Debug.LogError("Comm.translater, receive data error: " + ex.Message);
            }
            catch (Exception err)
            {
                Debug.LogError("Comm.translater, receive data error: " + err.Message);
                if (_client != null)
                {
                    _client.Close();
                }
                return;
            }
        }
    }

    private async Task UDPSafeKill()
    {
        _isRunning = false; // Stop the listener loop

        if (_client != null)
        {
            _client.Close();
            _client = null; // Nullify to avoid further usage
        }

        if (_listener != null && _listener.IsAlive)
        {
            // Wait for the listener thread to finish processing
            while (_listener.IsAlive)
            {
                await Task.Yield();
            }
        }
    }

    //Adjust according to the rest of your code
    private void ProcessMessage(string message)
    {
        if (message == "StartA")
        {
            //_gameManager.StartGameA(); //load round 1 game
        }
        else
        {
            Debug.Log("Unknown command received: " + message);
        }
    }

}