using UnityEngine;
using UnityEngine.Video;

public class VideoEndHandler : MonoBehaviour
{
    public GameObject panelToDisable; // Assign the panel or GameObject to disable in the Inspector

    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // Ensure the VideoPlayer component is assigned
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found!");
            return;
        }

        // Register to the event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // This will be called when the video ends
        Debug.Log("Video has ended.");

        // Disable the panel or GameObject
        if (panelToDisable != null)
        {
            panelToDisable.SetActive(false);
        }
        else
        {
            Debug.LogError("Panel to disable is not assigned!");
        }
    }

    void OnDestroy()
    {
        // Unregister from the event when the script is destroyed
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
