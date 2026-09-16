using UnityEngine;
using TMPro;
using System.Collections;

public class DragDrop : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;
    private bool isDragging = false;
    private static bool isAnyObjectBeingDragged = false; // Static variable to track dragging state

    public GameObject threshingMachine; // Assign the threshing machine in the Inspector
    public GameObject threshingDonePanel;
    public TextMeshProUGUI percentageText;
    public ParticleSystem particleEffectPrefab; // Assign the particle system prefab in the Inspector
    public Transform particleEffectPosition; // This should be the specific position for the particle effect (assign in the Inspector)
    public int totalObjects = 5;
    private static int destroyedObjects = 0;
    private float percentageIncrease = 12f;
    public AudioClip[] audioClips;

    public GameObject[] targetImages; // Array to hold target images

    void Start()
    {
        ResetStaticVariables();

        startPosition = transform.position;
        UpdatePercentageText();

        if (threshingDonePanel != null)
        {
            threshingDonePanel.SetActive(false);
        }

        // Ensure target images are disabled initially
        if (targetImages != null)
        {
            foreach (GameObject image in targetImages)
            {
                image.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Handle touch and mouse inputs
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0f;

            if (touch.phase == TouchPhase.Began)
            {
                OnMouseDown();
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                OnMouseDrag();
            }
            else if (touch.phase == TouchPhase.Ended && isDragging)
            {
                OnMouseUp();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            OnMouseDrag();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            OnMouseUp();
        }
    }

    void OnMouseDown()
    {
        if (isAnyObjectBeingDragged) return; // Prevent dragging if another object is being dragged

        Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        inputPosition.z = 0f;

        Vector3 objectPosition = transform.position;
        Vector2 distance = objectPosition - inputPosition;

        if (distance.magnitude < 1.0f)
        {
            isDragging = true;
            isAnyObjectBeingDragged = true; // Set the static variable to indicate dragging is in progress
            offset = objectPosition - inputPosition;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputPosition.z = 0f;
            transform.position = inputPosition + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        isAnyObjectBeingDragged = false; // Reset the static variable when dragging stops

        if (IsOverThreshingMachine())
        {
            PlayParticleEffect(); // Play particle effect at predefined position
            Destroy(gameObject);

            // Notify the ThreshingManager about the destroyed object
            ThreshingManager threshingManager = FindObjectOfType<ThreshingManager>();
            if (threshingManager != null)
            {
                threshingManager.ObjectDestroyed();
            }

            UpdateDestroyedObjectsCount();
        }
        else
        {
            transform.position = startPosition;
        }
    }

    bool IsOverThreshingMachine()
    {
        Collider2D threshingMachineCollider = threshingMachine.GetComponent<Collider2D>();
        if (threshingMachineCollider != null)
        {
            return threshingMachineCollider.bounds.Contains(transform.position);
        }
        return false;
    }

    void PlayParticleEffect()
    {
        if (particleEffectPrefab != null && particleEffectPosition != null)
        {
            // Instantiate the particle effect at the predefined position
            Instantiate(particleEffectPrefab, particleEffectPosition.position, Quaternion.Euler(0f, 90, -90f));
        }
    }

    void UpdateDestroyedObjectsCount()
    {
        destroyedObjects++;
        Debug.Log("Destroyed Objects: " + destroyedObjects); // Debug message to track count
        UpdatePercentageText();

        if (destroyedObjects >= totalObjects)
        {
            StartCoroutine(EnableThreshingDonePanelWithDelay(3f)); // Enable panel after 3 seconds
            StopAllAudioClips();
        }
    }

    void UpdatePercentageText()
    {
        float percentage = destroyedObjects * percentageIncrease;
        if (percentage > 100f)
        {
            percentage = 100f; // Ensure the percentage doesn't exceed 100%
        }

        if (percentageText != null)
        {
            percentageText.text = percentage.ToString("F0") + "%";
        }

        EnableImagesBasedOnPercentage(percentage);
    }

    void EnableImagesBasedOnPercentage(float percentage)
    {
        int indexToEnable = -1; // Start with -1 meaning no image is enabled

        if (percentage >= 100)
        {
            indexToEnable = 2; // Enable the third image at 96%
        }
        else if (percentage >= 72)
        {
            indexToEnable = 1; // Enable the second image at 72%
        }
        else if (percentage >= 36)
        {
            indexToEnable = 0; // Enable the first image at 36%
        }

        for (int i = 0; i < targetImages.Length; i++)
        {
            if (i == indexToEnable)
            {
                targetImages[i].SetActive(true); // Enable the current image at the required percentage
            }
            else
            {
                targetImages[i].SetActive(false); // Disable other images
            }
        }
    }

    private IEnumerator EnableThreshingDonePanelWithDelay(float delay)
    {
        Debug.Log("Waiting to enable threshingDonePanel for " + delay + " seconds");
        yield return new WaitForSeconds(delay);

        if (threshingDonePanel != null)
        {
            Debug.Log("Attempting to enable threshingDonePanel"); // Debug log
            threshingDonePanel.SetActive(true);
            Debug.Log("threshingDonePanel enabled");
        }
        else
        {
            Debug.LogError("threshingDonePanel is null!"); // Error log
        }
    }

    void StopAllAudioClips()
    {
        foreach (AudioClip clip in audioClips)
        {
            // Add logic to stop audio clips as needed
        }
    }

    void ResetStaticVariables()
    {
        destroyedObjects = 0;
        isAnyObjectBeingDragged = false; // Reset the dragging state on start
    }
}
