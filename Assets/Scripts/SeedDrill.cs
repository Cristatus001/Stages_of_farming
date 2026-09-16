using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SeedDrill : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float tractorHeight = 1.28f;
    public float ploughingInterval = 0.1f;
    public GameObject[] ploughedTilePrefabs; // Array of multiple ploughed tile prefabs
    public float ploughpos; // Offset for ploughed tile position

    public Transform Ploughing_Position;

    // UI elements
    public TextMeshProUGUI percentageText; // Assign this in the Inspector
    public GameObject imageToActivate; // Assign the image to activate at 100% ploughing

    // Audio elements
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip idleClip; // Clip to play in idle mode
    public AudioClip moveClip; // Clip to play when moving

    private Vector3 direction = Vector3.left;
    private float leftBoundary = -12.5f;
    private float rightBoundary = 12.5f;
    private float topBoundary = 3.5f;
    private float bottomBoundary = -5.05f;

    private float lastPloughTime;
    private Vector3 lastPloughPosition;

    private bool isMoving = false;
    private bool isPloughingDone = false;
    private float totalPloughArea = 100f;
    private float ploughedArea = 0f;

    public Transform targetPosition;
    private bool shouldMove = false;
    public GameObject ploughingDonePanel;

    public Animator Anim;

    // List to store instantiated prefabs
    private List<GameObject> instantiatedPrefabs = new List<GameObject>();

    void Start()
    {
        lastPloughPosition = transform.position;
        lastPloughTime = Time.time;

        UpdatePercentage();

        // Initially deactivate the image
        if (imageToActivate != null)
        {
            imageToActivate.SetActive(false);
        }

        // Play idle clip initially
        if (audioSource != null && idleClip != null)
        {
            audioSource.clip = idleClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (shouldMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
            {
                transform.position = targetPosition.position;
                shouldMove = false;
            }
        }

        if (isMoving && !isPloughingDone)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;

            if (Time.time - lastPloughTime >= ploughingInterval)
            {
                InstantiateRandomPloughedTile(Ploughing_Position.position);
                lastPloughPosition = transform.position;
                lastPloughTime = Time.time;
            }

            float tolerance = 0.1f;
            if (Mathf.Abs(transform.position.x - leftBoundary) <= tolerance)
            {
                MoveAndFlip(Vector3.right);
            }
            else if (Mathf.Abs(transform.position.x - rightBoundary) <= tolerance)
            {
                MoveAndFlip(Vector3.left);
            }

            if (transform.position.y <= bottomBoundary)
            {
                MoveAndFlip(Vector3.up);
            }
            else if (transform.position.y >= topBoundary)
            {
                moveSpeed = 0f;
                isMoving = false;
                isPloughingDone = true;
                UpdatePercentage();
            }
        }

        if (imageToActivate != null && imageToActivate.activeSelf)
        {
            DestroyAllInstantiatedPrefabs();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TopLeftCorner"))
        {
            Debug.Log("end of farming");
            shouldMove = true;
            ploughedArea = totalPloughArea;

            isPloughingDone = true;
            UpdatePercentage();

            Invoke("EnablingPanels", 3f);
        }
    }

    public void EnablingPanels()
    {
        ploughingDonePanel.SetActive(true);

        if (audioSource != null)
        {
            audioSource.enabled = false;
        }
    }

    public void StartMoving()
    {
        isMoving = true;
        moveSpeed = 5f;
        Anim.SetBool("Run", true);

        if (audioSource != null && moveClip != null)
        {
            audioSource.clip = moveClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopMoving()
    {
        isMoving = false;
        moveSpeed = 0f;
        Anim.SetBool("Run", false);
        Anim.SetBool("idle", true);

        if (audioSource != null && idleClip != null)
        {
            audioSource.clip = idleClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void MoveAndFlip(Vector3 newDirection)
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, transform.position.z);
        transform.position += new Vector3(0f, tractorHeight, 0f);
        direction = newDirection;
        FlipSprite();
    }

    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void InstantiateRandomPloughedTile(Vector3 position)
    {
        if (ploughedTilePrefabs != null && ploughedTilePrefabs.Length > 0)
        {
            Vector3 ploughPosition = position - direction * ploughpos;
            Quaternion rotation = Quaternion.Euler(0, 0, 0);

            // Select a random prefab from the array
            GameObject randomPrefab = ploughedTilePrefabs[Random.Range(0, ploughedTilePrefabs.Length)];

            GameObject newTile = Instantiate(randomPrefab, ploughPosition, rotation);

            instantiatedPrefabs.Add(newTile);

            ploughedArea += 1f;
            UpdatePercentage();
        }
        else
        {
            Debug.LogWarning("PloughedTilePrefabs array is empty or not assigned.");
        }
    }

    private void UpdatePercentage()
    {
        if (percentageText != null)
        {
            float percentage = Mathf.Clamp((ploughedArea / totalPloughArea) * 100f, 0f, 100f);
            percentageText.text = $"{percentage:F0}%";

            if (percentage >= 100f && imageToActivate != null)
            {
                imageToActivate.SetActive(true);
            }
        }
    }

    private void DestroyAllInstantiatedPrefabs()
    {
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            Destroy(prefab);
        }
        instantiatedPrefabs.Clear();
    }
}
