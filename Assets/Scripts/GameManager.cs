using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class GameManager : MonoBehaviour
{
    #region Attributes

    [SerializeField] private RecordList _recordList;
    [SerializeField] private RawImage _uiImage;
    [SerializeField] private Texture2D _uiNoSignalImage;

    [Header("   Controller Input")]
    [SerializeField] private int _selectedHour;
    [SerializeField] private int _selectedMinute;
    [SerializeField] private int _selectedCamera;

    [Header("   Max Values")]
    private const int MAX_CAMERA = 3;
    private const int MAX_HOUR = 23;
    private const int MAX_MIN = 59;

    #region Arduino Communication

    // WARNING : COM changes depending on the device
    private const string PORT_NAME = "COM11";
    private const int BAUD_RATE = 19200;
    private SerialPort data_stream = new(PORT_NAME, BAUD_RATE);
    private string receivedData = String.Empty;

    #endregion

    #endregion

    #region Unity API

    void Start()
    {
        data_stream.Open();
    }

    private void Update()
    {
        receivedData = data_stream.ReadLine();

        if (receivedData == String.Empty) return;
        
        ParseReceivedData(receivedData);
        if (!AreReceivedDataValid())
        {
            Debug.LogWarning("Recieved Dara are invalid, can't display anything");
            return;
        }

        UpdateRecord();
    }
    #endregion


    #region Methods
    private void ParseReceivedData(string receivedData)
    {
        string[] receivedDatabuffer = receivedData.Split(',');

        // TODO : parse CameraMode (Normal, HighConstrast, Hint)
        if (receivedDatabuffer.Length >= 3)
        {
            _selectedHour = int.Parse(receivedDatabuffer[0]);
            _selectedMinute = int.Parse(receivedDatabuffer[1]);
            _selectedCamera = int.Parse(receivedDatabuffer[2]);
        }
        else
        {
            Debug.LogWarning("Missing datas from arduino");
        }
    }

    private void UpdateRecord()
    {
        // Search if the Hour + Minute + Camera selected has a record
        foreach (IntervalInfos timeInterval in _recordList._cameras[_selectedCamera]._hours[_selectedHour]._intervalInfos)
        {
            if (MinuteIsInTimeInterval(_selectedMinute, timeInterval._minuteIntervals))
            {
                // Notice : if 2 intervals overlap, only the first one is displayed
                StartDisplayIntervalRecord(timeInterval);
                return;
            } 
        }

        DisplayNothing();
    }

    private bool AreReceivedDataValid()
    {
        return !(_selectedCamera > MAX_CAMERA || _selectedHour > MAX_HOUR || _selectedMinute > MAX_MIN);
    }

    private bool MinuteIsInTimeInterval(int minuteStamp, Vector2 minuteInterval)
    {
        return (minuteStamp >= minuteInterval.x && minuteStamp <= minuteInterval.y);
    }

    private void StartDisplayIntervalRecord(IntervalInfos intervalInfos)
    {
        if (intervalInfos._displayedImage)
        {
            _uiImage.texture = intervalInfos._displayedImage;
        }
        else
        {
            _uiImage.texture = _uiNoSignalImage;
        }

        // TODO : Start Ambiance Sound
        // TODO : Display potential subtitles
    }

    private void DisplayNothing()
    {
        _uiImage.texture = _uiNoSignalImage;
        // TODO : Start Ambiance Sound
        // TODO : Display potential subtitles
    }

    #endregion


}
