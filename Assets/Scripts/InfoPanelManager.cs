using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoPanelManager : MonoBehaviour
{
    public GameObject infoPanel;  // Reference to the Info panel
    private string previousScene; // To store the previous scene name

    // Call this method when the Info button is pressed
    public void OnInfoButtonPressed()
    {
        // Store the current scene's name before switching
        previousScene = SceneManager.GetActiveScene().name;

        // Load the Info scene or show the Info panel
        SceneManager.LoadScene("InfoScene");

        // Optionally pause the game
        Time.timeScale = 0;
    }

    // Call this method when the Continue button is pressed
    public void OnContinueButtonPressed()
    {
        // Close the Info Panel
        infoPanel.SetActive(false);

        // Return to the game scene
        SceneManager.LoadScene(previousScene);

        // Resume the game if it was paused
        Time.timeScale = 1;
    }
}
