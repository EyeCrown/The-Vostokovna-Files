using TMPro;
using UnityEngine;

public class ModePart : MonoBehaviour
{
    #region Attributes

    [SerializeField] private IntData _currentMode;
    [SerializeField] private GameModeDatas _gameModes;

    [SerializeField] private TextMeshProUGUI _modeText;
    
    #endregion
    
    
    #region Unity API

    void Start()
    {
        SetModeText();
    }

    #endregion

    #region Methods

    void SetModeText()
    {
        _modeText.text = _gameModes.DataModes[_currentMode.Value].modeName.ToUpper();
    }
    
    #endregion

    #region EventHandlers

    private void OnEnable()
    {
        _currentMode.OnChange.AddListener(SetModeText);
    }

    private void OnDisable()
    {
        _currentMode.OnChange.RemoveListener(SetModeText);
    }

    #endregion
}
