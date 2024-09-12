using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private IntData _selectedHour;
    [SerializeField] private IntData _selectedMinute;
    [SerializeField] private IntData _selectedCamera;
    [SerializeField] private BoolData _isDisplayingNoSignal;




    private void OnEnable()
    {
        _selectedHour.OnChange.AddListener(OnHourChanges);
        _selectedMinute.OnChange.AddListener(OnMinuteChanges);
        _selectedCamera.OnChange.AddListener(OnCameraChanges);
        GameManager.OnRecordChanges += OnRecordChanges;
        GameManager.OnNoSignal += OnNoSignal;
    }

    private void OnDisable()
    {
        _selectedHour.OnChange.RemoveListener(OnHourChanges);
        _selectedMinute.OnChange.RemoveListener(OnMinuteChanges);
        _selectedCamera.OnChange.RemoveListener(OnCameraChanges);
        GameManager.OnRecordChanges -= OnRecordChanges;
    }

    private void Start()
    {
        
    }

    private void OnHourChanges()
    {
        Debug.Log("Hour changed : " + _selectedHour.Value);
    }


    private void OnMinuteChanges()
    {
        Debug.Log("Minute changed : " + _selectedMinute.Value);


    }

    private void OnCameraChanges()
    {
        Debug.Log("Camera changed : " + _selectedCamera.Value);
        if (_isDisplayingNoSignal)
        {
            Debug.Log("Displaying No Signal");
        }
    }


    private void OnRecordChanges(string recordName)
    {
        Debug.Log("Record changed : "  + recordName);
    }
    private void OnNoSignal()
    {

        Debug.Log("NoSignal Invoqued");
        Debug.Log("Current Camera is : " + _selectedCamera.Value);
    }


}
