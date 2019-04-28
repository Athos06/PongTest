using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScreenFader : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup faderCanvasGroup;
    [SerializeField]
    private float fadeDuration = 1f;

    private bool isFading = false;
    private Coroutine fadeCoroutine;

    public void StartFadeOut(Action fadeInFinishedCallback = null)
    {
        fadeCoroutine = StartCoroutine(Fade(1, fadeInFinishedCallback));
    }

    public void StartFadeIn(Action fadeInFinishedCallback = null)
    {
        fadeCoroutine = StartCoroutine(Fade(0, fadeInFinishedCallback));
    }

    private IEnumerator Fade(float finalAlpha, Action fadeFinishedCallback = null)
    {
        isFading = true;

        // Make sure the CanvasGroup blocks raycasts into the scene so no more input can be accepted.
        faderCanvasGroup.blocksRaycasts = true;

        // Calculate how fast the CanvasGroup should fade based on it's current alpha, it's final alpha and how long it has to change between the two.
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        // While the CanvasGroup hasn't reached the final alpha yet...
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            // ... move the alpha towards it's target alpha.
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.unscaledDeltaTime);

            // Wait for a frame then continue.
            yield return null;
        }

        // Set the flag to false since the fade has finished.
        isFading = false;

        // Stop the CanvasGroup from blocking raycasts so input is no longer ignored.
        faderCanvasGroup.blocksRaycasts = false;

        if (fadeFinishedCallback != null)
            fadeFinishedCallback.Invoke(); 
    }


}
