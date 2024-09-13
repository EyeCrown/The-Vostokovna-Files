using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModeData", menuName = "Scriptable Objects/ModeData")]
public class DataMode : ScriptableObject
{
    [SerializeField] public string modeName;
    [SerializeField] public int modifier;
    [SerializeField] public int maximum;
}
