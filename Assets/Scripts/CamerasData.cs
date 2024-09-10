using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraList", menuName = "CameraList")]
[Serializable]
public class CameraList : ScriptableObject
{
    public List<CamerasData> _cameras;
}


[Serializable]
public class CamerasData
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
    public RenderTexture _displayedImage;
}