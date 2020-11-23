using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_ItemTypes;

public class SC_Item : MonoBehaviour
{
    public int ID;
    public string itemName;

    public Sprite icon;
    public ItemType type;

    public bool isPickedUp;

    private SC_Inventory inventory;

    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>();
    }

    public void PickItem() {
        gameObject.SetActive(false);
        isPickedUp = true;
        inventory.pointsForSorting++;
        Debug.Log("Item picked up, points: " + inventory.pointsForSorting);
    }

    public void DropItem() {
        gameObject.SetActive(true);
        isPickedUp = false;
        inventory.pointsForSorting--;
        Debug.Log("Item dropped, points: " + inventory.pointsForSorting);
    }
}
