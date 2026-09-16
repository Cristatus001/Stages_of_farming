using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TractorController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform targetPosition; // Assign the target position in the Inspector
    private bool shouldMove = false;

    // Audio elements
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip idleClip; // Clip to play in idle mode
    public AudioClip moveClip; // Clip to play when moving

    public Animator Anim;
    public GameObject wheatCropPrefab; // Prefab of the wheat crops (optional)

    void Start()
    {
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
            // Move the tractor towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            // Check if the object has reached the target position
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
            {
                transform.position = targetPosition.position; // Snap to target position
                shouldMove = false; // Stop movement
            }
        }
    }

    // This method starts the tractor's movement
    public void StartMoving()
    {
        shouldMove = true;
        Anim.SetBool("Run", true);

        // Play movement clip when moving
        if (audioSource != null && moveClip != null)
        {
            audioSource.clip = moveClip;
            audioSource.loop = true; // Loop the movement sound
            audioSource.Play();
        }
    }

    // This method stops the tractor's movement
    public void StopMoving()
    {
        shouldMove = false;
        Anim.SetBool("Run", false);

        // Stop movement clip and switch back to idle clip
        if (audioSource != null && idleClip != null)
        {
            audioSource.clip = idleClip;
            audioSource.loop = true;
            audioSource.Play(); // Play idle sound again
        }
    }

    // Destroy wheat crops when the tractor touches them
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WheatCrop")) // Tag all wheat crops as "WheatCrop"
        {
            Destroy(collision.gameObject); // Destroy the wheat crop when the tractor touches it
            Debug.Log("Wheat crop destroyed!");

            // Optionally play a sound effect or trigger an animation here
        }
    }
}
