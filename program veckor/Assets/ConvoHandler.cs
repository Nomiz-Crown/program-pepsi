using System;
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
    public Button npc1button;
    public Button npc2button;
    public Button npc3button;
    public Button npc4button;
    public Button npc5button;
    public Button corpsevidencebutton;

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
    public float likeyouamount = 1;
    public float suspectyouamount = 0;
    public float suspectnpc1amount = 0;
    public float likenpc1amount = 0;
    public float suspectnpc2amount = 0;
    public float likenpc2amount = 0;
    public float suspectnpc3amount = 0;
    public float likenpc3amount = 0;
    public float suspectnpc4amount = 0;
    public float likenpc4amount = 0;
    public float suspectnpc5amount = 0;
    public float likenpc5amount = 0;

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
            if (npc1button != null)
            {
                npc1button.gameObject.SetActive(false);
            }
            if (npc2button != null)
            {
                npc2button.gameObject.SetActive(false);
            }
            if (npc3button != null)
            {
                npc3button.gameObject.SetActive(false);
            }
            if (npc4button != null)
            {
                npc4button.gameObject.SetActive(false);
            }
            if (npc5button != null)
            {
                npc5button.gameObject.SetActive(false);
            }
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
       corpsevidencebutton.gameObject.SetActive(true);
    }
    public void corpse()
    {
        evidence = 0.5f;
        if (npc1button != null)
        {
            npc1button.gameObject.SetActive(true);
        }
        if (npc2button != null)
        {
            npc2button.gameObject.SetActive(true);
        }
        if (npc3button != null)
        {
            npc3button.gameObject.SetActive(true);
        }
        if (npc4button != null)
        {
            npc4button.gameObject.SetActive(true);
        }
        if (npc5button != null)
        {
            npc5button.gameObject.SetActive(true);
        }
    }
    public void npc1buttonpress()
    {
        if (npcroom == playeroom)
        {
            accusation(evidence, 1, 0);
        }
    }
    public void npc2buttonpress()
    {
        if (npcroom == playeroom)
        {
            accusation(evidence, 2, 0);
        }
    }
    public void npc3buttonpress()
    {
        if (npcroom == playeroom)
        {
            accusation(evidence, 3, 0);
        }
    }
    public void npc4buttonpress()
    {
        if (npcroom == playeroom)
        {
            accusation(evidence, 4, 0);
        }
    }
    public void npc5buttonpress()
    {
        if (npcroom == playeroom)
        {
            accusation(evidence, 5, 0);
        }
    }
    void accusation(float moresuspiciousamount, int whoismentiond, int whoistalikng)
    {
        if (whoistalikng == 0)
        {
            if (whoismentiond == 1)
            {
                if (likeyouamount + suspectnpc1amount > suspectyouamount + likenpc1amount)
                {
                    if (likeyouamount - (suspectyouamount + likenpc1amount) > 0)
                    {
                        suspectnpc1amount += moresuspiciousamount * (1 + likeyouamount - (suspectyouamount + likenpc1amount));
                    }
                    else
                    {
                        suspectnpc1amount += moresuspiciousamount;
                    }
                    if (suspectnpc1amount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("(npc 1 namn) is the killer!!"));
                        
                    }
                    else if(suspectnpc1amount > 1.5f)
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
                    if (likenpc1amount - (suspectnpc1amount + likeyouamount) > 0)
                    {
                        suspectyouamount += moresuspiciousamount * (1 + likenpc1amount - (likeyouamount + suspectnpc1amount));
                    }
                    else
                    {
                        suspectyouamount += moresuspiciousamount;
                    }
                    if (suspectnpc1amount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("..."));

                    }
                    else if (suspectnpc1amount > 1.5f)
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
                if (likeyouamount + suspectnpc2amount > suspectyouamount + likenpc2amount)
                {
                    if (likeyouamount - (suspectyouamount + likenpc2amount) > 0)
                    {
                        suspectnpc2amount += moresuspiciousamount * (1 + likeyouamount - (suspectyouamount + likenpc2amount));
                    }
                    else
                    {
                        suspectnpc2amount += moresuspiciousamount;
                    }
                    if (suspectnpc2amount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("(npc 2 namn) is the killer!!"));

                    }
                    else if (suspectnpc2amount > 1.5f)
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
                    if (likenpc2amount - (suspectnpc2amount + likeyouamount) > 0)
                    {
                        suspectyouamount += moresuspiciousamount * (1 + likenpc2amount - (likeyouamount + suspectnpc2amount));
                    }
                    else
                    {
                        suspectyouamount += moresuspiciousamount;
                    }
                    if (suspectyouamount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("..."));

                    }
                    else if (suspectyouamount > 1.5f)
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
                if (likeyouamount + suspectnpc3amount > suspectyouamount + likenpc3amount)
                {
                    if (likeyouamount - (suspectyouamount + likenpc3amount) > 0)
                    {
                        suspectnpc3amount += moresuspiciousamount * (1 + likeyouamount - (suspectyouamount + likenpc3amount));
                    }
                    else
                    {
                        suspectnpc3amount += moresuspiciousamount;
                    }
                    if (suspectnpc3amount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("(npc 3 namn) is the killer!!"));

                    }
                    else if (suspectnpc3amount > 1.5f)
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
                    if (likenpc3amount - (suspectnpc3amount + likeyouamount) > 0)
                    {
                        suspectyouamount += moresuspiciousamount * (1 + likenpc3amount - (likeyouamount + suspectnpc3amount));
                    }
                    else
                    {
                        suspectyouamount += moresuspiciousamount;
                    }
                    if (suspectyouamount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("..."));

                    }
                    else if (suspectyouamount > 1.5f)
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
                if (likeyouamount + suspectnpc4amount > suspectyouamount + likenpc4amount)
                {
                    if (likeyouamount - (suspectyouamount + likenpc4amount) > 0)
                    {
                        suspectnpc4amount += moresuspiciousamount * (1 + likeyouamount - (suspectyouamount + likenpc4amount));
                    }
                    else
                    {
                        suspectnpc4amount += moresuspiciousamount;
                    }
                    if (suspectnpc4amount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("(npc 4 namn) is the killer!!"));

                    }
                    else if (suspectnpc4amount > 1.5f)
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
                    if (likenpc4amount - (suspectnpc4amount + likeyouamount) > 0)
                    {
                        suspectyouamount += moresuspiciousamount * (1 + likenpc4amount - (likeyouamount + suspectnpc4amount));
                    }
                    else
                    {
                        suspectyouamount += moresuspiciousamount;
                    }
                    if (suspectyouamount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("..."));

                    }
                    else if (suspectyouamount > 1.5f)
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
                if (likeyouamount + suspectnpc5amount > suspectyouamount + likenpc5amount)
                {
                    if (likeyouamount - (suspectyouamount + likenpc5amount) > 0)
                    {
                        suspectnpc5amount += moresuspiciousamount * (1 + likeyouamount - (suspectyouamount + likenpc5amount));
                    }
                    else
                    {
                        suspectnpc5amount += moresuspiciousamount;
                    }
                    if (suspectnpc5amount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("(npc 5 namn) is the killer!!"));

                    }
                    else if (suspectnpc5amount > 1.5f)
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
                    if (likenpc5amount - (suspectnpc5amount + likeyouamount) > 0)
                    {
                        suspectyouamount += moresuspiciousamount * (1 + likenpc5amount - (likeyouamount + suspectnpc5amount));
                    }
                    else
                    {
                        suspectyouamount += moresuspiciousamount;
                    }
                    if (suspectyouamount > 3)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }

                        typingCoroutine = StartCoroutine(TypeSentence("..."));

                    }
                    else if (suspectyouamount > 1.5f)
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
    }
}