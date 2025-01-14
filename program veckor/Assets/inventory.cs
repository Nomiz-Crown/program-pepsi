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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && inventoryopen == false && caseFileOpen == false)
        {
            inventoryopen = true;
            backpack.SetActive(true);
            openCasefile.gameObject.SetActive(true);
            page = 0;
        }
        else if (Input.GetKeyDown(KeyCode.I) && inventoryopen == true)
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
        }else if (Input.GetKeyDown(KeyCode.I) && inventoryopen == false)
        {
            caseFileOpen = false;
            casefile.SetActive(false);
            corpsevidence.gameObject.SetActive(false);
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
    public void useitem(int e)
    {
        if (e == 1)
        {
            money -= 1;
        }
    }
    public void openingCaseFile()
    {
        inventoryopen = false;
        backpack.SetActive(false);
        openCasefile.gameObject.SetActive(false);
        page = 0;
        if (money > 0)
        {
            moneyItem.gameObject.SetActive(false);
        }

        caseFileOpen = true;
        casefile.SetActive(true);
        nextPageInventory.gameObject.SetActive(false);
    }
}
