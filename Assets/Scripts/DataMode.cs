using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModeDatas", menuName = "Scriptable Objects/GameModeDatas")]
public class GameModeDatas : ScriptableObject
{
    public List<DataMode> DataModes;
}


[CreateAssetMenu(fileName = "ModeData", menuName = "Scriptable Objects/ModeData")]
public class DataMode : ScriptableObject
{
    [SerializeField] public string modeName;
    [SerializeField] public int modifier;
    [SerializeField] public int maximum;
}
