using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class Tractor : MonoBehaviour
{
    public float speed = 5f;  // Tractor speed
    private bool isMoving = false;  // Movement flag
    public GameObject panel;  // First panel to open
    public GameObject secondPanel;  // Second panel to open after video ends
    public VideoPlayer videoPlayer;  // Reference to the Video Player component
    public float delay = 2f;  // Delay before showing the first panel

    public AudioSource idleAudioSource;  // Audio source for idle sound
    public AudioSource movingAudioSource;  // Audio source for moving sound

    public VideoManager videoManager;

    void Start()
    {
        // Subscribe to the VideoPlayer event to detect when the video finishes
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        // Start playing the idle audio initially
        if (idleAudioSource != null)
        {
            idleAudioSource.Play();
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTractor();

            // If tractor starts moving, stop idle sound and play moving sound
            if (!movingAudioSource.isPlaying && idleAudioSource.isPlaying)
            {
                idleAudioSource.Stop();
                movingAudioSource.Play();
            }
        }
        else
        {
            // If the tractor stops, stop moving sound and play idle sound
            if (movingAudioSource.isPlaying)
            {
                movingAudioSource.Stop();
                if (idleAudioSource != null)
                {
                    idleAudioSource.Play();
                }
            }
        }
    }

    // Move the tractor (for 2D movement)
    void MoveTractor()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);  // Tractor moves left
    }

    // Start moving the tractor
    public void StartMoving()
    {
        isMoving = true;
    }

    // Stop moving the tractor
    public void StopMoving()
    {
        isMoving = false;
    }

    // When the tractor hits the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriggerZone"))
        {
            StartCoroutine(OpenPanelAfterDelay());
            videoManager.PlayVideo_1();
        }
    }

    // Coroutine to open the first panel after a delay
    IEnumerator OpenPanelAfterDelay()
    {
        yield return new WaitForSeconds(delay);  // Wait for the set delay
        panel.SetActive(true);  // Enable the first panel
        gameObject.SetActive(false);  // Disable the player (tractor)
    }

    // When the video finishes, open the second panel
    void OnVideoFinished(VideoPlayer vp)
    {
        secondPanel.SetActive(true);  // Enable the second panel
        panel.SetActive(false);  // Optionally disable the first panel
    }
}
