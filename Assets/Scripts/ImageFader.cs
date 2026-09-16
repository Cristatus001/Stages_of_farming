using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageFade : MonoBehaviour
{
    public Image imageToFadeOut; // The image that will fade out
    public Image imageToFadeIn;  // The image that will fade in
    public GameObject newPanel;  // The panel to enable after the transition
    public float fadeDuration = 1.0f; // Duration of the fade effect
    public GameObject faderPanel; // The panel that triggers the fade

    private void OnEnable()
    {
        // Subscribe to the event when the fader panel is activated
        if (faderPanel != null)
        {
            faderPanel.SetActive(false); // Ensure the fader panel starts inactive
            StartCoroutine(WaitForFaderPanel());
        }
    }

    private IEnumerator WaitForFaderPanel()
    {
        // Wait until the fader panel is active
        yield return new WaitUntil(() => faderPanel.activeSelf);

        // Start the fade transition once the fader panel is active
        StartCoroutine(FadeImages());
    }

    private IEnumerator FadeImages()
    {
        // Ensure the image to fade in is fully transparent at the start
        imageToFadeIn.color = new Color(imageToFadeIn.color.r, imageToFadeIn.color.g, imageToFadeIn.color.b, 0);

        // Calculate the fade step based on the duration
        float fadeStep = Time.deltaTime / fadeDuration;

        // Fade out the first image and fade in the second image simultaneously
        for (float t = 0; t <= 1.0f; t += fadeStep)
        {
            // Fade out the first image
            imageToFadeOut.color = new Color(imageToFadeOut.color.r, imageToFadeOut.color.g, imageToFadeOut.color.b, 1.0f - t);

            // Fade in the second image
            imageToFadeIn.color = new Color(imageToFadeIn.color.r, imageToFadeIn.color.g, imageToFadeIn.color.b, t);

            // Wait until the next frame
            yield return null;
        }

        // Ensure the images are fully faded in/out at the end of the transition
        imageToFadeOut.color = new Color(imageToFadeOut.color.r, imageToFadeOut.color.g, imageToFadeOut.color.b, 0);
        imageToFadeIn.color = new Color(imageToFadeIn.color.r, imageToFadeIn.color.g, imageToFadeIn.color.b, 1.0f);

        // Activate the new panel after the fade transition is complete
        newPanel.SetActive(true);
    }
}
