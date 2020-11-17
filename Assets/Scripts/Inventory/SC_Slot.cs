using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class SC_Slot : MonoBehaviour
{
    private GameObject itemObject;
    
    public Image itemIcon;

    public TMP_Text stackCounter;

    public int itemID;
    public string itemType;
    private string itemName;

    public int numberOfItems;      //Number of stack items

    public void AddItem(SC_Item item)
    {
        stackCounter = gameObject.transform.Find("Counter").GetChild(0).GetComponent<TMP_Text>();
        itemIcon = gameObject.transform.Find("ItemButton").GetChild(0).GetComponent<Image>();

        if (numberOfItems == 0)
        {
            itemID = item.ID;
            itemType = item.type;
            itemName = item.description;

            itemObject = item.itemObject;
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;

            item.isPickedUp = true;
            item.gameObject.SetActive(false);
        }
        numberOfItems++;
        stackCounter.text = numberOfItems.ToString();
    }

    public void DropItem()
    {
        // TODO: instantiate near Player
    }
}