using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HeartManager : MonoBehaviour
{
    public RawImage heart1;
    public RawImage heart2;
    public RawImage heart3;
    public RawImage fadeEffect;  // The RawImage for the fade effect

    [Range(0, 3)]
    public int heartsLeft = 3;

    void Update()
    {
        UpdateHearts();
    }

    public void DecreaseHeart()
    {
        if (heartsLeft > 0)
        {
            heartsLeft--;
            UpdateHearts();
        }
    }

    void UpdateHearts()
    {
        heart1.gameObject.SetActive(heartsLeft >= 3);
        heart2.gameObject.SetActive(heartsLeft >= 2);
        heart3.gameObject.SetActive(heartsLeft >= 1);

        // If heartsLeft reaches 0, trigger the fade effect
        if (heartsLeft == 0 && !fadeEffect.gameObject.activeSelf)
        {
            fadeEffect.gameObject.SetActive(true);
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        // Set the alpha to 0 initially
        Color fadeColor = fadeEffect.color;
        fadeColor.a = 0;
        fadeEffect.color = fadeColor;

        // Gradually increase the alpha to 1
        float fadeDuration = 2f; // Duration of the fade
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            fadeColor.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeEffect.color = fadeColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the alpha is fully 1 at the end
        fadeColor.a = 1;
        fadeEffect.color = fadeColor;

        // Load the new scene after the fade effect
        SceneManager.LoadScene("FramedAsKiller");  // Replace "SampleScene" with the actual scene name
    }
}