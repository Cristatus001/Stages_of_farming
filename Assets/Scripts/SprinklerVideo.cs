using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SprinklerVideo : MonoBehaviour
{
    
    public VideoPlayer videoPlayer;  // Reference to the VideoPlayer component
    public GameObject videoPanel;    // Reference to the panel holding the video
    public GameObject newPanel;      // Reference to the new panel to show after the video

    void Start()
    {

        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished; // Subscribe to the event
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Disable the video panel
        videoPanel.SetActive(false);

        // Enable the new panel
        newPanel.SetActive(true);

        Debug.Log("Video finished playing, video panel closed, new panel opened.");
    }
}