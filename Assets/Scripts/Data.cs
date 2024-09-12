using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "IntData", menuName = "Scriptable Objects/IntData")]
public class IntData : ScriptableObject
{
    public int intValue;

    public int Value
    {
        get => intValue;
        set
        {
            intValue = value;
            OnChange.Invoke();
        }
    }


    public UnityEvent OnChange;

}