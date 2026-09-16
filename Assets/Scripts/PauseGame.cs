using UnityEngine;

public class PauseGame : MonoBehaviour
{
    // Boolean to track game state
    private bool isPaused = false;

    // Method to pause the game
    public void Pause()
    {
        //if (!isPaused)
        {
            Time.timeScale = 0f; // Pauses the game
            AudioListener.pause = true; // Pauses all audio
            isPaused = true;
        }
    }

    // Method to resume the game
    public void Resume()
    {
        //if (isPaused)
        {
            Time.timeScale = 1f; // Resumes the game
            AudioListener.pause = false; // Resumes all audio
            isPaused = false;
        }
    }
}
