using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menubackroundshift : MonoBehaviour
{
    public RawImage backgroundImage;
    public float parallaxStrength = 10f;

    private Vector2 initialPosition;

    void Start()
    {
        if (backgroundImage == null)
            backgroundImage = GetComponent<RawImage>();

        initialPosition = backgroundImage.rectTransform.anchoredPosition;
    }

    void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        float moveX = (mousePosition.x - Screen.width / 2) / Screen.width;
        float moveY = (mousePosition.y - Screen.height / 2) / Screen.height;

        Vector2 offset = new Vector2(moveX, moveY) * parallaxStrength;
        backgroundImage.rectTransform.anchoredPosition = initialPosition + offset;
    }
}