
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRHandedGrabInteractable : XRGrabInteractable
{
    [SerializeField]
    private Transform LeftHandAttachTransform;
    [SerializeField]
    private Transform RightHandAttachTransform;
    [SerializeField]
    XRDirectInteractor LeftController;
    [SerializeField]
    XRDirectInteractor RightController;

    private Transform m_OriginalAttachTransform;
    [SerializeField]
    private Transform m_attachTransform;

    void Awake()
    {
        m_OriginalAttachTransform = m_attachTransform;
    }

    //  OnSelectEntering - set attachTransform - then call base
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject == LeftController)
        {
            Debug.Log($"Left hand");
            attachTransform.SetPositionAndRotation(LeftHandAttachTransform.position, LeftHandAttachTransform.rotation);
        }
        else if (args.interactorObject == RightController)
        {
            Debug.Log($"Right hand");
            attachTransform.SetPositionAndRotation(RightHandAttachTransform.position, RightHandAttachTransform.rotation);
        }
        base.OnSelectEntering(args);
    }
}