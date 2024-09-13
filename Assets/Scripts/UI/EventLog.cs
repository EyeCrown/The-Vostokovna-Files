using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventLog : MonoBehaviour
{
    [SerializeField] private RecordList _recordList;
    [SerializeField] private GameObject _infoContainer;
    [SerializeField] private GameObject _eventLogContainer;

    [SerializeField] private GameObject _logUpdateUI;
    [SerializeField] private int _logUpdateDuration;
    private Coroutine _logUpdateCoroutine;

    [SerializeField] private GameObject _logRegistryPrefab;
    [SerializeField] private IntData _selectedCamera;

    [SerializeField] private List<LogRegistry> _logRegistries = new();


    private List<RecordBuffer> buffer = new();

    private void Start()
    {
        InitialiseList();
    }

    private void OnEnable()
    {
        GameManager.OnRecordWithInfoDisplay += RevealInfo;
        _selectedCamera.OnChange.AddListener(OnCameraChanges);
    }

    private void OnDisable()
    {
        GameManager.OnRecordWithInfoDisplay -= RevealInfo;
        _selectedCamera.OnChange.RemoveListener(OnCameraChanges);
    }

    private void InitialiseList()
    {
        buffer = new();
        _logRegistries = new();

        // Get the records
        for (int camIndex = 0; camIndex < _recordList._cameras.Count; camIndex++)
        {
            for (int hourIndex = 0; hourIndex < _recordList._cameras[camIndex]._hours.Count; hourIndex++)
            {

                for (int intervalIndex = 0; intervalIndex < _recordList._cameras[camIndex]._hours[hourIndex]._intervalInfos.Count; intervalIndex++)
                {
                    var currentInterval = _recordList._cameras[camIndex]._hours[hourIndex]._intervalInfos[intervalIndex];

                    if (currentInterval._eventLogText == "")
                        continue;


                    RecordBuffer test = new();
                    test.cam = camIndex + 1;
                    test.hour = hourIndex;
                    test.minute = (int)((currentInterval._minuteIntervals.x + currentInterval._minuteIntervals.y) / 2);
                    test.info = currentInterval._eventLogText;

                    buffer.Add(test);
                }
            }
        }


        // Order the record buffer
        SortRecordBuffer(buffer);

        // Display log registry in record buffer
        foreach (var record in buffer)
        {
            GameObject logRegistryBuffer = Instantiate(_logRegistryPrefab, _infoContainer.transform);
            LogRegistry logRegistry = logRegistryBuffer.GetComponent<LogRegistry>();
            logRegistry.TimeField.text = record.hour + " : " + record.minute;
            logRegistry.InfoField.text = record.info;
            logRegistry.CamField.text = "" + record.cam;
            _logRegistries.Add(logRegistry);
        }
    }

    private void RevealInfo(string infoText)
    {
        foreach (var logRegistry in _logRegistries)
        {
            if (logRegistry.InfoField.text == infoText)
            {
                if (_logUpdateCoroutine!= null)
                {
                    StopCoroutine( _logUpdateCoroutine );
                }

                _logUpdateCoroutine = StartCoroutine("EnableAndDisableLogUpdateUI");
                logRegistry.RevealInfoText();
                logRegistry.EnableNotification(true);
                break;
            }
        }
    }

    public IEnumerator EnableAndDisableLogUpdateUI()
    {
        _logUpdateUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(_logUpdateDuration);
        _logUpdateUI.gameObject.SetActive(false);
    }




    [ContextMenu("Reveal Every Infos")]
    public void RevealEveryInfos()
    {
        foreach (var logRegistry in _logRegistries)
        {
            logRegistry.RevealInfoText();
        }
    }

    private void OnCameraChanges()
    {
        Debug.Log(_selectedCamera.Value);
        if (_selectedCamera.Value == 5)
        {
            _eventLogContainer.gameObject.SetActive(true);
        } else
        {
            if (_eventLogContainer.gameObject.active == true)
            {
                _eventLogContainer.gameObject.SetActive(false);
                RemoveAllNotifications();
            }
        }
    }

    public void RemoveAllNotifications()
    {
        foreach (var logRegistry in _logRegistries)
        {
            logRegistry.EnableNotification(false);
        }
    }


    public struct RecordBuffer
    {
        public int hour;
        public int cam;
        public int minute;
        public string info;
    }
    public static void SortRecordBuffer(List<RecordBuffer> recordList)
    {
        recordList.Sort((x, y) => (x.hour * 60 + x.minute).CompareTo(y.hour * 60 + y.minute));
    }
}


