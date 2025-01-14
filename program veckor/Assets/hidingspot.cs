using UnityEngine;

public class hidingspot : MonoBehaviour
{
    // Reference to the "E to interact" UI element (drag the UI element here in the inspector)
    public GameObject interactUI;
    // Reference to the "Press E to stop hiding" UI element (drag the UI element here in the inspector)
    public GameObject stopHidingUI;

    // Reference to the Player GameObject (drag the player object here in the inspector)
    public GameObject player;

    // Flag to check if the player is hiding
    private bool isHiding = false;

    private void Start()
    {
        // Ensure the UI elements are deactivated at the start
        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }

        if (stopHidingUI != null)
        {
            stopHidingUI.SetActive(false);
        }
    }

    // Trigger when another collider enters this collider (2D)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the Player object
        if (other.gameObject == player)
        {
            Debug.Log("Player entered the trigger zone");

            // Activate the "E to interact" UI element when the player enters the trigger zone
            if (interactUI != null)
            {
                interactUI.SetActive(true);
            }
        }
    }

    // Trigger when another collider exits this collider (2D)
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider is the Player object
        if (other.gameObject == player)
        {
            Debug.Log("Player exited the trigger zone");

            // Deactivate the "E to interact" UI element when the player leaves the trigger zone
            if (interactUI != null)
            {
                interactUI.SetActive(false);
            }

            // Hide the "Press E to stop hiding" UI if the player leaves the trigger zone
            if (stopHidingUI != null)
            {
                stopHidingUI.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if the player is inside the trigger zone (using OnTriggerStay2D) and presses "E"
        if (isHiding && Input.GetKeyDown(KeyCode.E) && player != null)
        {
            // Stop hiding: reactivate the player and hide the "stop hiding" UI
            player.SetActive(true);
            stopHidingUI.SetActive(false);
            isHiding = false;
        }
        else if (Input.GetKeyDown(KeyCode.E) && interactUI.activeSelf && player != null)
        {
            // Start hiding: deactivate the player and show the "stop hiding" UI
            player.SetActive(false);
            stopHidingUI.SetActive(true);
            isHiding = true;
        }
    }
}