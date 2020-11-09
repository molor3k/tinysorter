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
        CheckInteraction();
    }

    private void AllSet()
    {
        // Use SC_Inventory values, functions, .. in this class
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag) {
            case "Item":
                SC_Item item = other.GetComponent<SC_Item>();

                inventory.OpenMessagePanel("");
                mItemToPickup = item;
            break;

            case "Can":
                Debug.Log("You are colliding with Can");
            break;
        }
        if (other.gameObject.CompareTag("Item")) {
            SC_Item item = other.GetComponent<SC_Item>();

            inventory.OpenMessagePanel("");
            mItemToPickup = item;
        } else if (other.gameObject.CompareTag("Can")) {
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

    private void CheckInteraction() {
        if (Input.GetButton("ButtonAction")) {
            if (mItemToPickup != null && mItemToPickup.isPickedUp != true) {
                inventory.AddItem(mItemToPickup);
                inventory.CloseMessagePanel();
                mItemToPickup = null;
            }
        }
    }
}