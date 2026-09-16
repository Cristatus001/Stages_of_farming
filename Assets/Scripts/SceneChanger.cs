using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string MainMenu)
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnRestartButtonPressed()
    {
        // Get the currently active scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the active scene
        SceneManager.LoadScene(currentSceneName);
    }
}
