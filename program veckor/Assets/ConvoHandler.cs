using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class ConvoHandler : MonoBehaviour
{
    [SerializeField] private string titletext; // Set this in the Inspector
    [SerializeField] private Text title; // Assign the Text UI component in Inspector
    public GameObject gregerUi;       // The main UI that activates with E
    public GameObject interactUi;     // "Press E to Interact" UI
    public Text dialogueText;         // Text component for dialogue
    public GameObject caseFile;
    public Button button1;            // First button for primary dialogue
    public Button button2;            // Second button for alternate dialogue
    public Button part2button1;       
    public Button part2button2;
    public Button otherButtons1;
    public Button otherButtons2;
    public Button npc1button;
    public Button npc2button;
    public Button npc3button;
    public Button npc4button;
    public Button corpsevidencebutton;

    public int ratholevidence = 0;
    public int ratpoisonevidence = 0;
    public int corpsevidence = 0;

    public int poweroutage;

    public int npcroom;
    public int witchNpcIsThis;
    static int playeroom;
    static int whoistalking;

    [TextArea(3, 10)]
    public string startingMessage;    // Message displayed before paths
    [TextArea(3, 10)]
    public string part2startingMessage;    // Message displayed before paths
    [TextArea(3, 10)]
    public List <List<string>> dialogueLines; // Dialogue path 1
    public List <List<int>> dialougPortraits;
    public List <GameObject> dialougeNextButtons;
    public List<GameObject> dialougeNextButtons2;



    [TextArea(3, 10)]
    public List<string> dialogueLines1 = new List<string>(); // Dialogue path 1
    public List <int> dialoug1Portraits = new List<int>();
    public int dialouge1NextButtons = 0;
    [TextArea(3, 10)]
    public List<string> dialogueLines2 = new List<string>(); // Dialogue path 2
    public List<int> dialouge2Portraits = new List<int>();
    public int dialouge2NextButtons = 0;
    [TextArea(3, 10)]
    public List<string> part2DialogueLines1 = new List<string>(); // Dialogue path 1 post power outage
    public List<int> part2Dialouge1Portraits = new List<int>();
    public int part2Dialouge1NextButtons = 0;
    [TextArea(3, 10)]
    public List<string> part2DialogueLines2 = new List<string>(); // Dialogue path 2 post power outage
    public List<int> part2Dialouge2Portraits = new List<int>();
    public int part2Dialouge2NextButtons = 0;
    public float typingSpeed = 0.05f; // Time delay between letters

    private int currentLineIndex = 0; // Tracks the current dialogue line
    GameObject actvienextbuttons;
    GameObject actvienextbuttons2;
    List<int> activeportrait;
    private List<string> activeDialogue; // Holds the current dialogue path
    private Coroutine typingCoroutine; // Tracks the typing coroutine
    private bool isPlayerInTrigger = false;
    private bool showingStartingMessage = true; // Tracks if we’re in the starting phase
    float evidence;
    static string npc1says;
    static string npc2says;
    static string npc3says;
    static string npc4says;
    static string npc5says;
    public int currentportrait = 1;
    static int npc1portrait;
    static int npc2portrait;
    static int npc3portrait;
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
    public List<GameObject> portraits;
    static int whoisclosest = 0;
    static int queue = 0;
    public GameObject player;
    inventory playerinventory;
    private void Start()
    {
        playerinventory = player.GetComponent<inventory>();
        title.text = titletext;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            queue = whoisclosest;
            whoisclosest = witchNpcIsThis;
            playeroom = npcroom;   
            isPlayerInTrigger = true;

            // Show the interaction UI when player enters the trigger

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (whoisclosest == witchNpcIsThis)
            {
                whoisclosest = queue;
            }
            isPlayerInTrigger = false;
            // Hide both UIs and reset dialogue state
            if (whoistalking == witchNpcIsThis)
            {
                whoistalking = 0;
            }
            if (gregerUi != null)
            {
                gregerUi.SetActive(false);
            }
            npcbuttons(false);
            removeportrait();
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
        if (isPlayerInTrigger && whoisclosest == witchNpcIsThis)
        {
            if (interactUi != null)
            {
                interactUi.SetActive(true);
            }
        }
        else
        {
            interactUi.SetActive(false);
        }
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E) && whoisclosest == witchNpcIsThis && whoistalking == 0)
        {
            // Hide interact UI and show greger UI
            whoistalking = witchNpcIsThis;
            dialogueText.text = "";
            if (interactUi != null)
            {
                interactUi.SetActive(false);
            }

            if (gregerUi != null)
            {
                gregerUi.SetActive(true);
                title.text = titletext;

                if (showingStartingMessage)
                {
                    // Display the starting message
                    ShowButtons(false, false); // Hide buttons during starting message
                    if (typingCoroutine != null)
                    {
                        StopCoroutine(typingCoroutine);
                    }
                    if (poweroutage == 0)
                    {
                        typingCoroutine = StartCoroutine(TypeSentence(startingMessage));
                    }
                    else if (poweroutage == 1)
                    {
                        typingCoroutine = StartCoroutine(TypeSentence(part2startingMessage));
                    }
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
        if (activeportrait != null)
        {
            currentportrait = activeportrait[0];
        }
        removeportrait();
        typingCoroutine = StartCoroutine(TypeSentence(activeDialogue[currentLineIndex]));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        KeyCode[] ignoredKeys = {

            KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D,
    KeyCode.RightArrow, KeyCode.LeftArrow,
               KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.I
             };
        if (whoistalking != witchNpcIsThis) // om en npc inte är den som pratar så blir vad den säger till en statci variable.
        {
            if (witchNpcIsThis == 1)
            {
                npc1says = sentence;
                npc1portrait = currentportrait;
            }
            else if (witchNpcIsThis == 2)
            {
                npc2says = sentence;
                npc2portrait = currentportrait;
            }
            else if (witchNpcIsThis == 3)
            {
                npc3says = sentence;
                npc3portrait = currentportrait;
            }
            else if (witchNpcIsThis == 4)
            {
                npc4says = sentence + "  (npc 4)";
            }
            else if (witchNpcIsThis == 5)
            {
                npc5says = sentence + "  (npc 5)";
            }
        }
        else
        {
            if (portraits[currentportrait] != null)
            {
                portraits[currentportrait].SetActive(true);
            }
        }
        dialogueText.text = ""; // Clear the text before typing
         foreach (char letter in sentence.ToCharArray())
       {
        dialogueText.text += letter; // Add one letter at a time
        yield return new WaitForSeconds(typingSpeed); // Wait for the next letter
       }

        if (!showingStartingMessage)
        {
            while (!Input.anyKeyDown || ignoredKeys.Any(Input.GetKey))
            {
                yield return null; // Wait for the next frame
            }
        }

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
            if (activeDialogue != null)
            {
                if (currentLineIndex + 1 < activeDialogue.Count)
                {
                    removeportrait();
                    currentLineIndex++;
                    if (activeportrait != null && currentLineIndex < activeportrait.Count)
                    {
                        currentportrait = activeportrait[currentLineIndex];
                    }
                    if (currentLineIndex < activeDialogue.Count)
                    {
                        if (typingCoroutine != null)
                        {
                            StopCoroutine(typingCoroutine);
                        }
                        typingCoroutine = StartCoroutine(TypeSentence(activeDialogue[currentLineIndex]));
                    }
                }
                else
                {
                    if (actvienextbuttons == null)
                    {
                        gregerUi.SetActive(false); // Hide the UI
                        showingStartingMessage = true;
                        whoistalking = 0;

                    }
                    else
                    {
                        actvienextbuttons.SetActive(true);
                        actvienextbuttons2.SetActive(true);
                    }
                    ShowButtons(false, false); // No buttons if dialogue is finished
                    currentLineIndex = 0;
                    activeDialogue = null;
                    removeportrait();
                }
            }
            else if (npc1says != null || npc2says != null || npc3says != null || npc4says != null || npc5says != null) // den npc som pratar säger vad dom andar npc skulle säga
            {
                removeportrait();
                if (npc1says != null)
                {
                    typingCoroutine = StartCoroutine(TypeSentence(npc1says));
                    currentportrait = npc1portrait;
                    npc1says = null;
                    
                }
                else if (npc2says != null)
                {
                    typingCoroutine = StartCoroutine(TypeSentence(npc2says));
                    npc2says = null;
                    currentportrait = npc2portrait;
                }
                else if (npc3says != null)
                {
                    typingCoroutine = StartCoroutine(TypeSentence(npc3says));
                    currentportrait = npc3portrait;
                    npc3says = null;
                }
                else if (npc4says != null)
                {
                    typingCoroutine = StartCoroutine(TypeSentence(npc4says));
                    npc4says = null;
                }
                else if (npc5says != null)
                {
                    typingCoroutine = StartCoroutine(TypeSentence(npc5says));
                    npc5says = null;
                }
            }
            else
            {
                ShowButtons(false, false); // No buttons if dialogue is finished
                gregerUi.SetActive(false); // Hide the UI
                currentLineIndex = 0;
                showingStartingMessage = true;
                activeDialogue = null;
                removeportrait();
                whoistalking = 0;
            }
       }
    }
    

    private void ShowButtons(bool showButton1, bool showButton2)
    {
        if (button1 != null && poweroutage == 0)
        {
            button1.gameObject.SetActive(showButton1);
        }else if (part2button1 != null)
        {
            part2button1.gameObject.SetActive(showButton1);
        }

        if (button2 != null && poweroutage == 0)
        {
            button2.gameObject.SetActive(showButton2);
        }
        else if (part2button2 != null)
        {
            part2button2.gameObject.SetActive(showButton2);
        }
    }

    // Button 1 click event to proceed to the next line of dialogue or start path 1
    public void OnButtonClick(int witchbutton)
    {
        if (witchbutton == 1)
        {
            if (dialoug1Portraits != null)
            {
                activeportrait = dialoug1Portraits;
            }
            if (!showingStartingMessage && activeDialogue == null)
            {
                StartDialogue(dialogueLines1); // Start path 1 dialogue
            }
        }
        if (witchbutton == 2)
        {
            if (dialouge2Portraits != null)
            {
                activeportrait = dialouge2Portraits;
            }
            if (!showingStartingMessage && activeDialogue == null)
            {
                StartDialogue(dialogueLines2); // Start path 1 dialogue
            }
        }
        if (witchbutton == 3)
        {
            if (part2Dialouge1Portraits != null)
            {
                activeportrait = part2Dialouge1Portraits;
            }
            if (!showingStartingMessage && activeDialogue == null)
            {
                StartDialogue(part2DialogueLines1); // Start path 1 dialogue
            }
        }
        if (witchbutton == 4)
        {
            if (part2Dialouge2Portraits != null)
            {
                activeportrait = part2Dialouge2Portraits;
            }
            if (!showingStartingMessage && activeDialogue == null)
            {
                StartDialogue(part2DialogueLines2); // Start path 1 dialogue
            }
        }
    }
    void removeportrait()
    {
        int i = 0;
        while(i < portraits.Count)
        {
            if (portraits[i] != null)
            {
                portraits[i].SetActive(false);
            }
            i += 1;
        }
    }

    public void giveMoney()
    {
        if (witchNpcIsThis == whoistalking)
        {
            likeYouAmount += 0.5f;
            StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeSentence("thanks"));
            playerinventory.money -= 1;
        }
    }
    // Button 2 click event to start path 2 dialogue

    public void corpse(int evidencetype)
    {
        if (evidencetype == 1)
        {
            if (corpsevidence > 0)
            {
                evidence = 0.5f;
                corpsevidence = 0; // so npc can only hear the same evidence once
            }
        }
        else
        {
            if (ratpoisonevidence > 0)
            {
                evidence = 0.5f;
                ratpoisonevidence = 0; // so npc can only hear the same evidence once
            }
        }
        if (isPlayerInTrigger == true)
        {
            npcbuttons(true);
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
    void accusation(float moresuspiciousamount, int whoismentiond1, int whoistalikng)
    {
        npcbuttons(false);
        if (whoismentiond1 == witchNpcIsThis)
        {
            likeYouAmount -= 1.2f;
            suspectYouAmount += 0.3f;
            if (witchNpcIsThis == 1)
            {
                npc1portrait = 2;
            }
            if (witchNpcIsThis == 2)
            {
                npc2portrait = 1;
            }
            typingCoroutine = StartCoroutine(TypeSentence("how dare you acuse me"));
        }
        else if (whoistalikng == 0)
        {
            if (whoismentiond1 == 1)
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
                    writestuff(suspectYouAmount, "Sabrina Bellagamba");
                }
                else
                {
                    if (likeNpc1Amount - (suspectNpc1Amount + likeYouAmount) > 0)
                    {
                        likeYouAmount += moresuspiciousamount * (1 + likeNpc1Amount - (likeYouAmount + suspectNpc1Amount));
                    }
                    else
                    {
                        likeYouAmount += moresuspiciousamount;
                    }
                    writestuff(likeYouAmount, "you");
                }
            }
            else if (whoismentiond1 == 2)
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
                    writestuff(suspectYouAmount, "Giovane Donna");
                }
                else
                {
                   if (likeNpc2Amount - (suspectNpc2Amount + likeYouAmount) > 0)
                    {
                        likeYouAmount += moresuspiciousamount * (1 + likeNpc2Amount - (likeYouAmount + suspectNpc2Amount));
                    }
                    else
                    {
                        likeYouAmount += moresuspiciousamount;
                    }
                    writestuff(likeYouAmount, "you");
                }
            }
            else if (whoismentiond1 == 3)
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
                    writestuff(suspectYouAmount, "lucina");
                }
                else
                {
                    if (likeNpc3Amount - (suspectNpc3Amount + likeYouAmount) > 0)
                    {
                        likeYouAmount += moresuspiciousamount * (1 + likeNpc3Amount - (likeYouAmount + suspectNpc3Amount));
                    }
                    else
                    {
                        likeYouAmount += moresuspiciousamount;
                    }
                    writestuff(likeYouAmount, "you");
                }
            }
            else if (whoismentiond1 == 4)
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
                    writestuff(suspectYouAmount, "antonio");
                }
                else
                {
                    if (likeNpc4Amount - (suspectNpc4Amount + likeYouAmount) > 0)
                    {
                        likeYouAmount += moresuspiciousamount * (1 + likeNpc4Amount - (likeYouAmount + suspectNpc4Amount));
                    }
                    else
                    {
                        likeYouAmount += moresuspiciousamount;
                    }
                    writestuff(likeYouAmount, "you");
                }
            }
        }
        evidence = 0;
    }
    void writestuff(float suspecthing, string whoismentioned2)
    {
        if (whoismentioned2 == "you")
        {
            if (suspecthing > 3)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }

                typingCoroutine = StartCoroutine(TypeSentence("..."));

            }
            else if (suspecthing > 1.5f)
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
        else
        {
            if (suspectNpc4Amount > 3)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }

                typingCoroutine = StartCoroutine(TypeSentence(whoismentioned2 + " is the killer!!"));

            }
            else if (suspectNpc4Amount > 1.5f)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }

                typingCoroutine = StartCoroutine(TypeSentence("perhaps " + whoismentioned2 + " is the killer?"));
            }
            else
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }

                typingCoroutine = StartCoroutine(TypeSentence("why do you think " + whoismentioned2 + " is the killer?"));
            }
        }
    }
}