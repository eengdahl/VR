using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticScript : MonoBehaviour
{
    [Range(0, 1)]
    float intensity;
    float duration;

    XRBaseInteractable interactable;


    // Start is called before the first frame update
    void Start()
    {
        interactable = FindAnyObjectByType<XRBaseInteractable>();
        interactable.activated.AddListener(TriggerHapticEvent);
    }

    public void TriggerHapticEvent(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactableObject is XRBaseControllerInteractor controllerInteractor)
        {
            TriggerHapticEvent(controllerInteractor.xrController);
        }
    }

    public void TriggerHapticEvent(XRBaseController controller)
    {
        if (intensity > 0)
        {
            controller.SendHapticImpulse(intensity, duration);
        }
    }


}
