using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC_Inventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject messagePanel;
    public GameObject slotHolder;
    
    private GameObject[] slot;

    public GameObject[] removeButton;
    private Button[] removeButtonInteraction;
    private Image[] removeButtonImage;
    
    private GameObject[] stackItems;
    private TMP_Text[] stackItemsCounter;

    private GameObject[] itemHolder;
    private RemoveButtonID removeButtonID;

    private bool inventoryEnabled;
    private int allSlots;
    private int[] counter;

    //
    public int[] testButtonArray;
    //

    void Start()
    {
        allSlots = 21; //Set all slots to 21

        itemHolder = new GameObject[allSlots];

        slot = new GameObject[allSlots]; //Set slot as array with 21 indexes

        removeButton = new GameObject[allSlots];
        removeButtonInteraction = new Button[allSlots];
        removeButtonImage = new Image[allSlots];

        stackItems = new GameObject[allSlots];
        stackItemsCounter = new TMP_Text[allSlots];

        removeButtonID = GameObject.Find("RemoveButton").GetComponent<RemoveButtonID>();

        counter = new int[allSlots];
        testButtonArray = new int[allSlots];

        //Set slot holder object to slot index
        for(int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            removeButton[i] = slotHolder.transform.GetChild(i).GetChild(1).gameObject;
            removeButtonInteraction[i] = removeButton[i].GetComponent<Button>();
            removeButtonImage[i] = removeButton[i].GetComponent<Image>();

            stackItems[i] = slotHolder.transform.GetChild(i).GetChild(2).GetChild(0).gameObject;
            stackItemsCounter[i] = stackItems[i].GetComponent<TMP_Text>();
 
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
            if(itemID == 1)
            {
                //Check empty slots if true then move item to slot holder with his parameters (icon, ID, type, ...)
                if (slot[i].GetComponent<SC_Slot>().ID == itemID)
                {
                    itemHolder[i] = itemObject;

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = true; //Set item picked up value to true

                    //itemHolder[i].transform.parent = slot[i].transform;
                    itemHolder[i].SetActive(false); //Change visibility of picked up item to non-visible

                    removeButtonInteraction[i].interactable = true;
                    removeButtonImage[i].enabled = true;

                    counter[i]++;
                    stackItemsCounter[i].text = counter[i].ToString();
                    
                    break;
                } else if(slot[i].GetComponent<SC_Slot>().empty && slot[i].GetComponent<SC_Slot>().ID != itemID)
                {     
                    itemHolder[i] = itemObject;

                    slot[i].GetComponent<SC_Slot>().item = itemHolder[i];
                    slot[i].GetComponent<SC_Slot>().icon = itemIcon;
                    slot[i].GetComponent<SC_Slot>().ID = itemID;
                    slot[i].GetComponent<SC_Slot>().type = itemType;
                    slot[i].GetComponent<SC_Slot>().description = itemDescription;
                    slot[i].GetComponent<SC_Slot>().empty = false;
                    slot[i].GetComponent<SC_Slot>().UpdateSlot(); //Update slot image to item image

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = true; //Set item picked up value to true

                    //itemHolder[i].transform.parent = slot[i].transform;
                    itemHolder[i].SetActive(false); //Change visibility of picked up item to non-visible

                    removeButtonInteraction[i].interactable = true;
                    removeButtonImage[i].enabled = true;

                    counter[i]++;
                    stackItemsCounter[i].text = counter[i].ToString();

                    break;
                }
            } else
            {
                //Check empty slots if true then move item to slot holder with his parameters (icon, ID, type, ...)
                if(slot[i].GetComponent<SC_Slot>().empty && slot[i].GetComponent<SC_Slot>().ID != itemID)
                {     
                    itemHolder[i] = itemObject;

                    slot[i].GetComponent<SC_Slot>().item = itemHolder[i];
                    slot[i].GetComponent<SC_Slot>().icon = itemIcon;
                    slot[i].GetComponent<SC_Slot>().ID = itemID;
                    slot[i].GetComponent<SC_Slot>().type = itemType;
                    slot[i].GetComponent<SC_Slot>().description = itemDescription;
                    slot[i].GetComponent<SC_Slot>().empty = false;
                    slot[i].GetComponent<SC_Slot>().UpdateSlot(); //Update slot image to item image

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = true; //Set item picked up value to true

                    //itemHolder[i].transform.parent = slot[i].transform;
                    itemHolder[i].SetActive(false); //Change visibility of picked up item to non-visible

                    removeButtonInteraction[i].interactable = true;
                    removeButtonImage[i].enabled = true;

                    break;
                }
            }
        }
    }

    public void DropItem(GameObject itemObject)
    {
        for(int i = 0; i < allSlots; i++)
        {
            if(!(slot[i].GetComponent<SC_Slot>().empty))
            {     
                slot[i].GetComponent<SC_Slot>().item = null;
                slot[i].GetComponent<SC_Slot>().icon = null;
                slot[i].GetComponent<SC_Slot>().ID = 0;
                slot[i].GetComponent<SC_Slot>().type = null;
                slot[i].GetComponent<SC_Slot>().description = null;
                slot[i].GetComponent<SC_Slot>().empty = true;
                slot[i].GetComponent<SC_Slot>().DropUpdateSlot();

                itemHolder[i].GetComponent<SC_Item>().isPickedUp = false;

                //Here should be transform of the object
                itemHolder[i].SetActive(true);

                removeButtonInteraction[i].interactable = false;
                removeButtonImage[i].enabled = false;

                counter[i]--;
                stackItemsCounter[i].text = counter[i].ToString();

                if(counter[i] <= 0)
                {
                    stackItemsCounter[i].text = "";
                }

                break;
            }
        }
    }
}
