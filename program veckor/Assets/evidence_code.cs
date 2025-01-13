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
    public GameObject npc1;
    public GameObject npc2;
    public GameObject npc3;
    public GameObject npc4;
    public GameObject npc5;
    public int evidenctype = 1;

    void Start()
    {
        npc1convo = npc1.GetComponent<ConvoHandler>();
        npc2convo = npc2.GetComponent<ConvoHandler>();
        npc3convo = npc3.GetComponent<ConvoHandler>();
        npc4convo = npc4.GetComponent<ConvoHandler>();
        npc5convo = npc5.GetComponent<ConvoHandler>();
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
            if (evidenctype == 1)
            {
                npc1convo.corpsevidence = 1;
                npc2convo.corpsevidence = 1;
                npc3convo.corpsevidence = 1;
                npc4convo.corpsevidence = 1;
                npc5convo.corpsevidence = 1;
                evidenctype = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (evidenctype != 0)
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
