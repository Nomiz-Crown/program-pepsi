using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;

public class evidence_code : MonoBehaviour
{
    // Start is called before the first frame update
    bool isPlayerInTrigger;
    public GameObject interactUi;
    ConvoHandler npc1convo;
    ConvoHandler npc2convo;
    ConvoHandler npc3convo;
    ConvoHandler npc4convo;
    inventory playerinventory;
    public GameObject player;
    public GameObject npc1;
    public GameObject npc2;
    public GameObject npc3;
    public GameObject npc4;
    public int evidenceOrItemType = 0;
    public GameObject ratpoison;
    public GameObject gregerUi;
    public Text dialogueText;
    private Coroutine typingCoroutine;
    [TextArea(3, 10)]
    public string text = "e";
    public bool consume;

    void Start()
    {
        if (player != null)
        {
            playerinventory = player.GetComponent<inventory>();
        }
        if (npc1 != null)
        {
            npc1convo = npc1.GetComponent<ConvoHandler>();
            npc2convo = npc2.GetComponent<ConvoHandler>();
            npc3convo = npc3.GetComponent<ConvoHandler>();
            npc4convo = npc4.GetComponent<ConvoHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E) && typingCoroutine == null)
        {
            
            if (interactUi != null)
            {
                interactUi.SetActive(false);
            }
            if (evidenceOrItemType == 1)
            {
                playerinventory.corpse = 1;
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 2)
            {
                playerinventory.money += 1;
                evidenceOrItemType = 0;
                transform.position = new Vector3(transform.position.x,transform.position.y, 2);
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 3)
            {
                playerinventory.ratpoison = 1;
                Instantiate(ratpoison, transform.position, Quaternion.identity);
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 4)
            {
                playerinventory.ratpoisonbottle += 1;
                evidenceOrItemType = 0;
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 5)
            {
                playerinventory.rats = 1;
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 6)
            {
                playerinventory.ventilation = 1;
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 7)
            {
                playerinventory.corpse2 = 1;
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (evidenceOrItemType != 0)
        {
            if (collision.CompareTag("Player"))
            {
                isPlayerInTrigger = true;
                if (interactUi != null)
                {
                    interactUi.SetActive(true);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            if (interactUi != null)
            {
                interactUi.SetActive(false);
            }
            gregerUi.SetActive(false);
            if (typingCoroutine != null)
            {
                if (consume)
                {
                    Destroy(gameObject);
                }
            }
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = null;
        }
    }

    private IEnumerator TypeSentence()
    {
        KeyCode[] ignoredKeys =
        {
            KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D,
            KeyCode.RightArrow, KeyCode.LeftArrow,
            KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.I
        };
        dialogueText.text = ""; // Clear the text before typing
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(0.05f); // Wait for the next letter
        }

        while (!Input.anyKeyDown || ignoredKeys.Any(Input.GetKey))
        {
            yield return null; // Wait for the next frame
        }
        gregerUi.SetActive(false);
        if (consume)
        {
            Destroy(gameObject);
        }
        typingCoroutine = null;
    }
}
