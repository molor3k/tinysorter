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
        FireInteraction();
    }

    private void AllSet()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Inventory>();
        messagePanelText = messagePanel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        CloseMessagePanel();
    }

    // Colliders
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
        {
            SC_Item item = other.GetComponent<SC_Item>();

            if(item != null)
            {
                if(hands == true && wastePicker == false && rake == false && ((item.type == "Paper") || (item.type == "Plastic") || (item.type == "Organic")))
                {
                    OpenMessagePanel("- Press E to pick up -");
                    mItemToPickup = item;

                } else if(wastePicker == true && hands == false && rake == false && ((item.type == "Paper") || (item.type == "Plastic") || (item.type == "Metal")))
                {
                    OpenMessagePanel("- Press E to pick up -");
                    mItemToPickup = item;

                } else if(rake == true && hands == false && wastePicker == false && ((item.type == "Plastic") || (item.type == "Metal") || (item.type == "Organic")))
                {
                    OpenMessagePanel("- Press E to pick up -");
                    mItemToPickup = item;
                }
            }

        } else if(other.gameObject.CompareTag("Can"))
        {
            OpenMessagePanel("- Press E to start recycling -");
            startRecycle = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
        {
            SC_Item item = other.GetComponent<SC_Item>();

            if(item != null)
            {
                CloseMessagePanel();
                mItemToPickup = null;
            }

        } else if(other.gameObject.CompareTag("Can"))
        {
            CloseMessagePanel();
            startRecycle = false;
        }
    }

    // Fire interaction
    private void FireInteraction()
    {
        if(mItemToPickup != null && mItemToPickup.isPickedUp != true && Input.GetKeyDown(KeyCode.E))
        {
            inventory.AddItem(mItemToPickup);
            CloseMessagePanel();
            mItemToPickup = null;

        } else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventory.TransferWastePicker();

        } else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventory.TransferRake();
        }
    }

    // Message panel
    private void OpenMessagePanel(string text)
    {
        messagePanel.SetActive(true);
        messagePanelText.text = text;
    }

    private void CloseMessagePanel()
    {
        messagePanel.SetActive(false);
        messagePanelText.text = null;
    }

}