using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogRegistry : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI TimeField;
    [SerializeField] public TextMeshProUGUI InfoField;
    [SerializeField] public TextMeshProUGUI CamField;


    [SerializeField] private Image _mask;
    [SerializeField] private Image _notification;

    public void RevealInfoText()
    {
        _mask.gameObject.SetActive(false);
    }

    public void EnableNotification(bool enabled)
    {
        _notification.gameObject.SetActive(enabled);
    }
}
