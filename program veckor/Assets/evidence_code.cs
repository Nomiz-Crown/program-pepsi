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
    inventory playerinventory;
    public GameObject player;
    public int evidenceOrItemType = 0;
    public GameObject spawnthing;
    public GameObject gregerUi;
    public Text dialogueText;
    private Coroutine typingCoroutine;
    [TextArea(3, 10)]
    public string text = "e";
    public bool spawnsomething;
    public bool consume;
    static int whatisclosest = 0;
    static int queue = 0;

    void Start()
    {
        if (player != null)
        {
            playerinventory = player.GetComponent<inventory>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInTrigger && whatisclosest == evidenceOrItemType)
        {
            if (interactUi != null)
            {
                interactUi.SetActive(true);
            }
        }
        else
        {
            if (interactUi != null)
            {
                interactUi.SetActive(false);
            }
        }
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E) && typingCoroutine == null && whatisclosest == evidenceOrItemType)
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
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 4)
            {
                playerinventory.ratpoisonbottle += 1;
                transform.position = new Vector3(transform.position.x, transform.position.y, 2);
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 5)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 2);
                playerinventory.rats = 1;
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 6)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 2);
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
            if (evidenceOrItemType == 8)
            {
                playerinventory.watch = 1;
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 9)
            {
                playerinventory.bloood = 1;
                if (gregerUi != null)
                {
                    gregerUi.SetActive(true);
                    typingCoroutine = StartCoroutine(TypeSentence());
                }
            }
            if (evidenceOrItemType == 10)
            {
                playerinventory.trapdoor = true;
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
                queue = whatisclosest;
                whatisclosest = evidenceOrItemType;
                isPlayerInTrigger = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (whatisclosest == evidenceOrItemType)
            {
                whatisclosest = queue;
            }
            isPlayerInTrigger = false;
            gregerUi.SetActive(false);
            if (typingCoroutine != null)
            {
                if (consume)
                {
                    if (spawnsomething)
                    {
                        spawnthing.SetActive(true);
                        spawnthing.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                    }
                    if (interactUi != null)
                    {
                        interactUi.SetActive(false);
                    }
                    gameObject.SetActive(false);
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
            if (spawnsomething)
            {
                spawnthing.SetActive(true);
            }
            if (interactUi != null)
            {
                interactUi.SetActive(false);
            }
            gameObject.SetActive(false);
        }
        typingCoroutine = null;
    }
}
