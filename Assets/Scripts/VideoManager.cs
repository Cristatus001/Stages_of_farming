using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer1;
    public VideoPlayer videoPlayer2;
    public Button button1; // Button containing the first video thumbnail
    public Button button2; // Button containing the second video thumbnail
    public Button continueButton;

    private bool video1Opened = false; // Track if video1 is opened
    private bool video2Opened = false; // Track if video2 is opened

    public string videoName_1;
    public string videoName_2;

    void Start()
    {
        // Add listeners to the buttons to start the videos
        if (button1 != null)
        {
            button1.onClick.AddListener(PlayVideo1);
        }

        if (button2 != null)
        {
            button2.onClick.AddListener(PlayVideo2);
        }

        // Initially hide the continue button
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
        }
        //PlayVideo_1();
    }

    public void PlayVideo_1()
    {
        if (videoPlayer1 != null)
        {
            Debug.Log("SS");
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoName_1);
            videoPlayer1.url = videoPath;
            videoPlayer1.Play();
            video1Opened = true; // Mark video1 as opened
            CheckIfAllVideosOpened();

        }
    }
    public void PlayVideo_2()
    {
        if (videoPlayer2 != null)
        {

            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoName_2);
            Debug.Log(videoPath);
            videoPlayer2.url = videoPath;
            videoPlayer2.Play();
            video2Opened = true; // Mark video1 as opened
            CheckIfAllVideosOpened();
        }
    }
    private void PlayVideo1()
    {
        if (videoPlayer1 != null)
        {
            videoPlayer1.Play();
            video1Opened = true; // Mark video1 as opened
            CheckIfAllVideosOpened();
        }
    }

    private void PlayVideo2()
    {
        if (videoPlayer2 != null)
        {
            videoPlayer2.Play();
            video2Opened = true; // Mark video2 as opened
            CheckIfAllVideosOpened();
        }
    }

    // Check if both videos have been opened (played at least once)
    private void CheckIfAllVideosOpened()
    {
        if (video1Opened && video2Opened)
        {
            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(true); // Enable continue button
            }
        }
    }

    void OnDestroy()
    {
        // Clean up button listeners if necessary
        if (button1 != null)
        {
            button1.onClick.RemoveListener(PlayVideo1);
        }

        if (button2 != null)
        {
            button2.onClick.RemoveListener(PlayVideo2);
        }
    }
}