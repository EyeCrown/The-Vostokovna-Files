using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModeDatas", menuName = "Scriptable Objects/GameModeDatas")]
public class GameModeDatas : ScriptableObject
{
    public List<DataMode> DataModes;
}
