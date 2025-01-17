using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeout : MonoBehaviour
{
    public RawImage uiRawImage;      // Assign the RawImage UI element here
    public float fadeDuration = 2f;  // Duration of the fade-out effect in seconds

    private void Start()
    {
        if (uiRawImage != null)
        {
            StartCoroutine(FadeOutRawImage());
        }
        else
        {
            Debug.LogError("RawImage is not assigned.");
        }
    }

    private System.Collections.IEnumerator FadeOutRawImage()
    {
        float elapsedTime = 0f;
        Color startColor = uiRawImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            uiRawImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        uiRawImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}
