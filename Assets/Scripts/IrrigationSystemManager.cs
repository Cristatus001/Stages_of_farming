using UnityEngine;
using UnityEngine.UI;

public class IrrigationSystemManager : MonoBehaviour
{
    public int totalPipes; // Set this to the total number of pipes needed
    private int placedPipes = 0;
    public Text percentageText; // UI Text to show the percentage

    public void PipePlaced()
    {
        placedPipes++;
        UpdatePercentage();
    }

    private void UpdatePercentage()
    {
        float percentage = (float)placedPipes / totalPipes * 100f;
        percentageText.text = percentage.ToString("0") + "%";

        if (placedPipes == totalPipes)
        {
            // All pipes are placed
            Debug.Log("Irrigation system complete!");
        }
    }
}
