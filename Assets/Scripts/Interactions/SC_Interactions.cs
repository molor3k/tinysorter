using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_Interactions : MonoBehaviour
{
    public GameObject messagePanel;
    
    private TMP_Text messagePanelText;

    // Tools
    public bool hands = true;
    public bool wastePicker;
    public bool rake;

    // Recycling
    public bool startRecycle;

    private SC_Inventory inventory;
    private SC_Item mItemToPickup = null;

    void Start()
    {
        AllSet();
    }

    void Update()
    {
        CheckInteraction();
    }

    private void AllSet()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Inventory>();
        messagePanelText = messagePanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag) 
        {
            case "Item":
                SC_Item item = other.GetComponent<SC_Item>();

                if(item != null)
                {
                    if(hands == true && wastePicker == false && rake == false && ((item.type == "Paper") || (item.type == "Plastic") || (item.type == "Organic")))
                    {
                        OpenMessagePanel("");
                        mItemToPickup = item;

                    } else if(wastePicker == true && hands == false && rake == false && ((item.type == "Paper") || (item.type == "Plastic") || (item.type == "Metal")))
                    {
                        OpenMessagePanel("");
                        mItemToPickup = item;

                    } else if(rake == true && hands == false && wastePicker == false && ((item.type == "Plastic") || (item.type == "Metal") || (item.type == "Organic")))
                    {
                        OpenMessagePanel("");
                        mItemToPickup = item;
                    }
                }
            break;

            case "Can":
                OpenMessagePanel("");
                startRecycle = true;
            break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch(other.tag)
        {
            case "Item":
                SC_Item item = other.GetComponent<SC_Item>();

                if(item != null)
                {
                    CloseMessagePanel();
                    mItemToPickup = null;
                }
            break;

            case "Can":
                CloseMessagePanel();
                startRecycle = false;
            break;
        }
    }

    private void CheckInteraction() 
    {
        if (Input.GetButton("ButtonAction")) 
        {
            if (mItemToPickup != null && mItemToPickup.isPickedUp != true) 
            {
                inventory.AddItem(mItemToPickup);
                CloseMessagePanel();
                mItemToPickup = null;

            } 
        } /*else if(Input.GetButton("Tool1"))
        {
            inventory.TransferWastePicker();

        } else if(Input.GetButton("Tool2"))
        {
            inventory.TransferRake();
        }*/
    }

    // Message panel
    private void OpenMessagePanel(string text)
    {
        messagePanel.SetActive(true);
    }

    private void CloseMessagePanel()
    {
        messagePanel.SetActive(false);
    }
}