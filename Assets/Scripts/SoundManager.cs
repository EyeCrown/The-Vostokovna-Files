using JetBrains.Annotations;
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
        AkSoundEngine.PostEvent("PlayTickClockwise", gameObject);
    }


    private void OnMinuteChanges()
    {
        Debug.Log("Minute changed : " + _selectedMinute.Value);
        AkSoundEngine.PostEvent("PlayTickClockwiseMinutes", gameObject);
    }

    private void OnCameraChanges()
    {
        Debug.Log("Camera changed : " + _selectedCamera.Value); 
        AkSoundEngine.PostEvent("PlayCameraNext", gameObject); 

        if (_isDisplayingNoSignal)
        {
            Debug.Log("Displaying No Signal");
        }
    }

    public void SoundOnHelpNeeded()
    {
        AkSoundEngine.PostEvent("PlayHelpButton", gameObject);
    }


    private void OnRecordChanges(string recordName)
    {
        Debug.Log("Record changed : "  + recordName);
        AkSoundEngine.PostEvent(recordName, gameObject);
    }
    private void OnNoSignal()
    {
        Debug.Log("NoSignal Invoqued");
        Debug.Log("Current Camera is : " + _selectedCamera.Value);
        if (_selectedCamera.Value == 0){
            AkSoundEngine.PostEvent("Cam1Offline", gameObject);
        }
        if (_selectedCamera.Value == 1)
        {
            AkSoundEngine.PostEvent("Cam2Offline", gameObject);
        }
        if (_selectedCamera.Value == 2)
        {
            AkSoundEngine.PostEvent("Cam4Offline", gameObject);
        }
        if (_selectedCamera.Value == 3)
        {
            AkSoundEngine.PostEvent("Cam6Offline", gameObject);
        }
    

       
    }


}
