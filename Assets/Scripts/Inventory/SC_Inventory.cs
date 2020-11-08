using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using static EasingFunction;

public class SC_Inventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject messagePanel;
    public GameObject slotHolder;
    
    private GameObject[] slot;

    private GameObject[] stackItems;
    private TMP_Text[] stackItemsCounter;

    private GameObject[] itemHolder;

    private bool inventoryEnabled;
    private int allSlots;
    private int[] counter;

    void Start()
    {
        AllSet();
        CloseMessagePanel(); // Call function which close the message panel

        // TEMPORARY: Set Inventory scale to 0 (DOESN'T WORK BTW, IDK WHY)
        inventory.SetActive(false);
        inventory.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 1);
    }

    void Update()
    {
        // Tip: maybe replace with coroutine
        OpenInventory();
    }

    private void AllSet()
    {
        // Arrays with allSlots indexes
        allSlots = 9;

        itemHolder = new GameObject[allSlots];

        slot = new GameObject[allSlots];

        stackItems = new GameObject[allSlots];
        stackItemsCounter = new TMP_Text[allSlots];

        counter = new int[allSlots];

        // Set slot holder object to slot index
        for(int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            stackItems[i] = slotHolder.transform.GetChild(i).GetChild(1).GetChild(0).gameObject;
            stackItemsCounter[i] = stackItems[i].GetComponent<TMP_Text>();
 
            // Set empty to true if slot doesn't contain any
            if(slot[i].GetComponent<SC_Slot>().item == null) 
            {
                slot[i].GetComponent<SC_Slot>().empty = true;
            }
        }
    }

    IEnumerator InventoryAnimation(bool isOpen) {
        Function easingFunc = GetEasingFunction(Ease.EaseInBounce);
        float interpolationRatio = 0.0f;
        float interpolationProgress = 0.0f;

        while (interpolationRatio < 1.0f) {
            if (isOpen) {
                inventory.SetActive(true);
                inventory.GetComponent<RectTransform>().localScale = Vector3.Lerp(inventory.GetComponent<RectTransform>().localScale, new Vector3(1, 1, 1), interpolationRatio);
            } else {
                inventory.GetComponent<RectTransform>().localScale = Vector3.Lerp(inventory.GetComponent<RectTransform>().localScale, new Vector3(0, 0, 1), interpolationRatio);
            }

            interpolationRatio = easingFunc(0, 1, interpolationProgress);
            interpolationProgress += .05f;

            yield return new WaitForEndOfFrame();
        }

        if (!isOpen) {
            inventory.SetActive(false);
        }
    }

    // Open/close inventory
    private void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            inventoryEnabled = !inventoryEnabled;
            StartCoroutine(InventoryAnimation(inventoryEnabled));
            //inventory.SetActive(inventoryEnabled);
        }
    }

    // Message panel
    public void OpenMessagePanel(string text)
    {
        messagePanel.SetActive(true);
    }

    public void CloseMessagePanel()
    {
        messagePanel.SetActive(false);
    }

    // Add item to inventory slot
    public void AddItem(SC_Item item)
    {
        for(int i = 0; i < allSlots; i++)
        {
            // Check specific item for stack
            if(item.ID == 1)
            {
                // Check empty slots if true then move item to slot holder with his parameters (icon, ID, type, ...)
                if (slot[i].GetComponent<SC_Slot>().ID == item.ID)
                {
                    itemHolder[i] = item.itemObject;

                    slot[i].GetComponent<SC_Slot>().item = itemHolder[i];
                    slot[i].GetComponent<SC_Slot>().icon = item.icon;
                    slot[i].GetComponent<SC_Slot>().ID = item.ID;
                    slot[i].GetComponent<SC_Slot>().type = item.type;
                    slot[i].GetComponent<SC_Slot>().description = item.description;
                    slot[i].GetComponent<SC_Slot>().empty = false;
                    slot[i].GetComponent<SC_Slot>().UpdateSlot(); // Update slot image to item image

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = true; // Set item picked up value to true

                    itemHolder[i].transform.parent = slot[i].transform;
                    itemHolder[i].SetActive(false); // Change visibility of picked up item to non-visible

                    counter[i]++;
                    stackItemsCounter[i].text = counter[i].ToString();

                    break;

                } else if(slot[i].GetComponent<SC_Slot>().empty && slot[i].GetComponent<SC_Slot>().ID != item.ID)
                {     
                    itemHolder[i] = item.itemObject;

                    slot[i].GetComponent<SC_Slot>().item = itemHolder[i];
                    slot[i].GetComponent<SC_Slot>().icon = item.icon;
                    slot[i].GetComponent<SC_Slot>().ID = item.ID;
                    slot[i].GetComponent<SC_Slot>().type = item.type;
                    slot[i].GetComponent<SC_Slot>().description = item.description;
                    slot[i].GetComponent<SC_Slot>().empty = false;
                    slot[i].GetComponent<SC_Slot>().UpdateSlot(); // Update slot image to item image

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = true; // Set item picked up value to true

                    itemHolder[i].transform.parent = slot[i].transform;
                    itemHolder[i].SetActive(false); // Change visibility of picked up item to non-visible

                    counter[i]++;
                    stackItemsCounter[i].text = counter[i].ToString();

                    break;
                }
            } else
            {
                // Check empty slots if true then move item to slot holder with his parameters (icon, ID, type, ...)
                if(slot[i].GetComponent<SC_Slot>().empty && slot[i].GetComponent<SC_Slot>().ID != item.ID)
                {     
                    itemHolder[i] = item.itemObject;

                    slot[i].GetComponent<SC_Slot>().item = itemHolder[i];
                    slot[i].GetComponent<SC_Slot>().icon = item.icon;
                    slot[i].GetComponent<SC_Slot>().ID = item.ID;
                    slot[i].GetComponent<SC_Slot>().type = item.type;
                    slot[i].GetComponent<SC_Slot>().description = item.description;
                    slot[i].GetComponent<SC_Slot>().empty = false;
                    slot[i].GetComponent<SC_Slot>().UpdateSlot(); // Update slot image to item image

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = true; // Set item picked up value to true

                    itemHolder[i].transform.parent = slot[i].transform;
                    itemHolder[i].SetActive(false); // Change visibility of picked up item to non-visible

                    break;
                }
            }
        }
    }

    // Drop item from slot
    public void DropItem()
    {
        for(int i = 0; i < allSlots; i++)
        {
            if(slot[i].GetComponent<SC_Slot>().ID == 1)
            {
                if(!(slot[i].GetComponent<SC_Slot>().empty) /*&& slot[i].GetComponent<SC_Slot>().item == */)
                { 
                    slot[i].GetComponent<SC_Slot>().item = null;
                    slot[i].GetComponent<SC_Slot>().icon = null;
                    slot[i].GetComponent<SC_Slot>().ID = 0;
                    slot[i].GetComponent<SC_Slot>().type = null;
                    slot[i].GetComponent<SC_Slot>().description = null;
                    slot[i].GetComponent<SC_Slot>().empty = true;
                    slot[i].GetComponent<SC_Slot>().DropUpdateSlot();

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = false;

                    // slot[i].transform = ;
                    itemHolder[i].SetActive(true);

                    counter[i]--;
                    stackItemsCounter[i].text = counter[i].ToString();

                    break;
                }
            } else
            {
                // Check empty slots if true then move item to slot holder with his parameters (icon, ID, type, ...)
                if(!(slot[i].GetComponent<SC_Slot>().empty) /*&& slot[i].GetComponent<SC_Slot>().item == */)
                {     
                    slot[i].GetComponent<SC_Slot>().item = null;
                    slot[i].GetComponent<SC_Slot>().icon = null;
                    slot[i].GetComponent<SC_Slot>().ID = 0;
                    slot[i].GetComponent<SC_Slot>().type = null;
                    slot[i].GetComponent<SC_Slot>().description = null;
                    slot[i].GetComponent<SC_Slot>().empty = true;
                    slot[i].GetComponent<SC_Slot>().DropUpdateSlot();

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = false;

                    // slot[i].transform =;
                    itemHolder[i].SetActive(true);

                    break;
                }
            }
        }
    }
}
