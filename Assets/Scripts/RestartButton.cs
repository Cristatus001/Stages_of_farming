using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void OnRestartButtonPressed()
    {
        // Get the currently active scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the active scene
        SceneManager.LoadScene(currentSceneName);
    }
}
