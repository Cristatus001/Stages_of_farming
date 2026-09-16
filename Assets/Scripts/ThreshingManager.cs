using UnityEngine;
using TMPro;
using System.Collections;

public class ThreshingManager : MonoBehaviour
{
    public GameObject threshingDonePanel;
    public TextMeshProUGUI percentageText;
    public int totalObjects = 5; // Total number of objects to destroy
    public AudioSource audioSource; // Reference to the AudioSource component
    private static int destroyedObjects = 0;
    private float percentageIncrease = 12f;

    void Start()
    {
        ResetStaticVariables();
        if (threshingDonePanel != null)
        {
            threshingDonePanel.SetActive(false);
        }
        UpdatePercentageText();
    }

    public void ObjectDestroyed()
    {
        destroyedObjects++;
        UpdatePercentageText();

        if (destroyedObjects >= totalObjects)
        {
            StartCoroutine(EnableThreshingDonePanelWithDelay(3f));
        }
    }

    private void UpdatePercentageText()
    {
        float percentage = destroyedObjects * percentageIncrease;
        if (percentageText != null)
        {
            percentageText.text = Mathf.Clamp(percentage, 0, 100).ToString("F0") + "%"; // Ensure percentage doesn't exceed 100%
        }
    }

    private IEnumerator EnableThreshingDonePanelWithDelay(float delay)
    {
        Debug.Log("Waiting to enable threshingDonePanel for " + delay + " seconds");
        yield return new WaitForSeconds(delay);

        if (threshingDonePanel != null)
        {
            Debug.Log("Enabling threshingDonePanel");
            threshingDonePanel.SetActive(true);

            // Mute the audio when the threshing done panel is enabled
            if (audioSource != null)
            {
                audioSource.mute = true;
            }
            else
            {
                Debug.LogWarning("AudioSource is not assigned!");
            }
        }
        else
        {
            Debug.LogError("threshingDonePanel is null!");
        }
    }

    void ResetStaticVariables()
    {
        destroyedObjects = 0;
    }
}
