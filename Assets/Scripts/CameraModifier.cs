using System;
using UnityEngine;
using UnityEngine.Events;

public class CameraModifier : MonoBehaviour
{
    #region Attributes

    private const float baseFOV = 60f;
    
    private Camera _camera;
    
    [SerializeField] private CameraState currentState;
    
    #endregion

    #region Events

    [HideInInspector]
    public UnityEvent<CameraState> UpdateState;

    #endregion

    #region Unity API

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Start()
    {
        
    }

    #endregion

    #region Methods

    void SetCameraOn()
    {
        _camera.enabled = true;
        _camera.fieldOfView = baseFOV;
        
        DebugLog();
    }
    
    void SetCameraOff()
    {
        _camera.enabled = false;

        DebugLog();
    }
    
    void SetCameraBlurry()
    {
        _camera.fieldOfView = 140f;
        
        DebugLog();
    }

    void DebugLog()
    {
        string message = $"{gameObject.name} state: {currentState.ToString()} \n" +
                         $"Camera component: {_camera.enabled}\n" +
                         $"Current FOV: {_camera.fieldOfView}";
        Debug.Log(message);
    }


    #endregion

    #region EventHandlers

    private void OnEnable()
    {
        UpdateState.AddListener(OnUpdateState);
    }

    private void OnDisable()
    {
        UpdateState.RemoveListener(OnUpdateState);
    }


    void OnUpdateState(CameraState newState)
    {
        currentState = newState;

        // TODO: Improve system by using ScriptableObjects
        switch (currentState)
        {
            case CameraState.ON:
            {
                SetCameraOn();
                break;
            }
            case CameraState.OFF:
            {
                SetCameraOff();
                break;
            }
            case CameraState.BLURRY:
            {
                SetCameraBlurry();
                break;
            }
        }
    }

    #endregion

}
