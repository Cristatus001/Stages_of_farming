using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TapEffect : MonoBehaviour
{
    public GameObject particleEffect; // Particle effect prefab
    public GameObject panel; // The panel to show after all taps
    public TextMeshProUGUI percentageText; // Text component to display percentage
    public Texture2D normalCursorTexture; // Texture for the normal cursor
    public GameObject[] boxes; // Array to store the four boxes
    public Color tappedColor; // Color to change after tapping 4-6 times
    public float delayBeforeShowingPanel = 2f; // Delay time before showing the panel
    public Transform character; // Reference to the character transform
    public float characterSpeed = 5f; // Speed of the character movement

    private Camera mainCamera;
    private int tapCount = 0;
    private int totalTapsNeededPerBox = 1;
    private int completedBoxes = 0;
    private int totalBoxes = 4; // Number of boxes to tap
    private Dictionary<GameObject, int> boxTaps = new Dictionary<GameObject, int>();
    private Vector3 targetPosition; // Position where the character will move
    private bool isMoving = false; // Check if character is currently moving

    public Animator anim;

    void Start()
    {
        mainCamera = Camera.main;
        panel.SetActive(false); // Hide panel initially
        UpdatePercentageText(); // Initialize percentage text

        // Set the cursor to normal at the start
        Cursor.SetCursor(normalCursorTexture, Vector2.zero, CursorMode.Auto);

        // Initialize tap counts for each box
        foreach (GameObject box in boxes)
        {
            boxTaps[box] = 0;
        }

        // Initialize target position to the character's starting position
        targetPosition = character.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;

            // Set target position for the character
            targetPosition = worldPosition;
            isMoving = true; // Set moving to true to prevent multiple clicks

            // Check which box was clicked
            foreach (GameObject box in boxes)
            {
                if (IsTapInsideBox(box, worldPosition))
                {
                    StartCoroutine(MoveCharacterAndHandleTap(box, worldPosition));
                    break;
                }
            }
        }

        // Move the character towards the target position
        if (isMoving)
        {
            MoveCharacter();
        }
    }

    void MoveCharacter()
    {
        if (Vector3.Distance(character.position, targetPosition) > 0.1f)
        {

            // Move the character towards the target position
            character.position = Vector3.MoveTowards(character.position, targetPosition, characterSpeed * Time.deltaTime);

            // Check the direction of movement to flip the sprite
            if (targetPosition.x > character.position.x)
            {
                // Moving to the right, so don't flip the sprite
                character.transform.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (targetPosition.x < character.position.x)
            {
                // Moving to the left, so flip the sprite
                character.transform.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else
        {
            // Once the character reaches the target position, stop moving
            isMoving = false;
        }
    }

    bool IsTapInsideBox(GameObject box, Vector3 tapPosition)
    {
        Vector3 boxPos = box.transform.position;
        Vector3 boxScale = box.transform.localScale;

        return tapPosition.x > boxPos.x - boxScale.x / 2 &&
               tapPosition.x < boxPos.x + boxScale.x / 2 &&
               tapPosition.y > boxPos.y - boxScale.y / 2 &&
               tapPosition.y < boxPos.y + boxScale.y / 2;
    }

    IEnumerator MoveCharacterAndHandleTap(GameObject box, Vector3 tapPosition)
    {
        anim.SetBool("IsWalking", true);
       
        // Wait until the character reaches the target position
        while (isMoving)
        {
            yield return null;
        }

        anim.SetBool("IsWalking", false);
        anim.SetTrigger("ReachedTarget");

        // Check if the box has already been fully tapped
        if (boxTaps[box] >= totalTapsNeededPerBox)
        {
            Debug.Log("This box has already been fully tapped.");
            yield break;
        }

        // Instantiate particle effect
        Instantiate(particleEffect, tapPosition, Quaternion.identity);

        // Increase tap count for this box
        boxTaps[box]++;

        // Check if the box is fully tapped
        if (boxTaps[box] >= totalTapsNeededPerBox)
        {
            // Change box color
            box.GetComponent<SpriteRenderer>().color = tappedColor;

            completedBoxes++;
            UpdatePercentageText(); // Update the percentage text

            // Check if all boxes are completed
            if (completedBoxes >= totalBoxes)
            {
                StartCoroutine(ShowPanelAfterDelay()); // Show the panel after a delay
                Invoke("DisableAllParticles", 2f); // Disable all particle systems
                Cursor.SetCursor(normalCursorTexture, Vector2.zero, CursorMode.Auto); // Reset cursor
            }
        }
    }

    void DisableAllParticles()
    {
        // Find all particle systems in the scene
        ParticleSystem[] allParticles = FindObjectsOfType<ParticleSystem>();

        // Disable each particle system
        foreach (ParticleSystem ps in allParticles)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // Stop and clear the particle system
            ps.gameObject.SetActive(false); // Disable the particle system's GameObject
        }
    }

    void UpdatePercentageText()
    {
        if (percentageText != null)
        {
            int percentage = (completedBoxes * 100) / totalBoxes;
            percentageText.text = percentage.ToString() + "%";
        }
        else
        {
            Debug.LogWarning("Percentage Text UI is not assigned.");
        }
    }

    IEnumerator ShowPanelAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeShowingPanel); // Wait for the specified delay
        panel.SetActive(true); // Show the panel
    }
}
