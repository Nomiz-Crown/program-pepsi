using System.Collections;
using System.Collections.Generic;
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
    ConvoHandler npc5convo;
    inventory playerinventory;
    public GameObject player;
    public GameObject npc1;
    public GameObject npc2;
    public GameObject npc3;
    public GameObject npc4;
    public GameObject npc5;
    public int evidenceOrItemType = 0;

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
            npc5convo = npc5.GetComponent<ConvoHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (interactUi != null)
            {
                interactUi.SetActive(false);
            }
            if (evidenceOrItemType == 1)
            {
                npc1convo.corpsevidence = 1;
                npc2convo.corpsevidence = 1;
                npc3convo.corpsevidence = 1;
                npc4convo.corpsevidence = 1;
                npc5convo.corpsevidence = 1;
                playerinventory.corpse += 1;
                evidenceOrItemType = 0;
            }
            if (evidenceOrItemType == 2)
            {
                playerinventory.money += 1;
                Destroy(gameObject);
            }
            if (evidenceOrItemType == 3)
            {
                npc1convo.ratpoisonevidence = 1;
                npc2convo.ratpoisonevidence = 1;
                npc3convo.ratpoisonevidence = 1;
                npc4convo.ratpoisonevidence = 1;
                npc5convo.ratpoisonevidence = 1;
                playerinventory.ratpoison += 1;
                evidenceOrItemType = 4;
            }
            if (evidenceOrItemType == 4)
            {
                playerinventory.ratpoisonbottle += 1;
                Destroy(gameObject);
            }
            if (evidenceOrItemType == 5)
            {
                npc1convo.ratholevidence = 1;
                npc2convo.ratholevidence = 1;
                npc3convo.ratholevidence = 1;
                npc4convo.ratholevidence = 1;
                npc5convo.ratholevidence = 1;
                playerinventory.rats = 1;
                evidenceOrItemType = 0;
            }
            if (evidenceOrItemType == 6)
            {
                playerinventory.ventilation = 1;
                evidenceOrItemType = 0;
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
        }
    }
}
