using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    // Start is called before the first frame update
    bool inventoryopen = false;
    bool caseFileOpen;
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
            if (money > 0)
            {
                moneyItem.gameObject.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.I) && inventoryopen == true)
        {
            inventoryopen = false;
            backpack.SetActive(false);
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
        }
    }
    public void openingCaseFile()
    {
        inventoryopen = false;
        backpack.SetActive(false);
        openCasefile.gameObject.SetActive(false);
        if (money > 0)
        {
            moneyItem.gameObject.SetActive(false);
        }

        caseFileOpen = true;
        casefile.SetActive(true);
        if (corpse > 0)
        {
            corpsevidence.gameObject.SetActive(true);
        }
    }
}
