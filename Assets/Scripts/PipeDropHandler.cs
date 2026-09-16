using UnityEngine;
using UnityEngine.EventSystems;

public class PipeDropHandler : MonoBehaviour, IDropHandler
{
    public IrrigationSystemManager irrigationSystemManager; // Reference to the IrrigationSystemManager script
    public bool isOccupied = false; // Marked as public to allow access from other scripts

    public void OnDrop(PointerEventData eventData)
    {
        if (isOccupied) return; // If this spot is occupied, do nothing

        GameObject dropped = eventData.pointerDrag;
        if (dropped != null)
        {
            dropped.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            isOccupied = true; // Mark this spot as occupied
            irrigationSystemManager.PipePlaced(); // Notify the IrrigationSystemManager that a pipe is placed
        }
    }
}
