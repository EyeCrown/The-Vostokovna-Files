using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllCameraData", menuName = "Scriptable Objects/AllCameraData")]
public class AllCameraData : ScriptableObject
{
    public List<CameraData> cameraDataList;
}
