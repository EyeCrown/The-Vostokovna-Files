using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimePart : MonoBehaviour
{
    #region Attributes

    [SerializeField] private IntData _hourData;
    [SerializeField] private IntData _minuteData;

    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _meridianText;

    [SerializeField] private RawImage iconMeridian;

    [SerializeField] private List<Texture2D> meridianIcons; // 0: moon      1: sun
    
    #endregion
    
    
    #region Unity API

    void Start()
    {
        SetTimeText();
    }

    #endregion

    #region Methods

    string GetHours()
    {
        if (_hourData.Value < 10)
            return $"0{_hourData.Value}";
        if (_hourData.Value > 12)
            return (_hourData.Value - 12).ToString();

        return _hourData.Value.ToString();
    }
    
    void SetTimeText()
    {
        string hours = GetHours();
        string minutes = _minuteData.Value.ToString();

        _timeText.text = $"{hours}:{minutes}";
        _meridianText.text = _hourData.Value < 12 ? "AM" : "PM";
    }

    void SetTimeIcon()
    {
        if (_hourData.Value < 12)
            iconMeridian.texture = meridianIcons[0];
        else
            iconMeridian.texture = meridianIcons[1];
    }

    #endregion

    #region EventHandlers

    private void OnEnable()
    {
        _hourData.OnChange.AddListener(SetTimeText);
        _hourData.OnChange.AddListener(SetTimeIcon);
        
        _minuteData.OnChange.AddListener(SetTimeText);
    }

    private void OnDisable()
    {
        _hourData.OnChange.RemoveListener(SetTimeText);
        _minuteData.OnChange.RemoveListener(SetTimeText);
    }

    #endregion
}
