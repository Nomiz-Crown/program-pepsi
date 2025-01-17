using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    // Start is called before the first frame update
    bool inventoryopen = false;
    int page = 0;
    bool caseFileOpen;
    public Button nextPageInventory;
    public Button nextPageCaseFile;
    public GameObject backpack;
    public GameObject casefile;
    public Button openCasefile;
    public int money = 0;
    public Button moneyItem;
    public int corpse = 0;
    public Button corpsevidence;
    public int corpse2 = 0;
    public Button corpsevidence2;
    public int rats;
    public Button ratsevidence;
    public int ratpoison;
    public Button ratpoisonevidence;
    public int ratpoisonbottle;
    public Button ratpoisonitem;
    public int ventilation;
    public Button ventilationevidence;
    public int watch;
    public Button watchevidence;
    int blackout = 0;
    public GameObject cantsee;
    public GameObject corpse3;
    public GameObject Watch;
    public GameObject npc1;
    public GameObject npc2;
    public GameObject npc3;
    public GameObject npc4;
    ConvoHandler npc1convo;
    ConvoHandler npc2convo;
    ConvoHandler npc4convo;
    void Start()
    {
        npc1convo = npc1.GetComponent<ConvoHandler>();
        npc2convo = npc2.GetComponent<ConvoHandler>();
        npc4convo = npc4.GetComponent<ConvoHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rats == 1 && corpse == 1 && ventilation == 1 && blackout == 0 && (transform.position.y > 2 || transform.position.y < -12)) // det här behöver finnas i case files innan strömavbrott
        {
            blackout = 1;
            Instantiate(cantsee, new Vector3(0, 0, -6), Quaternion.identity); // placerar en fyrkant framför hela kartan 
            corpse3.SetActive(true);
            Watch.SetActive(true);
            npc1.transform.position = new Vector2(9.5f, -3); // flyttar karaktärerna 
            npc2.transform.position = new Vector2(12 , 0);
            npc3.transform.position = new Vector2(-100, -100);
            npc4.transform.position = new Vector2(9.5f, 1);
            npc1convo.poweroutage = 1; // npc bytter dialog
            npc1convo.currentportrait = 9;
            npc2convo.poweroutage = 1;
            npc2convo.currentportrait = 7;
            npc4convo.poweroutage = 1;
        }
        if (Input.GetKeyDown(KeyCode.I) && inventoryopen == false && caseFileOpen == false) // öppnar inventory om inventory och case file är stängt
        {
            inventoryopen = true; 
            backpack.SetActive(true);
            openCasefile.gameObject.SetActive(true);
            page = 0;
        }
        else if (Input.GetKeyDown(KeyCode.I) && inventoryopen == true) // stänger inventory om inventory är öppet 
        {
            inventoryopen = false; 
            backpack.SetActive(false);
            page = 0;
            nextPageInventory.gameObject.SetActive(false);
            openCasefile.gameObject.SetActive(false);
            if (money > 0)
            {
                moneyItem.gameObject.SetActive(false);
            }
            ratpoisonitem.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.I) && inventoryopen == false) // stänger case file om inventory är stängt och case file är öppet
        {
            caseFileOpen = false;
            casefile.SetActive(false);
            corpsevidence.gameObject.SetActive(false);
            corpsevidence2.gameObject.SetActive(false);
            ratpoisonevidence.gameObject.SetActive(false);
            ratsevidence.gameObject.SetActive(false);
            ventilationevidence.gameObject.SetActive(false);
            watchevidence.gameObject.SetActive(false);
            page = 0;
            nextPageCaseFile.gameObject.SetActive(false);
        }
        
        if (caseFileOpen == true)
        {
            int position = 80;
            if (corpse > 0 && position + (page * 150) <= 80 && position + (page * 150) >= -40) 
            {
                corpsevidence.gameObject.SetActive(true);
                corpsevidence.GetComponent<RectTransform>().anchoredPosition = new Vector2(-284f, position + page * 150);
                position -= 30;
            }
            else
            {
                corpsevidence.gameObject.SetActive(false);
            }
            if (corpse2 > 0 && position + (page * 150) <= 80 && position + (page * 150) >= -40)
            {
                corpsevidence2.gameObject.SetActive(true);
                corpsevidence2.GetComponent<RectTransform>().anchoredPosition = new Vector2(-284f, position + page * 150);
                position -= 30;
            }
            else
            {
                corpsevidence2.gameObject.SetActive(false);
            }
            if (rats > 0 && position + (page * 150) <= 80 && position + (page * 150) >= -40)
            {
                ratsevidence.gameObject.SetActive(true);
                ratsevidence.GetComponent<RectTransform>().anchoredPosition = new Vector2(-284f, position + page * 150);
                position -= 30;
            }
            else
            {
                ratsevidence.gameObject.SetActive(false);
            }
            if (ratpoison > 0 && position + (page * 150) <= 80 && position + (page * 150) >= -40)
            {
                ratpoisonevidence.gameObject.SetActive(true);
                ratpoisonevidence.GetComponent<RectTransform>().anchoredPosition = new Vector2(-284f, position + page * 150);
                position -= 30;
            }
            else
            {
                ratpoisonevidence.gameObject.SetActive(false);
            }
            if (ventilation > 0 && position + (page * 150) <= 80 && position + (page * 150) >= -40)
            {
                ventilationevidence.gameObject.SetActive(true);
                ventilationevidence.GetComponent<RectTransform>().anchoredPosition = new Vector2(-284f, position + page * 150);
                position -= 30;
            }
            else
            {
                ventilationevidence.gameObject.SetActive(false);
            }
            if (watch > 0 && position + (page * 150) <= 80 && position + (page * 150) >= -40)
            {
                watchevidence.gameObject.SetActive(true);
                watchevidence.GetComponent<RectTransform>().anchoredPosition = new Vector2(-284f, position + page * 150);
                position -= 30;
            }
            else
            {
                watchevidence.gameObject.SetActive(false);
            }
            if (position < -40) 
            {
                nextPageCaseFile.gameObject.SetActive(true);
            }
        }
        if (inventoryopen == true)
        {
            int position = 55;
            if (money > 0 && position + (page * 120) <= 55 && position + (page * 120) >= -35)
            {
                moneyItem.gameObject.SetActive(true);
                moneyItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(-284f, position + page * 120);
                position -= 30;
            }
            else
            {
                moneyItem.gameObject.SetActive(false);
            }
            if (ratpoisonbottle > 0 && position + (page * 120) <= 55 && position + (page * 120) >= -35)
            {
                ratpoisonitem.gameObject.SetActive(true);
                ratpoisonitem.GetComponent<RectTransform>().anchoredPosition = new Vector2(-284f, position + page * 120);
                position -= 30;
            }
            else
            {
                ratpoisonitem.gameObject.SetActive(false);
            }
            if (position < -35)
            {
                nextPageInventory.gameObject.SetActive(true);
            }
        }
    }
    public void nextpage()
    {
        page += 1;
    }
    public void useitem(int witchitem)
    {
        if (witchitem == 1)
        {
            money -= 1;
        }
        if (witchitem == 2)
        {
            ratpoisonbottle -= 1;
        }
    }
    public void openingCaseFile()
    {
        inventoryopen = false; // öppnar case file
        backpack.SetActive(false);
        openCasefile.gameObject.SetActive(false);
        page = 0;
        ratpoisonitem.gameObject.SetActive(false);
        moneyItem.gameObject.SetActive(false);

        caseFileOpen = true;
        casefile.SetActive(true);
        nextPageInventory.gameObject.SetActive(false);
    }
}
