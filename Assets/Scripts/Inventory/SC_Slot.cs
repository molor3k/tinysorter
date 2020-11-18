using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using static SC_ItemTypes;

public class SC_Slot : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text stackCounter;
    public int slotID = -1;
    public int itemID;
    
    public List<GameObject> itemObjects;
    private ItemType itemType;
    private string itemName;

    private GameObject playerObject;
    private SC_EnvGrid environmentGrid;


    void Start() {
        playerObject = GameObject.Find("Player");
        environmentGrid = GameObject.Find("Env").GetComponent<SC_EnvGrid>();
    }

    public void AddItem(SC_Item item)
    {
        stackCounter = gameObject.transform.Find("Counter").GetChild(0).GetComponent<TMP_Text>();
        itemIcon = gameObject.transform.Find("ItemButton").GetChild(0).GetComponent<Image>();

        if (itemObjects.Count == 0) {
            itemID = item.ID;
            itemType = item.type;
            itemName = item.itemName;

            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;
        }

        itemObjects.Add(item.gameObject);
        item.PickItem();

        stackCounter.text = itemObjects.Count.ToString();
    }

    public void DropItem()
    {
        GameObject itemObject = itemObjects[itemObjects.Count - 1];
        itemObject.GetComponent<SC_EnvObject>().DropItemOnFreeCell();

        itemObjects.RemoveAt(itemObjects.Count - 1);
        stackCounter.text = itemObjects.Count.ToString();

        if (itemObjects.Count == 0) {
            itemID = -1;
            itemType = ItemType.None;
            itemName = "";

            itemIcon.sprite = null;
            itemIcon.enabled = false;

            stackCounter.text = "";
        }
    }
}