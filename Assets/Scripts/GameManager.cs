using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region Attributes

    [SerializeField] private RecordList _recordList;
    [SerializeField] private RawImage _uiImage;
    [SerializeField] private Texture2D _uiNoSignalImage;

    [Header("   Controller Input")] 
    [SerializeField] private IntData[] _currentDatas;
    
    [SerializeField] private IntData _selectedHour;
    [SerializeField] private IntData _selectedMinute;
    [SerializeField] private IntData _selectedCamera;

    [SerializeField] private string _currentCameraImageName;
    
    [Header("   Max Values")]
    private const int MAX_CAMERA = 5;
    private const int MAX_HOUR = 23;
    private const int MAX_MIN = 59;


    [Header("Subtitles")]
    [SerializeField] private TextMeshProUGUI _subtitleField;
    private Coroutine _subtitlesCoroutine;

    #region Arduino Communication
    [Header("   Arduino Communication")]
    [SerializeField] private bool _useArduino;
    
    // WARNING : COM changes depending on the device
    private const string PORT_NAME = "COM11";
    private const int BAUD_RATE = 19200;
    private SerialPort data_stream = new(PORT_NAME, BAUD_RATE);
    private string receivedData = String.Empty;

    #endregion

    #region KeyboardControls

    [SerializeField] private IntData _currentMode;
    [SerializeField] private GameModeDatas _gameModeDatas;

    [SerializeField] private GameObject _uiModePart;
    #endregion
    
    private const string camImageFolder = "Images/";

    #region Events
    static public event Action<IntervalInfos> OnRecordWithInfoDisplay;

    #endregion

    #endregion

    #region Unity API

    void Start()
    {
        _currentCameraImageName = String.Empty;
        
        if (_useArduino)
            SetupArduino();
        else
            SetupKeyboard();
    }

    private void Update()
    {
        
        if (_useArduino)
            UpdateArduino();
        else
            UpdateKeyboard();

        //UpdateRecord();
    }
    #endregion


    #region Methods

    void SetupArduino()
    {
        _uiModePart.SetActive(false);
        data_stream.Open();
    }

    bool UpdateArduino()
    {
        receivedData = data_stream.ReadLine();

        if (receivedData == String.Empty) 
            return false;
        
        ParseReceivedData(receivedData);
        if (!AreReceivedDataValid())
        {
            Debug.LogWarning("Recieved Dara are invalid, can't display anything");
            return false;
        }

        return true;
    }


    void SetupKeyboard()
    {
        _uiModePart.SetActive(true);
    }

    bool UpdateKeyboard()
    {
        // TODO: do 
        return true;
    }


    private void ParseReceivedData(string receivedData)
    {
        string[] receivedDatabuffer = receivedData.Split(',');

        // TODO : parse CameraMode (Normal, HighConstrast, Hint)
        if (receivedDatabuffer.Length >= 3)
        {
            for (int i = 0; i < _currentDatas.Length; i++)
                _currentDatas[i].Value = int.Parse(receivedDatabuffer[i]);
            
            // _selectedHour.Value = int.Parse(receivedDatabuffer[0]);
            // _selectedMinute.Value = int.Parse(receivedDatabuffer[1]);
            // _selectedCamera.Value = int.Parse(receivedDatabuffer[2]);
        }
        else
        {
            Debug.LogWarning("Missing datas from arduino");
        }
    }

    private void UpdateRecord()
    {
        if (_subtitlesCoroutine != null) StopCoroutine(_subtitlesCoroutine);
        _subtitleField.text = "";

        // Search if the Hour + Minute + Camera selected has a record
        foreach (IntervalInfos timeInterval in _recordList._cameras[_selectedCamera.Value]._hours[_selectedHour.Value]._intervalInfos)
        {
            if (MinuteIsInTimeInterval(_selectedMinute.Value, timeInterval._minuteIntervals))
            {
                int idCamLog = 6;
                if (_selectedCamera.Value == idCamLog)
                {
                    //TODO: Display Camera logger
                    
                    return;
                }

                string cameraId = (_selectedCamera.Value + 1).ToString();
                string hour     = _selectedHour.Value < 10 ? $"0{_selectedHour.Value}" : _selectedHour.Value.ToString();
                string minute   = timeInterval._minuteIntervals.x < 10 ? $"0{timeInterval._minuteIntervals.x}" : timeInterval._minuteIntervals.x.ToString();
                
                string filename = $"C{cameraId}_{hour}_{minute}";

                if (filename == _currentCameraImageName)
                    return;
                    
                DisplayTextureFile(filename);
                
                _currentCameraImageName = filename;


                if (timeInterval._eventLogText != "")
                {
                    OnRecordWithInfoDisplay?.Invoke(timeInterval);
                }
                timeInterval._discovered = true;

                // Display Subtitles
                if (timeInterval._subtitles.Length > 0)
                {
                    _subtitlesCoroutine = StartCoroutine(DisplaySubtitles(timeInterval._subtitles));
                }
                return;
            } 
        }

        DisplayNothing();
    }

    public IEnumerator DisplaySubtitles(Subtitle[] subtitles)
    {
        yield return new WaitForSeconds(0.5f);

        foreach(Subtitle sub in subtitles)
        {
            _subtitleField.text = sub._quote;
            yield return new WaitForSeconds(sub._duration);
            _subtitleField.text = "";
            yield return new WaitForSeconds(0.06f);
        }

        _subtitleField.text = "";
    }

    private bool AreReceivedDataValid()
    {
        return !(_selectedCamera.Value > MAX_CAMERA || _selectedHour.Value > MAX_HOUR || _selectedMinute.Value > MAX_MIN);
    }

    private bool MinuteIsInTimeInterval(int minuteStamp, Vector2 minuteInterval)
    {
        return (minuteStamp >= minuteInterval.x && minuteStamp <= minuteInterval.y);
    }


    void DisplayTextureFile(string filename)
    {
        
        Debug.Log($"Try loading {filename}");
      
        Texture2D cameraImage = Resources.Load<Texture2D>($"{camImageFolder}{filename}");

        if (cameraImage == null)
        {
            Debug.Log("File doesn't exists. Displaying No Signal.");
            DisplayNothing();
            return;
        }

        _uiImage.texture = cameraImage;

        
    }


    private void DisplayNothing()
    {
        _uiImage.texture = _uiNoSignalImage;
        
        _currentCameraImageName = String.Empty;
        
        // TODO : Start Ambiance Sound
        // TODO : Display potential subtitles
    }
    
    
    void UseHelp()
    {
        if (_currentCameraImageName != null)
        {
            DisplayTextureFile($"{_currentCameraImageName}_help");
        }
    }

    
    #endregion

    #region EventHandlers

    private void OnEnable()
    {
        foreach (var data in _currentDatas)
        {
            data.OnChange.AddListener(UpdateRecord);
        }
    }

    private void OnDisable()
    {
        foreach (var data in _currentDatas)
        {
            data.OnChange.RemoveListener(UpdateRecord);
        }
    }


    public void OnChangeMode(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        int value = (int) context.ReadValue<float>();
        
        _currentMode.Value = mod(_currentMode.Value + value, _gameModeDatas.DataModes.Count);
        
        
    }
    
    public void OnChangeValue(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
        int value = (int) context.ReadValue<float>();

        int k = _currentDatas[_currentMode.Value].Value +
                value * _gameModeDatas.DataModes[_currentMode.Value].modifier;
        int n = _gameModeDatas.DataModes[_currentMode.Value].maximum;
        
        _currentDatas[_currentMode.Value].Value = mod(k, n);
        
    }

    public void OnHelpNeeded(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        UseHelp();
    }

    
    #endregion
    
    int mod(int k, int n) {  return ((k %= n) < 0) ? k+n : k;  }
}
