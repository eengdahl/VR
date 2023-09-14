using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomGrabInteractable : XRGrabInteractable
{
    public void OnSelectExit(SelectExitEventArgs args)
    {
        // Add your custom logic here to control when and how the object is dropped.
        // For example, you might check if a specific button is pressed before allowing the drop.
        if (SomeConditionMet())
        {
            base.OnSelectExited(args); // Call the base method to handle standard dropping.
        }
    }

    private bool SomeConditionMet()
    {
        // Implement your custom logic here to determine if the object should be dropped.
        return true; // Change this condition based on your requirements.
    }
}
