using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BoolData", menuName = "Scriptable Objects/BoolData")]
public class BoolData : ScriptableObject
{
    [NonSerialized] private bool boolValue;

    public bool Value
    {
        get => boolValue;
        set
        {
            boolValue = value;
            OnChange.Invoke();
        }
    }

    public UnityEvent OnChange;
}