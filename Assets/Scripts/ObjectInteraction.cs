using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectInteraction : MonoBehaviour
{
    public ParticleSystem particlePrefab; // Particle system prefab to instantiate
    public TextMeshProUGUI percentageText; // UI Text field
    public GameObject[] objectsToDestroy; // Array to hold references to objects
    public GameObject panel; // The panel to enable after completion
    public GameObject rotatingImagePrefab; // Prefab of the image that will rotate

    private int totalObjects;
    private int destroyedObjects = 0; // Counter for destroyed objects

    private void Start()
    {
        percentageText.text = "0%"; // Initialize text to 0%
        totalObjects = objectsToDestroy.Length; // Set total objects count based on array length
        panel.SetActive(false); // Hide the panel initially
    }

    private void Update()
    {
        // Check for tap or click input
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Vector2 touchPos;

        // Determine whether the input is from touch or mouse
        if (Input.touchCount > 0)
        {
            touchPos = Input.GetTouch(0).position; // Get touch position
        }
        else
        {
            touchPos = Input.mousePosition; // Get mouse position
        }

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touchPos), Vector2.zero);
        if (hit.collider != null)
        {
            GameObject clickedObject = hit.collider.gameObject;
            if (IsObjectToDestroy(clickedObject))
            {
                ObjectClickHandler(clickedObject, Camera.main.ScreenToWorldPoint(touchPos));
            }
        }
    }

    private bool IsObjectToDestroy(GameObject obj)
    {
        foreach (GameObject go in objectsToDestroy)
        {
            if (go == obj) return true;
        }
        return false;
    }

    private void ObjectClickHandler(GameObject obj, Vector3 clickPosition)
    {
        ClickCounter clickCounter = obj.GetComponent<ClickCounter>() ?? obj.AddComponent<ClickCounter>();
        if (clickCounter.IncrementClickCount() >= 1)
        {
            DestroyObject(obj, clickPosition); // Immediately destroy the object
        }
    }

    private void DestroyObject(GameObject obj, Vector3 clickPosition)
    {
        if (particlePrefab != null)
        {
            // Instantiate particle system and start coroutine to destroy it after it finishes
            ParticleSystem particleInstance = Instantiate(particlePrefab, obj.transform.position, Quaternion.identity);
            StartCoroutine(DestroyParticlesWhenComplete(particleInstance));
        }

        // Instantiate the rotating image at the click position
        GameObject rotatingImage = Instantiate(rotatingImagePrefab, clickPosition, Quaternion.identity);
        rotatingImage.transform.position = new Vector3(clickPosition.x, clickPosition.y, 0); // Ensure it's positioned correctly
        StartCoroutine(RotateImage(rotatingImage)); // Start rotating the image

        Destroy(obj);
        destroyedObjects++;
        percentageText.text = (destroyedObjects * 10).ToString() + "%";

        if (destroyedObjects >= totalObjects)
        {
            Invoke("ShowPanel",2f); // Show the panel immediately without delay
        }
    }

    private IEnumerator DestroyParticlesWhenComplete(ParticleSystem particleInstance)
    {
        // Set stop action to destroy the particle system on completion
        var main = particleInstance.main;
        main.stopAction = ParticleSystemStopAction.Destroy;

        // Wait until particles stop playing
        while (particleInstance.isPlaying)
        {
            yield return null;
        }
    }

    private void ShowPanel()
    {
        panel.SetActive(true);
    }

    private IEnumerator RotateImage(GameObject image)
    {
        float rotationDuration = 1f; // Duration of the rotation
        float elapsedTime = 0f;
        float startRotation = image.transform.rotation.eulerAngles.z;

        while (elapsedTime < rotationDuration)
        {
            float angle = Mathf.Lerp(startRotation, startRotation + 360f, elapsedTime / rotationDuration);
            image.transform.rotation = Quaternion.Euler(0, 0, angle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(image); // Destroy the image after rotation completes
    }
}
