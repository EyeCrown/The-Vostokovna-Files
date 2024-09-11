using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Attributes

    [SerializeField] private RecordList _recordList;
    [SerializeField] private RawImage _uiImage;
    [SerializeField] private Texture2D _uiNoSignalImage;

    [Header("   Controller Input")]
    [SerializeField] private int _selectedCamera;
    [SerializeField] private int _selectedHour;
    [SerializeField] private int _selectedMinute;

    [Header("   Max Values")]
    private const int MAX_CAMERA = 3;
    private const int MAX_HOUR = 23;
    private const int MAX_MIN = 59;


    #endregion

    #region Unity API

    private void Update()
    {
        if (!AreInputValid())
        {
            Debug.LogWarning("Recieved Input are invalid, can't display anything");
            return;
        }

        UpdateRecord();
    }

    #endregion


    #region Methods

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

    private bool AreInputValid()
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
