using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    public float fadeDuration = 1f; // Duration of the fading animation
    public Texture[] gestureImages; // Array of gesture images
    public RawImage displayedGesture; // Reference to the RawImage component

    private Coroutine currentFadeCoroutine; // Reference to the currently running fade coroutine

    // Call this method to change the displayed gesture image with fading
    public void ChangeGestureWithFade(int gestureIndex)
    {
        if (currentFadeCoroutine != null)
        {
            // Stop the currently running fade coroutine if it exists
            StopCoroutine(currentFadeCoroutine);
        }

        // Start the fade coroutine to fade out the current image
        currentFadeCoroutine = StartCoroutine(FadeOutAndChangeGesture(gestureIndex));
    }

    // Coroutine to fade out the current image, change to the new image, and fade it in
    private IEnumerator FadeOutAndChangeGesture(int gestureIndex)
    {
        // Fade out the current image
        float timer = 0f;
        Color startColor = displayedGesture.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            displayedGesture.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            yield return null;
        }

        // Change the image
        displayedGesture.texture = gestureImages[gestureIndex];

        // Fade in the new image
        timer = 0f;
        startColor = new Color(displayedGesture.color.r, displayedGesture.color.g, displayedGesture.color.b, 0f);
        endColor = new Color(displayedGesture.color.r, displayedGesture.color.g, displayedGesture.color.b, 1f);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            displayedGesture.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            yield return null;
        }

        currentFadeCoroutine = null; // Reset the reference to the fade coroutine
    }
}
