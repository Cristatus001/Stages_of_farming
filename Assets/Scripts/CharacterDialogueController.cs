/*using System.Collections;
using UnityEngine;

public class CharacterDialogueController : MonoBehaviour
{
    public AudioClip dialogueClip;     // Declare this at the class level
    public Animator animator;          // Assign your Animator
    private float audioDelay = 0.5f;      // Delay before the audio starts
    private float animationDelay = 0.5f;  // Delay before the animation starts

    private AudioSource audioSource;

    private void Start()
    {
        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Start a unified coroutine for both animation and audio
        StartCoroutine(PlayAnimationAndAudioWithDelay());
    }

    private IEnumerator PlayAnimationAndAudioWithDelay()
    {
        // Determine the maximum delay between audioDelay and animationDelay
        float maxDelay = Mathf.Max(audioDelay, animationDelay);

        // Wait for the longer delay
        yield return new WaitForSeconds(maxDelay);

        // Start the animation if animator is assigned
        if (animator != null)
        {
            animator.SetBool("IsTalking", true);
            Debug.Log("Animation started after delay of " + maxDelay + " seconds");
        }
        else
        {
            Debug.LogError("Animator is not assigned");
        }

        // Play the dialogue audio
        if (dialogueClip != null)
        {
            audioSource.clip = dialogueClip;
            audioSource.Play();
            Debug.Log("Audio started after delay of " + maxDelay + " seconds");

            // Stop the animation when the audio ends
            StartCoroutine(StopAnimationAfterDialogue());
        }
        else
        {
            Debug.LogError("AudioClip is not assigned");
        }
    }

    private IEnumerator StopAnimationAfterDialogue()
    {
        // Wait for the duration of the audio clip
        yield return new WaitForSeconds(dialogueClip.length);

        // Stop the animation
        if (animator != null)
        {
            animator.SetBool("IsTalking", false);
            Debug.Log("Animation stopped");
        }
    }
}
*/