using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // This function will be called when the "Continue" button is clicked
    public void LoadNextScene()
    {
        // Load the next scene based on the build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
