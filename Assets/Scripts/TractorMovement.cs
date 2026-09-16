using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using PathCreation;

public class TractorMovement : MonoBehaviour
{
    public PathCreator pathCreator;  // Reference to PathCreator
    public float moveSpeed = 5f;
    private float distanceTravelled;

    public GameObject ploughedTilePrefab;
    public GameObject ploughParticlePrefab;
    public Transform Ploughing_Position;

    public TextMeshProUGUI percentageText;
    public GameObject imageToActivate;
    public GameObject ploughingdonepannel;

    public AudioSource audioSource;
    public AudioClip idleClip;
    public AudioClip moveClip;

    public Animator Anim;

    private bool isMoving = false;
    private bool isPloughingDone = false;
    private float totalPathLength;
    private float ploughedArea = 0f;

    private List<GameObject> instantiatedPrefabs = new List<GameObject>();

    void Start()
    {
        if (pathCreator != null)
        {
            totalPathLength = pathCreator.path.length;
            distanceTravelled = 0f;
        }

        if (imageToActivate != null)
        {
            imageToActivate.SetActive(false);
        }

        if (audioSource != null && idleClip != null)
        {
            audioSource.clip = idleClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        distanceTravelled = 0f;
    }

    public float ploughingInterval = 0.1f;
    private Vector3 lastPloughPosition;
    private float lastPloughTime;
    void Update()
    {
        // Check for input (tap or click)
        /*if (!isMoving && !isPloughingDone && Input.GetMouseButtonDown(0))
        {
            StartMoving();
        }*/

        // Tractor movement along the path
        if (isMoving && pathCreator != null)
        {
            // Move along the path
            distanceTravelled += moveSpeed * Time.deltaTime;
            Vector3 newPosition = pathCreator.path.GetPointAtDistance(distanceTravelled);

            // Calculate direction for rotation
            Vector3 direction = newPosition - transform.position;
            if (direction != Vector3.zero)  // Prevent rotation when stationary
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 180); // Apply 2D rotation
            }

            transform.position = newPosition;

            if (Time.time - lastPloughTime >= ploughingInterval)
            {
                Invoke("InstantiatePloughedTile", 0.2f);
                lastPloughPosition = transform.position;
                lastPloughTime = Time.time;
            }

            
            UpdatePercentage();

            // Stop the tractor when it reaches the end of the path
            if (distanceTravelled >= totalPathLength)
            {
                foreach (var prefab in instantiatedPrefabs)
                {
                    Destroy(prefab);
                }
                instantiatedPrefabs.Clear();
                isMoving = false;
                isPloughingDone = true;
                ploughingdonepannel.SetActive(true);
            }
        }
    }


    public void StartMoving()
    {
        isMoving = true;
        //distanceTravelled = 0f;
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
        Anim.SetBool("Run", false);
        Anim.SetBool("idle", true);

        if (audioSource != null && idleClip != null)
        {
            audioSource.clip = idleClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void InstantiatePloughedTile()
    {
        if (ploughedTilePrefab != null)
        {
            GameObject newTile = Instantiate(ploughedTilePrefab, Ploughing_Position.position, Quaternion.identity);
            instantiatedPrefabs.Add(newTile);
            //ploughedArea += 1f;
        }
    }

    private void UpdatePercentage()
    {
        if (percentageText != null)
        {
            float percentage = Mathf.Clamp((distanceTravelled / totalPathLength) * 100f, 0f, 100f);
            percentageText.text = $"{percentage:F0}%";

            if (percentage >= 100f && imageToActivate != null)
            {
                imageToActivate.SetActive(true);
            }
        }
    }
}
