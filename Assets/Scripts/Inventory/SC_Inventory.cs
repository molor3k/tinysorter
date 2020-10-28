using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_Inventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject messagePanel;
    public GameObject slotHolder;

    private GameObject[] slot;

    private bool inventoryEnabled;
    private int allSlots;


    void Start()
    {
        allSlots = 21; //Set all slots to 21
        slot = new GameObject[allSlots]; //Set slot as array with 21 indexes

        //Set slot holder object to slot index
        for(int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;
 
            //Check if slot index is null, if true then set empty to true
            if(slot[i].GetComponent<SC_Slot>().item == null) 
            {
                slot[i].GetComponent<SC_Slot>().empty = true;
            }
        }

        CloseMessagePanel(); //Call function which close the message panel
    }

    void Update()
    {
        //Set bool value to inventoryEnabled if click I
        if (Input.GetKeyDown(KeyCode.I))
            inventoryEnabled = !inventoryEnabled;

        //Open/close inventory if true/false
        if(inventoryEnabled == true)
        {
            inventory.SetActive(true);
        } else
        {
            inventory.SetActive(false);
        }
    }

    public void OpenMessagePanel(string text)
    {
        messagePanel.SetActive(true); //Open message panel
    }

    public void CloseMessagePanel()
    {
        messagePanel.SetActive(false); //Close message panel
    }

    public void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)
    {
        //Cycle which going trough all slots
        for(int i = 0; i < allSlots; i++)
        {
            //Check empty slots if true then move item to slot holder with his parameters (icon, ID, type, ...)
            if(slot[i].GetComponent<SC_Slot>().empty)
            {
                slot[i].GetComponent<SC_Slot>().item = itemObject;
                slot[i].GetComponent<SC_Slot>().icon = itemIcon;
                slot[i].GetComponent<SC_Slot>().ID = itemID;
                slot[i].GetComponent<SC_Slot>().type = itemType;
                slot[i].GetComponent<SC_Slot>().description = itemDescription;
                slot[i].GetComponent<SC_Slot>().empty = false;
                slot[i].GetComponent<SC_Slot>().UpdateSlot(); //Update slot image to item image

                itemObject.GetComponent<SC_Item>().isPickedUp = true; //Set item picked up value to true

                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false); //Change visibility of picked up item to non-visible

                return;
            }
        }
    }
}
