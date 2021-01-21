/*
MIT License

Copyright (c) 2021 IBPM Team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using static SC_ItemTypes;

public class SC_Slot : MonoBehaviour {

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

    public ItemType GetItemType() {
        return itemType;
    }

    public void AddItem(SC_Item item) {
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

    public bool DropItem(bool needToDrop, bool isDroppingStack) {
        int startValue = (itemObjects.Count - 1);
        int endValue = isDroppingStack ? 0 : (itemObjects.Count - 1);

        bool haveDroppedAnyItem = false;

        for(var i = startValue; i >= endValue; i--) {
            if (needToDrop) {
                if (itemObjects[i].GetComponent<SC_EnvObject>().DropItemOnFreeCell()) {
                    itemObjects.RemoveAt(i);
                    haveDroppedAnyItem = true;
                }
            } else {
                // TODO: recycle all "destroyed" assets for further use, instead of Instantiate->Destroy cycle
                Destroy(itemObjects[i]);
                itemObjects.RemoveAt(i);
            }
        }
        stackCounter.text = itemObjects.Count.ToString();

        if (itemObjects.Count < 1) {
            itemID = -1;
            itemType = ItemType.None;
            itemName = "";

            itemIcon.sprite = null;
            itemIcon.enabled = false;

            stackCounter.text = "";
        }

        if (!haveDroppedAnyItem) {
            return false;
            //playerObject.GetComponent<SC_StateController>().onNono();
        }

        return true;
    }
}