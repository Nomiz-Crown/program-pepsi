using UnityEngine;
using System.Collections.Generic;

public class RoomDetection : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float alphaWhenTouched = 0f;   // Alpha when touched (0 for fully transparent)
    public float alphaWhenNotTouched = 0.7f;  // Alpha when not touched (0.7 for semi-transparent)

    // List to keep track of all predefined objects in the room
    public List<GameObject> roomObjects = new List<GameObject>();

    void Start()
    {
        // Get the SpriteRenderer component of the room transparency square
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Store the original color of the sprite
        originalColor = spriteRenderer.color;
        
        // Set the initial transparency to alphaWhenNotTouched
        SetAlpha(alphaWhenNotTouched);
        AdjustRoomObjectsZPosition(11f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // When the player touches the object, set alpha to alphaWhenTouched
        if (other.CompareTag("Player")) // Ensure the player's collider has the "Player" tag
        {
            SetAlpha(alphaWhenTouched);
            AdjustRoomObjectsZPosition(1f); // Move room objects to Z = 11 when player enters
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // When the player stops touching the object, set alpha to alphaWhenNotTouched
        if (other.CompareTag("Player"))
        {
            SetAlpha(alphaWhenNotTouched);
            AdjustRoomObjectsZPosition(11f); // Move room objects back to Z = 0 when player exits
        }
    }

    void SetAlpha(float alpha)
    {
        // Set the alpha value of the sprite while keeping its original color
        Color color = originalColor;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    void AdjustRoomObjectsZPosition(float zPosition)
    {
        // Change the Z position of all objects in the roomObjects list
        foreach (GameObject roomObject in roomObjects)
        {
            if (roomObject != null)
            {
                // Adjust Z position
                Vector3 objectPosition = roomObject.transform.position;
                objectPosition.z = zPosition;
                roomObject.transform.position = objectPosition;
            }
        }
    }
}