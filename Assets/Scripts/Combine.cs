using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Combine : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float tractorHeight = 1.28f; // Height of the tractor for vertical movement
    public float ploughingInterval = 0.1f; // Time interval between each ploughing
    public GameObject ploughedTilePrefab; // Assign this in the Inspector
    public float ploughpos; // Offset for ploughed tile position

    // UI elements
    public TextMeshProUGUI percentageText; // Assign this in the Inspector
    public GameObject imageToActivate; // Assign the image to activate at 100%

    // Audio elements
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip idleClip; // Clip to play in idle mode
    public AudioClip moveClip; // Clip to play when moving

    private Vector3 direction = Vector3.left; // Initial direction
    private float leftBoundary = -12.5f; // Adjusted left screen edge position
    private float rightBoundary = 12.5f; // Adjusted right screen edge position
    private float topBoundary = 3.5f; // Adjusted top screen edge position
    private float bottomBoundary = -5.05f; // Adjusted bottom screen edge position

    private float lastPloughTime;
    private Vector3 lastPloughPosition;

    private bool isMoving = false;
    private bool isPloughingDone = false;
    private float totalPloughArea = 100f; // Example total area, adjust as needed
    private float ploughedArea = 0f;

    public Transform targetPosition; // Assign the target position in the Inspector
    private bool shouldMove = false;
    public GameObject ploughingdonepannel;

    public Vector3 position;

    public Animator Anim;

    // List to store instantiated prefabs
    private List<GameObject> instantiatedPrefabs = new List<GameObject>();

    // Variables to manage movement and counting
    private float distanceTraveled = 0f;

    // Flag to control counting
    public bool countingActive = true; // Control when the percentage should be counted
    private bool hasReachedMaxPercentage = false; // Flag to indicate if 100% is reached

    void Start()
    {
        lastPloughPosition = transform.position; // Initialize with the starting position
        lastPloughTime = Time.time; // Initialize the last plough time

        //UpdatePercentage();

        // Initially deactivate the image
        if (imageToActivate != null)
        {
            imageToActivate.SetActive(false);
        }

        // Play idle clip initially
        if (audioSource != null && idleClip != null)
        {
            audioSource.clip = idleClip;
            audioSource.loop = true; // Idle sound should loop
            audioSource.Play();
        }
    }

    void Update()
    {
        if (shouldMove)
        {
            // Smoothly move the object to the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // Check if the object has reached the target position
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
            {
                transform.position = targetPosition.position; // Snap to target position
                shouldMove = false; // Stop movement
            }
        }

        if (isMoving && !isPloughingDone)
        {
            // Move the tractor in the current direction
            transform.position += direction * moveSpeed * Time.deltaTime;
            distanceTraveled += moveSpeed * Time.deltaTime; // Update distance traveled

            // Continuously instantiate ploughed tile behind the tractor
            if (Time.time - lastPloughTime >= ploughingInterval)
            {
                // Instantiate the ploughed tile at the position behind the tractor
                InstantiatePloughedTile(lastPloughPosition);

                // Update the last plough position to the current position
                lastPloughPosition = transform.position;
                lastPloughTime = Time.time; // Update the last plough time
            }

            // Calculate the percentage based on distance traveled only if counting is active and hasn't reached 100%
            if (countingActive && !hasReachedMaxPercentage)
            {
                ploughedArea = Mathf.Clamp((distanceTraveled / totalPloughArea) * 100f, 0f, totalPloughArea);
                UpdatePercentage();
            }

            // Ensure the tractor stays within the boundaries
            if (transform.position.x <= leftBoundary)
            {
                MoveAndFlip(Vector3.right);
            }
            else if (transform.position.x >= rightBoundary)
            {
                MoveAndFlip(Vector3.left);
            }

            if (transform.position.y <= bottomBoundary)
            {
                MoveAndFlip(Vector3.up); // Move up if it reaches the bottom boundary
            }
            else if (transform.position.y >= topBoundary)
            {
                moveSpeed = 0f; // Stop the tractor
                isMoving = false; // Stop movement
                isPloughingDone = true; // Mark ploughing as done
                UpdatePercentage(); // Update the percentage display
            }
        }

        // Check if the image is activated and destroy all prefabs
        if (imageToActivate != null && imageToActivate.activeSelf)
        {
            DestroyAllInstantiatedPrefabs();
            ploughedArea = totalPloughArea; // Set ploughed area to total area
            UpdatePercentage(); // Ensure the UI reflects the final percentage
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TopLeftCorner"))
        {
            Debug.Log("End of farming");
            shouldMove = true; // Start moving to the target position
            isPloughingDone = true; // Mark ploughing as done
            countingActive = false; // Stop counting the percentage
            hasReachedMaxPercentage = true;

            // Ensure percentage is set to 100%
            ploughedArea = totalPloughArea;
            percentageText.text = "100%"; // Instantly update UI text
            if (imageToActivate != null)
            {
                imageToActivate.SetActive(true); // Activate image when done
            }

            Invoke("EnablingPanels", 3f);
        }
    }


    public void EnablingPanels()
    {
        ploughingdonepannel.SetActive(true);

        if (audioSource != null)
        {
            audioSource.enabled = false;
        }
    }

    public void StartMoving()
    {
        isMoving = true;
        moveSpeed = 5f; // Set the speed to the desired value when moving
        Anim.SetBool("Run", true);

        // Play movement clip when moving
        if (audioSource != null && moveClip != null)
        {
            audioSource.clip = moveClip;
            audioSource.loop = true; // Loop the movement sound
            audioSource.Play();
        }
    }

    public void StopMoving()
    {
        isMoving = false;
        moveSpeed = 0f; // Stop the tractor
        Anim.SetBool("Run", false);
        Anim.SetBool("idle", true);

        // Stop movement clip and switch back to idle clip
        if (audioSource != null && idleClip != null)
        {
            audioSource.clip = idleClip;
            audioSource.loop = true;
            audioSource.Play(); // Play idle sound again
        }
    }

    private void MoveAndFlip(Vector3 newDirection)
    {
        // Move the tractor up by its height
        transform.position += new Vector3(0f, tractorHeight, 0f);

        // Update the direction
        direction = newDirection;

        // Flip the sprite horizontally
        FlipSprite();
    }

    private void InstantiatePloughedTile(Vector3 position)
    {
        if (ploughedTilePrefab != null)
        {
            // Instantiate the ploughed tile with the specified position and rotation
            Vector3 ploughPosition = position - direction * ploughpos;
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            GameObject newTile = Instantiate(ploughedTilePrefab, ploughPosition, rotation);

            // Add the instantiated tile to the list
            instantiatedPrefabs.Add(newTile);
        }
        else
        {
            Debug.LogWarning("PloughedTilePrefab is not assigned.");
        }
    }

    private void UpdatePercentage()
    {
        if (percentageText != null)
        {
            if (countingActive)
            {
                float percentage = Mathf.Clamp((ploughedArea / totalPloughArea) * 100f, 0f, 100f);
                percentageText.text = $"{percentage:F0}%";

                // Check if the maximum percentage is reached
                if (percentage >= 100f)
                {
                    hasReachedMaxPercentage = true; // Set the flag to prevent further counting
                    countingActive = false; // Stop any further counting
                    if (imageToActivate != null)
                    {
                        imageToActivate.SetActive(true); // Activate the image when 100% is reached
                    }
                }
            }
        }
    }

    // Method to destroy all instantiated prefabs
    private void DestroyAllInstantiatedPrefabs()
    {
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            Destroy(prefab);
        }
        instantiatedPrefabs.Clear(); // Clear the list after destroying the prefabs
    }

    // Method to flip the sprite based on direction
    private void FlipSprite()
    {
        // Assuming the tractor has a SpriteRenderer component
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Flip the parent's sprite based on direction
            spriteRenderer.flipX = direction == Vector3.right;
        }

        // Flip the child object's scale along the X-axis
        Transform childTransform = transform.GetChild(0); // Assuming the child is the first child
        if (childTransform != null)
        {
            // Invert the local scale of the child along the X-axis to flip it
            Vector3 childScale = childTransform.localScale;
            childScale.x *= -1;
            childTransform.localScale = childScale;
        }
    }
}
    