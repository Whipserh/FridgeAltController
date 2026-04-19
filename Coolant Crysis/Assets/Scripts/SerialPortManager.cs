using System;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Threading;

using TMPro;
using UnityEngine;



public class InputCommand
{
    public string command;
    public string state;
}

public class SerialPortManager : MonoBehaviour
{
    [Header("Robot Object")]
    [SerializeField] private CircuitManager bot;

    [Header("Serial Port Settings")]
    [SerializeField] private string portName = "COM3";
    [SerializeField] private int baudRate = 115200;
    [SerializeField] private int readTimeoutMS = 200;

    [Header("Debug")]
    [SerializeField] private TMP_Text outputText;


    private SerialPort port;
    private Thread thread;
    private volatile bool running;

    private readonly ConcurrentQueue<string> rxQueue = new();

    private void OnEnable()
    {
        OpenPort();
    }

    private void Update()
    {
        while (rxQueue.TryDequeue(out string line))
        {
            if (outputText) outputText.text = line;
            try
            {
                //translate the command from a JSON to a command class
                InputCommand rxCommand = JsonUtility.FromJson<InputCommand>(line);

                //check to see if it is empty or if command is empty
                if (rxCommand == null || string.IsNullOrEmpty(rxCommand.command))
                    continue;//not readable JSON, so move to next line


                bot.updateValues(rxCommand.command, rxCommand.state);
                
            }
            catch (ArgumentException) { Debug.Log("Found an invald command in the queue line"); }
            
        }
    }

    private void OnDisable()
    {
        ClosePort();
    }

    private void OpenPort()
    {
        ClosePort();

        port = new SerialPort(portName, baudRate)
        {
            Handshake = Handshake.None,
            ReadTimeout = readTimeoutMS,
            WriteTimeout = 200,
            NewLine = "\n",
            DtrEnable = true,
            RtsEnable = false
        };

        try
        {
            port.Open();
        }
        catch (Exception ex)
        {
            rxQueue.Enqueue($"Serial port failed to open: {ex.GetType().Name} - {ex.Message}");
            return;
        }

        running = true;
        thread = new Thread(ReadLoop) { IsBackground = true };
        thread.Start();

        Application.quitting -= ClosePort;
        Application.quitting += ClosePort;

        rxQueue.Enqueue($"Serial port connection established on port {portName}");
    }

    private void ReadLoop()
    {
        while (running)
        {
            try
            {
                string line = port.ReadLine();
                rxQueue.Enqueue(line);
            }
            catch (TimeoutException) { /* expected exception */ }
            catch (Exception ex)
            {
                rxQueue.Enqueue($"Failed to read from serial port : {ex.GetType().Name} - {ex.Message}");
                break;
            }
        }
    }

    private void ClosePort()
    {
        running = false;

        try
        {
            if (port is { IsOpen: true })
                port.Close();
        }
        catch { /* ignored */ }

        if (thread != null)
        {
            if (!thread.Join(500))
                thread.Interrupt();

            thread = null;
        }

        port?.Dispose();
        port = null;
    }
}
