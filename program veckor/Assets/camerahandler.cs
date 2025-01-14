using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float smoothSpeed = 0.125f;  // Smoothing factor for camera movement
    public Vector3 offset;  // Offset from the player
    public float dampingTime = 0.1f;  // Time to smooth the camera movement

    private Vector3 velocity = Vector3.zero;  // For SmoothDamp calculation

    void LateUpdate()
    {
        if (player != null)
        {
            // The desired position of the camera, based on the player's position
            Vector3 desiredPosition = player.position + offset;

            // Ensure the camera only moves along the X and Y axes for 2D
            desiredPosition.z = transform.position.z;

            // Smoothly move the camera towards the desired position using SmoothDamp
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, dampingTime);
        }
    }
}