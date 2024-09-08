using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Attributes

    [SerializeField] private List<CameraData> cameraDataList;

    [SerializeField] private List<GameObject> cameraObjects;
    
    [SerializeField] private Slider timeSlider;

    #endregion

    #region Unity API

    private void Awake()
    {
        timeSlider.onValueChanged.AddListener(delegate { TimeChange(); });
    }

    #endregion


    #region Methods

    void TimeChange()
    {
        int value = Mathf.RoundToInt(timeSlider.value);

        for (int i = 0; i < cameraObjects.Count; i++)
        {
            CameraState state = cameraDataList[i].states[value];
            
            cameraObjects[i].GetComponent<CameraModifier>().UpdateState.Invoke(state);
        }
        
    }

    #endregion
    
    

}
