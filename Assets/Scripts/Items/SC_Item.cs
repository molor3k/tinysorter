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
    

    public void PickItem() {
        gameObject.SetActive(false);
        isPickedUp = true;
    }

    public void DropItem() {
        gameObject.SetActive(true);
        isPickedUp = false;
    }
}
