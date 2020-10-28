using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Item : MonoBehaviour
{
    public GameObject item;
    public Sprite icon;

    public int ID;
    public string type;
    public string description;
    public bool isPickedUp;

    private SC_Inventory inventory;

    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>(); //Find SC_Inventory for use values, functions, .. in this class
    }

    public void PickUp()
    {
        //Close message panel and move item to slot holder with his parameters (icon, ID, type, ...)
        if(!isPickedUp)
        {
            inventory.CloseMessagePanel();
            inventory.AddItem(item, ID, type, description, icon);
            
            //Debug.Log("Item picked up");
        }
    }
}
