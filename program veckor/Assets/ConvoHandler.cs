using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class ConvoHandler : MonoBehaviour
{
    public GameObject gregerUi;       // The main UI that activates with E
    public GameObject interactUi;     // "Press E to Interact" UI
    public Text dialogueText;         // Text component for dialogue
    public Button button1;            // First button for primary dialogue
    public Button button2;            // Second button for alternate dialogue

    [TextArea(3, 10)]
    public string startingMessage;    // Message displayed before paths
    [TextArea(3, 10)]
    public List<string> dialogueLines1 = new List<string>(); // Dialogue path 1
    [TextArea(3, 10)]
    public List<string> dialogueLines2 = new List<string>(); // Dialogue path 2

    public float typingSpeed = 0.05f; // Time delay between letters

    private int currentLineIndex = 0; // Tracks the current dialogue line
    private List<string> activeDialogue; // Holds the current dialogue path
    private Coroutine typingCoroutine; // Tracks the typing coroutine
    private bool isPlayerInTrigger = false;
    private bool showingStartingMessage = true; // Tracks if we’re in the starting phase

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = true;

            // Show the interaction UI when player enters the trigger
            if (interactUi != null)
            {
                interactUi.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;

            // Hide both UIs and reset dialogue state
            if (interactUi != null)
            {
                interactUi.SetActive(false);
            }

            if (gregerUi != null)
            {
                gregerUi.SetActive(false);
            }

            ShowButtons(false, false); // Hide both buttons

            // Reset dialogue state
            currentLineIndex = 0;
            showingStartingMessage = true;
            activeDialogue = null;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
        }
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            // Hide interact UI and show greger UI
            if (interactUi != null)
            {
                interactUi.SetActive(false);
            }

            if (gregerUi != null)
            {
                gregerUi.SetActive(true);

                if (showingStartingMessage)
                {
                    // Display the starting message
                    ShowButtons(false, false); // Hide buttons during starting message
                    if (typingCoroutine != null)
                    {
                        StopCoroutine(typingCoroutine);
                    }

                    typingCoroutine = StartCoroutine(TypeSentence(startingMessage));
                }
            }
        }
    }

    private void StartDialogue(List<string> dialogueLines)
    {
        activeDialogue = dialogueLines; // Set the current dialogue path
        currentLineIndex = 0; // Start at the first line
        ShowButtons(false, false); // Hide the buttons initially

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(activeDialogue[currentLineIndex]));
    }

    private IEnumerator TypeSentence(string sentence)
    {
         dialogueText.text = ""; // Clear the text before typing
         foreach (char letter in sentence.ToCharArray())
       {
        dialogueText.text += letter; // Add one letter at a time
        yield return new WaitForSeconds(typingSpeed); // Wait for the next letter
       }

    
       yield return new WaitForSeconds(2f); // Adjust the time here (2 seconds)

       typingCoroutine = null; // Reset the coroutine

       if (showingStartingMessage)
       {
        // After the starting message, show buttons for path selection
        ShowButtons(true, true);
        showingStartingMessage = false;
       }
       else
       {
        // If in the middle of a dialogue path, show button1 if there are more lines
        if (currentLineIndex + 1 < activeDialogue.Count)
        {
            ShowButtons(true, false); // Only button1 for next line
        }
        else
        {
            ShowButtons(false, false); // No buttons if dialogue is finished
            gregerUi.SetActive(false); // Hide the UI
        }
       }
    }


    private void ShowButtons(bool showButton1, bool showButton2)
    {
        if (button1 != null)
        {
            button1.gameObject.SetActive(showButton1);
        }

        if (button2 != null)
        {
            button2.gameObject.SetActive(showButton2);
        }
    }

    // Button 1 click event to proceed to the next line of dialogue or start path 1
    public void OnButton1Click()
    {
        if (!showingStartingMessage && activeDialogue == null)
        {
            StartDialogue(dialogueLines1); // Start path 1 dialogue
        }
        else if (activeDialogue != null)
        {
            currentLineIndex++;
            if (currentLineIndex < activeDialogue.Count)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }

                typingCoroutine = StartCoroutine(TypeSentence(activeDialogue[currentLineIndex]));
            }
        }
    }

    // Button 2 click event to start path 2 dialogue
    public void OnButton2Click()
    {
        if (!showingStartingMessage)
        {
            StartDialogue(dialogueLines2); // Start path 2 dialogue
        }
    }
}