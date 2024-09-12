using System;
using TMPro;
using UnityEngine;

public class CamPart : MonoBehaviour
{
    #region Attributes

    [SerializeField] private IntData _cameraData;

    [SerializeField] private TextMeshProUGUI _camText;

    #endregion

    #region Unity API

    void Start()
    {
        SetCameraText();
    }

    #endregion

    #region Methods

    void SetCameraText()
    {
        _camText.text = $"CAM{_cameraData.Value + 1}";
    }

    #endregion

    #region EventHandlers

    private void OnEnable()
    {
        _cameraData.OnChange.AddListener(SetCameraText);
    }

    private void OnDisable()
    {
        _cameraData.OnChange.RemoveListener(SetCameraText);
    }
    
    #endregion
    
}
