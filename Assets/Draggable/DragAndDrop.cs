using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{
    public Transform dropPosition; // Reference to the intended drop position
    public int totalObjects = 4; // Total number of draggable objects
    public GameObject[] waterAnimations; // Array to hold the water animations (should be 3 in this case)
    public TextMeshProUGUI percentageText;  // Reference to the UI Text component for displaying percentage
    public Camera mainCamera; // Reference to the main camera
    public Sprite newBackgroundImage; // Reference to the new background image for the main camera
    public GameObject panel; // Reference to the panel to enable after delay
    public float panelDelay = 2f; // Delay before the panel is enabled
    public float blinkSpeed = 1f; // Speed of the blink effect

    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 originalPosition;
    private Transform originalParent;
    private bool isPlacedCorrectly = false; // Flag to track if the object is placed correctly
    private Coroutine blinkCoroutine; // Reference to the blinking coroutine

    public static int correctlyPlacedCount = 0; // Static counter for correctly placed objects
    public int percentage = 0;

    private void Start()
    {
        correctlyPlacedCount = 0;
        percentage = 0;
        UpdatePercentageText();

        originalPosition = transform.position;
        originalParent = transform.parent; // Store the original parent
        Debug.Log($"Pipe {name} started at position {originalPosition}");

        percentageText.text = percentage.ToString() + "%";
    }

    private void OnMouseDown()
    {
        if (isPlacedCorrectly)
            return; // Do nothing if the object is already placed correctly

        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;

        // Start the blinking effect on the drop position
        if (blinkCoroutine == null && dropPosition != null)
        {
            blinkCoroutine = StartCoroutine(BlinkDropPosition());
        }

        Debug.Log($"Dragging {name}");
    }

    private void OnMouseUp()
    {
        if (isPlacedCorrectly)
            return; // Do nothing if the object is already placed correctly

        isDragging = false;
        CheckDropPosition();
    }

    private void Update()
    {
        if (isDragging && !isPlacedCorrectly)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePos.x, mousePos.y, 0);
    }

    private void CheckDropPosition()
    {
        if (dropPosition != null)
        {
            if (Vector3.Distance(transform.position, dropPosition.position) < 0.5f)
            {
                transform.position = dropPosition.position;
                isPlacedCorrectly = true;
                correctlyPlacedCount++;
                UpdatePercentageText();

                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                    blinkCoroutine = null;
                    dropPosition.GetComponent<SpriteRenderer>().enabled = true; // Ensure the drop position is visible
                }

                Debug.Log($"{name} placed correctly. Total placed: {correctlyPlacedCount}");

                if (correctlyPlacedCount == totalObjects)
                {
                    Debug.Log("All pipes placed correctly! Enabling water animations.");
                    EnableWaterAnimations();
                    StartCoroutine(ChangeBackgroundAndEnablePanel());
                }
            }
            else
            {
                ResetPosition();
            }
        }
        else
        {
            Debug.Log("No drop position assigned. Returning to original position.");
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        transform.position = originalPosition;
        transform.SetParent(originalParent); // Restore the original parent

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
            dropPosition.GetComponent<SpriteRenderer>().enabled = true; // Ensure the drop position is visible
        }

        Debug.Log($"{name} reset to original position.");
    }

    private void EnableWaterAnimations()
    {
        foreach (GameObject waterAnimation in waterAnimations)
        {
            if (waterAnimation != null)
            {
                waterAnimation.SetActive(true);
                Debug.Log($"Water animation enabled for {name}");
            }
            else
            {
                Debug.LogWarning($"Water animation for {name} is not assigned.");
            }
        }
    }

    private void UpdatePercentageText()
    {
        if (percentageText != null)
        {
            percentage = (correctlyPlacedCount * 100) / totalObjects;
            percentageText.text = percentage.ToString() + "%";
        }
        else
        {
            Debug.LogWarning("Percentage Text UI is not assigned.");
        }
    }

    private IEnumerator ChangeBackgroundAndEnablePanel()
    {
        if (mainCamera != null && newBackgroundImage != null)
        {
            mainCamera.GetComponent<SpriteRenderer>().sprite = newBackgroundImage;
            Debug.Log("Main camera background image changed.");

            yield return new WaitForSeconds(panelDelay);

            if (panel != null)
            {
                panel.SetActive(true);
                Debug.Log("Panel enabled after delay.");
            }
        }
        else
        {
            Debug.LogWarning("Main camera or new background image is not assigned.");
        }
    }

    private IEnumerator BlinkDropPosition()
    {
        SpriteRenderer dropSpriteRenderer = dropPosition.GetComponent<SpriteRenderer>();

        if (dropSpriteRenderer != null)
        {
            while (true)
            {
                dropSpriteRenderer.enabled = !dropSpriteRenderer.enabled;
                yield return new WaitForSeconds(blinkSpeed);
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ResetOnNewScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetOnNewScene;
    }

    private void ResetOnNewScene(Scene scene, LoadSceneMode mode)
    {
        correctlyPlacedCount = 0;
    }
}
