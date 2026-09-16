using UnityEngine;
using UnityEngine.UI;

public class ThreshingMachine : MonoBehaviour
{
    public GameObject[] images; // Assign the images to this array in the Inspector
    private int objectsDropped = 0;
    private int totalObjects;

    private void Start()
    {
        totalObjects = FindObjectsOfType<DragDrop>().Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Draggable"))
        {
            Destroy(collision.gameObject);
            objectsDropped++;

            // Enable the corresponding image
            if (objectsDropped <= images.Length)
            {
                images[objectsDropped - 1].SetActive(true);
            }

            // Check if all objects are dropped
            if (objectsDropped == totalObjects)
            {
                ShowCompleteImage();
            }
        }
    }

    private void ShowCompleteImage()
    {
        // Show the full image or any other final action
        foreach (GameObject image in images)
        {
            image.SetActive(true); // This could be a full image or a panel
        }
    }
}
