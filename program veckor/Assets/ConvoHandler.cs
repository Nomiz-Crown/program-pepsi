using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class ConvoHandler : MonoBehaviour
{
    public GameObject gregerUi;       // The main UI that activates with E
    public GameObject interactUi;     // "Press E to Interact" UI
    public Text dialogueText;         // Text component for dialogue
    public Button button1;            // First button for primary dialogue
    public Button button2;            // Second button for alternate dialogue
    public Button npc1button;
    public Button npc2button;
    public Button npc3button;
    public Button npc4button;
    public Button npc5button;
    public Button corpsevidencebutton;
    public int corpsevidence = 0;

    public int npcroom;
    static int playeroom;

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
    float evidence;
    public float likeYouAmount = 1;
    public float suspectYouAmount = 0;
    public float suspectNpc1Amount = 0;
    public float likeNpc1Amount = 0;
    public float suspectNpc2Amount = 0;
    public float likeNpc2Amount = 0;
    public float suspectNpc3Amount = 0;
    public float likeNpc3Amount = 0;
    public float suspectNpc4Amount = 0;
    public float likeNpc4Amount = 0;
    public float suspectNpc5Amount = 0;
    public float likeNpc5Amount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playeroom = npcroom;
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
            npcbuttons(false);
            corpsevidencebutton.gameObject.SetActive(false);

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

    
       yield return new WaitForSeconds(1f); // Adjust the time here (2 seconds)

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

    }
    public void sayevidencebuttonclick()
    {
        if (corpsevidence == 1 && npcroom == playeroom)
        {
            corpsevidencebutton.gameObject.SetActive(true);
        }
    }
    public void corpse()
    {
        evidence = 0.5f;
        npcbuttons(true);
        if (playeroom == npcroom)
        {
            Debug.Log(npcroom);
            Debug.Log(playeroom);
            Debug.Log(corpsevidence);
            corpsevidence = 0;
        }
    }
    void npcbuttons(bool e)
    {
        if (npc1button != null)
        {
            npc1button.gameObject.SetActive(e);
        }
        if (npc2button != null)
        {
            npc2button.gameObject.SetActive(e);
        }
        if (npc3button != null)
        {
            npc3button.gameObject.SetActive(e);
        }
        if (npc4button != null)
        {
            npc4button.gameObject.SetActive(e);
        }
        if (npc5button != null)
        {
            npc5button.gameObject.SetActive(e);
        }
    }
    public void npc1buttonpress()
    {
        if (npcroom == playeroom && evidence != 0)
        {
            accusation(evidence, 1, 0);
        }
    }
    public void npc2buttonpress()
    {
        if (npcroom == playeroom && evidence != 0)
        {
            accusation(evidence, 2, 0);
        }
    }
    public void npc3buttonpress()
    {
        if (npcroom == playeroom && evidence != 0)
        {
            accusation(evidence, 3, 0);
        }
    }
    public void npc4buttonpress()
    {
        if (npcroom == playeroom && evidence != 0)
        {
            accusation(evidence, 4, 0);
        }
    }
    public void npc5buttonpress()
    {
        if (npcroom == playeroom && evidence != 0)
        {
            accusation(evidence, 5, 0);
        }
    }
    void accusation(float moresuspiciousamount, int whoismentiond, int whoistalikng)
    {
        npcbuttons(false);

        corpsevidencebutton.gameObject.SetActive(false);
        
        if (whoistalikng == 0)
        {
            if (whoismentiond == 1)
            {
                if (likeYouAmount + suspectNpc1Amount> suspectYouAmount + likeNpc1Amount)
                {
                    if (likeYouAmount - (suspectYouAmount + likeNpc1Amount) > 0)
                    {
                        suspectNpc1Amount += moresuspiciousamount * (1 + likeYouAmount - (suspectYouAmount + likeNpc1Amount));
                    }
                    else
                    {
                        suspectNpc1Amount += moresuspiciousamount;
                    } 
                    if (suspectNpc1Amount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("(npc 1 namn) is the killer!!"));
                        
                    }
                    else if(suspectNpc1Amount > 1.5f)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("perhaps (npc 1 namn) is the killer?"));
                    }
                    else
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("you think (npc 1 namn) is the killer?"));
                    }
                }
                else
                {
                    if (likeNpc1Amount - (suspectNpc1Amount + likeYouAmount) > 0)
                    {
                        suspectYouAmount += moresuspiciousamount * (1 + likeNpc1Amount - (likeYouAmount + suspectNpc1Amount));
                    }
                    else
                    {
                        suspectYouAmount += moresuspiciousamount;
                    }
                    if (suspectYouAmount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("..."));

                    }
                    else if (suspectYouAmount > 1.5f)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("stop lying"));
                    }
                    else
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("he's/she's inocent"));
                    }
                }
            }
            else if (whoismentiond == 2)
            {
                if (likeYouAmount + suspectNpc2Amount > suspectYouAmount + likeNpc2Amount)
                {
                    if (likeYouAmount - (suspectYouAmount + likeNpc2Amount) > 0)
                    {
                        suspectNpc2Amount += moresuspiciousamount * (1 + likeYouAmount - (suspectYouAmount + likeNpc2Amount));
                    }
                    else
                    {
                        suspectNpc2Amount += moresuspiciousamount;
                    }
                    if (suspectNpc2Amount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("(npc 2 namn) is the killer!!"));

                    }
                    else if (suspectNpc2Amount > 1.5f)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("perhaps (npc 2 namn) is the killer?"));
                    }
                    else
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("you think (npc 2 namn) is the killer?"));
                    }
                }
                else
                {
                   if (likeNpc2Amount - (suspectNpc2Amount + likeYouAmount) > 0)
                    {
                        suspectYouAmount += moresuspiciousamount * (1 + likeNpc2Amount - (likeYouAmount + suspectNpc2Amount));
                    }
                    else
                    {
                        suspectYouAmount += moresuspiciousamount;
                    }
                    if (suspectYouAmount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("..."));

                    }
                    else if (suspectYouAmount > 1.5f)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("stop lying"));
                    }
                    else
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("he's/she's inocent"));
                    }
                }
            }
            else if (whoismentiond == 3)
            {
                if (likeYouAmount + suspectNpc3Amount > suspectYouAmount + likeNpc3Amount)
                {
                    if (likeYouAmount - (suspectYouAmount + likeNpc3Amount) > 0)
                    {
                        suspectNpc3Amount += moresuspiciousamount * (1 + likeYouAmount - (suspectYouAmount + likeNpc3Amount));
                    }
                    else
                    {
                        suspectNpc3Amount += moresuspiciousamount;
                    }
                    if (suspectYouAmount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("(npc 3 namn) is the killer!!"));

                    }
                    else if (suspectYouAmount > 1.5f)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("perhaps (npc 3 namn) is the killer?"));
                    }
                    else
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("you think (npc 3 namn) is the killer?"));
                    }
                }
                else
                {
                    if (likeNpc3Amount - (suspectNpc3Amount + likeYouAmount) > 0)
                    {
                        suspectYouAmount += moresuspiciousamount * (1 + likeNpc3Amount - (likeYouAmount + suspectNpc3Amount));
                    }
                    else
                    {
                        suspectYouAmount += moresuspiciousamount;
                    }
                    if (suspectYouAmount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("..."));

                    }
                    else if (suspectYouAmount > 1.5f)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("stop lying"));
                    }
                    else
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("he's/she's inocent"));
                    }
                }
            }
            else if (whoismentiond == 4)
            {
                if (likeYouAmount + suspectNpc4Amount > suspectYouAmount + likeNpc4Amount)
                {
                    if (likeYouAmount - (suspectYouAmount + likeNpc4Amount) > 0)
                    {
                        suspectNpc4Amount += moresuspiciousamount * (1 + likeYouAmount - (suspectYouAmount + likeNpc4Amount));
                    }
                    else
                    {
                        suspectNpc4Amount += moresuspiciousamount;
                    }
                    if (suspectNpc4Amount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("(npc 4 namn) is the killer!!"));

                    }
                    else if (suspectNpc4Amount > 1.5f)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("perhaps (npc 4 namn) is the killer?"));
                    }
                    else
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("you think (npc 4 namn) is the killer?"));
                    }
                }
                else
                {
                    if (likeNpc4Amount - (suspectNpc4Amount + likeYouAmount) > 0)
                    {
                        suspectYouAmount += moresuspiciousamount * (1 + likeNpc4Amount - (likeYouAmount + suspectNpc4Amount));
                    }
                    else
                    {
                        suspectYouAmount += moresuspiciousamount;
                    }
                    if (suspectYouAmount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("..."));

                    }
                    else if (suspectYouAmount > 1.5f)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("stop lying"));
                    }
                    else
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("he's/she's inocent"));
                    }
                }
            }
            else if (whoismentiond == 5)
            {
                if (likeYouAmount + suspectNpc5Amount > suspectYouAmount + likeNpc5Amount)
                {
                    if (likeYouAmount - (suspectYouAmount + likeNpc5Amount) > 0)
                    {
                        suspectNpc5Amount += moresuspiciousamount * (1 + likeYouAmount - (suspectYouAmount + likeNpc5Amount));
                    }
                    else
                    {
                        suspectNpc5Amount += moresuspiciousamount;
                    }
                    if (suspectNpc5Amount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("(npc 5 namn) is the killer!!"));

                    }
                    else if (suspectNpc5Amount > 1.5f)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("perhaps (npc 5 namn) is the killer?"));
                    }
                    else
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("you think (npc 5 namn) is the killer?"));
                    }
                }
                else
                {
                    if (likeNpc5Amount - (suspectNpc5Amount + likeYouAmount) > 0)
                    {
                        suspectYouAmount += moresuspiciousamount * (1 + likeNpc5Amount - (likeYouAmount + suspectNpc5Amount));
                    }
                    else
                    {
                        suspectYouAmount += moresuspiciousamount;
                    }
                    if (suspectYouAmount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("..."));

                    }
                    else if (suspectYouAmount > 1.5f)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("stop lying"));
                    }
                    else
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("he's/she's inocent"));
                    }
                }
            }
        }
        evidence = 0;
    }
}