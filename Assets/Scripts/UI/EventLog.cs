using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EventLog : MonoBehaviour
{
    [SerializeField] private RecordList _recordList;
    [SerializeField] private GameObject _container;

    [SerializeField] private GameObject _logRegistryPrefab;


    private List<RecordBuffer> buffer = new();

    private void Start()
    {
        InitialiseList();
}

    private void InitialiseList()
    {
        buffer = new();

        // Get the records
        for (int camIndex = 0; camIndex < _recordList._cameras.Count; camIndex++)
        {
            for (int hourIndex = 0; hourIndex < _recordList._cameras[camIndex]._hours.Count; hourIndex++)
            {

                for (int intervalIndex = 0; intervalIndex < _recordList._cameras[camIndex]._hours[hourIndex]._intervalInfos.Count; intervalIndex++)
                {
                    var currentInterval = _recordList._cameras[camIndex]._hours[hourIndex]._intervalInfos[intervalIndex];

                    RecordBuffer test = new();
                    test.cam  = camIndex + 1;
                    test.hour = hourIndex;
                    test.minute = (int) ((currentInterval._minuteIntervals.x + currentInterval._minuteIntervals.y) / 2);
                    test.info = "Info to fill";
                    buffer.Add(test);
                }

            }
        }


        // Order the record buffer
        SortRecordBuffer(buffer);

        // Display log registry in record buffer
        foreach (var record in buffer)
        {
            GameObject logRegistryBuffer = Instantiate(_logRegistryPrefab, _container.transform);
            LogRegistry logRegistry = logRegistryBuffer.GetComponent<LogRegistry>();
            logRegistry.TimeField.text = record.hour + " : " + record.minute;
            logRegistry.InfoField.text = record.info;
            logRegistry.CamField.text = "" + record.cam;

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


