using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ObjectInteraction : MonoBehaviour
{
    public Texture2D normalCursor; // The normal cursor texture
    public ParticleSystem particleSystem; // Your particle system
    public TextMeshProUGUI percentageText; // UI Text field
    public GameObject[] objectsToDestroy; // Array to hold references to objects
    public GameObject panel; // The panel to enable after completion
    public Image cursorImage; // The UI Image acting as the custom cursor
    public float rotationAngle = 45f; // Angle to rotate on click
    public float rotationDuration = 0.2f; // Duration for rotation animation

    private int totalObjects;
    private int destroyedObjects = 0; // Counter for destroyed objects
    private RectTransform cursorRectTransform;

    private void Start()
    {
        cursorRectTransform = cursorImage.GetComponent<RectTransform>();
        percentageText.text = "0%"; // Initialize text to 0%
        totalObjects = objectsToDestroy.Length; // Set total objects count based on array length
        panel.SetActive(false); // Hide the panel initially
        Cursor.visible = false; // Hide the default cursor
    }

    private void Update()
    {
        // Update the position of the UI cursor to follow the mouse
        Vector2 cursorPos = Input.mousePosition;
        cursorRectTransform.position = cursorPos;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (IsObjectToDestroy(clickedObject))
                {
                    ObjectClickHandler(clickedObject);
                    StartCoroutine(RotateCursor()); // Start rotating the cursor on click
                }
            }
        }
    }

    private bool IsObjectToDestroy(GameObject obj)
    {
        // Check if the clicked object is in the objectsToDestroy array
        foreach (GameObject go in objectsToDestroy)
        {
            if (go == obj)
            {
                return true;
            }
        }
        return false;
    }

    private void ObjectClickHandler(GameObject obj)
    {
        // Check if the object has already been clicked 1 time (update to 1 for demonstration)
        ClickCounter clickCounter = obj.GetComponent<ClickCounter>();
        if (clickCounter == null)
        {
            clickCounter = obj.AddComponent<ClickCounter>();
        }

        if (clickCounter.IncrementClickCount() >= 1)
        {
            DestroyObject(obj);
        }
    }

    private void DestroyObject(GameObject obj)
    {
        // Play particle effect at the object's position
        if (particleSystem != null)
        {
            Instantiate(particleSystem, obj.transform.position, Quaternion.identity);
        }

        // Destroy the object
        Destroy(obj);

        // Update the percentage text
        destroyedObjects++;
        percentageText.text = (destroyedObjects * 4).ToString() + "%";

        // Check if all objects are destroyed
        if (destroyedObjects >= totalObjects)
        {
            // Start coroutine to enable the panel and reset the cursor after a delay
            StartCoroutine(ShowPanelAndResetCursorAfterDelay(2.0f)); // 2 seconds delay
        }
    }

    private IEnumerator ShowPanelAndResetCursorAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Enable the panel
        panel.SetActive(true);

        // Reset the cursor to normal
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Reset to the default system cursor
        Cursor.visible = true; // Make sure the default cursor is visible
        cursorImage.gameObject.SetActive(false); // Hide the UI Image cursor
    }

    private IEnumerator RotateCursor()
    {
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            float angle = Mathf.Lerp(0, rotationAngle, elapsedTime / rotationDuration);
            cursorRectTransform.rotation = Quaternion.Euler(0, 0, angle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Optionally, reset the cursor rotation after the animation
        cursorRectTransform.rotation = Quaternion.identity;
    }
}
