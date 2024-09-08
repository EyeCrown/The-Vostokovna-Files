using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "TheClock/CameraData")]
public class CameraData : ScriptableObject
{
    [SerializeField]
    public CameraState[] states = new CameraState[12];
}

