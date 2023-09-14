using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

public class ShootableButton : MonoBehaviour
{
    public enum UItype { buttonType, toggleType }

    [SerializeField] private UItype uiType;
    
    public void TriggerButton()
    {
        switch (uiType)
        {
            case UItype.buttonType:
                GetComponent<Button>().onClick.Invoke();
                break;
            case UItype.toggleType:
                GetComponent<Toggle>().isOn = !GetComponent<Toggle>().isOn;
                break;
        }
    }
}