using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecordList", menuName = "TheClock/RecordList")]
[Serializable]
public class RecordList : ScriptableObject
{
    public List<RecordData> _cameras;
}


[Serializable]
public class RecordData
{
    public List<Hours> _hours;
}


[Serializable]
public class Hours
{
    public List<IntervalInfos> _intervalInfos;
}


[Serializable]
public class IntervalInfos
{
    public Vector2 _minuteIntervals;
    public string _eventLogText;
    public Subtitle[] _subtitles;

    [NonSerialized] public bool _discovered;
}

[Serializable]
public class Subtitle
{
    public float _duration;
    public string _quote;
}