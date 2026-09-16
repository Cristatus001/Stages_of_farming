using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public Button continueButton;   // Assign in Inspector
    public GameObject videoPanel;   // Assign in Inspector
    public VideoPlayer videoPlayer; // Assign in Inspector
    public GameObject nextPanel;    // Assign in Inspector for the next panel

    void Start()
    {
        // Ensure the video panel is hidden initially
        videoPanel.SetActive(false);
        continueButton.onClick.AddListener(OnContinueButtonClick);
    }

    void OnContinueButtonClick()
    {
        // Show the video panel
        videoPanel.SetActive(true);
        // Play the video
        videoPlayer.Play();
        // Add a callback for when the video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Hide the video panel
        videoPanel.SetActive(false);
        // Show the next panel
        if (nextPanel != null)
        {
            nextPanel.SetActive(true);
        }
        // Optionally, you can deactivate or hide the current panel here if needed
    }
}
