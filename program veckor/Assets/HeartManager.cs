using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public RawImage heart1;
    public RawImage heart2;
    public RawImage heart3;

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
    }
}
