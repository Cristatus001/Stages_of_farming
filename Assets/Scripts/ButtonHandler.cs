using UnityEngine;
using UnityEngine.EventSystems; // Required for EventTrigger
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public TractorMovement tractorMovement; // Assign this in the Inspector

    // This method is called when the button is pressed
    public void OnPointerDown(BaseEventData eventData)
    {
        if (tractorMovement != null)
        {
            tractorMovement.StartMoving();
        }
    }

    // This method is called when the button is released
    public void OnPointerUp(BaseEventData eventData)
    {
        if (tractorMovement != null)
        {
            tractorMovement.StopMoving();
        }
    }
}
