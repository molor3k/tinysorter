using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Interactions : MonoBehaviour
{
    // Tools
    public bool hands = true;
    public bool wastePicker;
    public bool rake;

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
        // Use SC_Inventory values, functions, .. in this class
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Collider
        if(other.gameObject.CompareTag("Item"))
        {
            SC_Item item = other.GetComponent<SC_Item>();

            if(item != null)
            {
                if(hands == true && wastePicker == false && rake == false && ((item.type == "Paper") || (item.type == "Plastic") || (item.type == "Organic")))
                {
                    inventory.OpenMessagePanel("");
                    mItemToPickup = item;

                } else if(wastePicker == true && hands == false && rake == false && ((item.type == "Paper") || (item.type == "Plastic") || (item.type == "Metal")))
                {
                    inventory.OpenMessagePanel("");
                    mItemToPickup = item;
                } else if(rake == true && hands == false && wastePicker == false && ((item.type == "Plastic") || (item.type == "Metal") || (item.type == "Organic")))
                {
                    inventory.OpenMessagePanel("");
                    mItemToPickup = item;
                }

            }

        } else if(other.gameObject.CompareTag("Can"))
        {
            Debug.Log("You are colliding with Can");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Collider
        if(other.gameObject.CompareTag("Item"))
        {
            SC_Item item = other.GetComponent<SC_Item>();

            if(item != null)
            {
                inventory.CloseMessagePanel();
                mItemToPickup = null;
            }

        } else if(other.gameObject.CompareTag("Can"))
        {
            Debug.Log("You are no longer colliding with Can");
        }
    }

    private void FireInteraction()
    {
        // Fire interaction
        if(mItemToPickup != null && mItemToPickup.isPickedUp != true && Input.GetKeyDown(KeyCode.E))
        {
            inventory.AddItem(mItemToPickup);
            inventory.CloseMessagePanel();
            mItemToPickup = null;
        }
    }
}