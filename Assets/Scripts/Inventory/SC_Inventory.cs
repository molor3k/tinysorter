using System;
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

    public SC_CameraController vCam;
    
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

        // Camera
        vCam = GetComponent<SC_CameraController>();
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
            if(slot[i].GetComponent<SC_Slot>().itemObject == null) 
            {
                slot[i].GetComponent<SC_Slot>().empty = true;
            }
        }
    }

    IEnumerator InventoryAnimation(bool isOpen) {
        Vector3 fromScale = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 toScale = new Vector3(1.0f, 1.0f, 1.0f);

        Vector3 fromPosition = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 toPosition = new Vector3(0.0f, 200.0f, 0.0f);

        Ease easeType = Ease.Spring;

        if (isOpen) {
            inventory.SetActive(true);
            vCam.OpenInventory();
        } else {
            var fromScaleOG = fromScale;
            var fromPositionOG = fromPosition;

            fromScale = toScale;
            toScale = fromScaleOG;

            fromPosition = toPosition;
            toPosition = fromPositionOG;

            vCam.CloseInventory();
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(
            Vector3Ease(
                (result => inventory.GetComponent<RectTransform>().localScale = result),
                fromScale, 
                toScale, 
                easeType,
                .035f
            )
        );

        StartCoroutine(
            Vector3Ease(
                (result => inventory.GetComponent<RectTransform>().localPosition = new Vector3(inventory.GetComponent<RectTransform>().localPosition.x, result.y, 0.0f)),
                fromPosition, 
                toPosition, 
                easeType,
                .05f
            )
        );

        yield return new WaitForSeconds(0.1f);

        if (!isOpen) {
            inventory.SetActive(false);
        }
    }

    IEnumerator Vector3Ease(Action<Vector3> valueToChange, Vector3 from, Vector3 to, Ease easingType, float interpolationIncrement) {
        Vector3 interpolationRatio;
        float interpolationProgress = 0.0f;

        Function easingFunc = GetEasingFunction(easingType);


        while (interpolationProgress < 1.0f) {
            var x = easingFunc(from.x, to.x, interpolationProgress);
            var y = easingFunc(from.y, to.y, interpolationProgress);
            var z = easingFunc(from.z, to.z, interpolationProgress);

            interpolationRatio = new Vector3(x, y, z);
            interpolationProgress += interpolationIncrement;
            
            valueToChange(interpolationRatio);

            yield return new WaitForEndOfFrame();
        }
    }

    // Open/close inventory
    private void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
            inventoryEnabled = !inventoryEnabled;
            StartCoroutine(InventoryAnimation(inventoryEnabled));
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

                    slot[i].GetComponent<SC_Slot>().itemObject = itemHolder[i];
                    slot[i].GetComponent<SC_Slot>().icon = item.icon;
                    slot[i].GetComponent<SC_Slot>().ID = item.ID;
                    slot[i].GetComponent<SC_Slot>().type = item.type;
                    slot[i].GetComponent<SC_Slot>().description = item.description;
                    slot[i].GetComponent<SC_Slot>().empty = false;
                    //slot[i].GetComponent<SC_Slot>().UpdateSlot(); // Update slot image to item image

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = true; // Set item picked up value to true

                    itemHolder[i].transform.parent = slot[i].transform;
                    itemHolder[i].SetActive(false); // Change visibility of picked up item to non-visible

                    counter[i]++;
                    stackItemsCounter[i].text = counter[i].ToString();

                    break;

                } else if(slot[i].GetComponent<SC_Slot>().empty && slot[i].GetComponent<SC_Slot>().ID != item.ID)
                {     
                    itemHolder[i] = item.itemObject;

                    slot[i].GetComponent<SC_Slot>().itemObject = itemHolder[i];
                    slot[i].GetComponent<SC_Slot>().icon = item.icon;
                    slot[i].GetComponent<SC_Slot>().ID = item.ID;
                    slot[i].GetComponent<SC_Slot>().type = item.type;
                    slot[i].GetComponent<SC_Slot>().description = item.description;
                    slot[i].GetComponent<SC_Slot>().empty = false;
                    //slot[i].GetComponent<SC_Slot>().UpdateSlot(); // Update slot image to item image

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

                    slot[i].GetComponent<SC_Slot>().itemObject = itemHolder[i];
                    slot[i].GetComponent<SC_Slot>().icon = item.icon;
                    slot[i].GetComponent<SC_Slot>().ID = item.ID;
                    slot[i].GetComponent<SC_Slot>().type = item.type;
                    slot[i].GetComponent<SC_Slot>().description = item.description;
                    slot[i].GetComponent<SC_Slot>().empty = false;
                    //slot[i].GetComponent<SC_Slot>().UpdateSlot(); // Update slot image to item image

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
                    slot[i].GetComponent<SC_Slot>().itemObject = null;
                    slot[i].GetComponent<SC_Slot>().icon = null;
                    slot[i].GetComponent<SC_Slot>().ID = 0;
                    slot[i].GetComponent<SC_Slot>().type = null;
                    slot[i].GetComponent<SC_Slot>().description = null;
                    slot[i].GetComponent<SC_Slot>().empty = true;
                    //slot[i].GetComponent<SC_Slot>().DropUpdateSlot();

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
                    slot[i].GetComponent<SC_Slot>().itemObject = null;
                    slot[i].GetComponent<SC_Slot>().icon = null;
                    slot[i].GetComponent<SC_Slot>().ID = 0;
                    slot[i].GetComponent<SC_Slot>().type = null;
                    slot[i].GetComponent<SC_Slot>().description = null;
                    slot[i].GetComponent<SC_Slot>().empty = true;
                    //slot[i].GetComponent<SC_Slot>().DropUpdateSlot();

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = false;

                    // slot[i].transform =;
                    itemHolder[i].SetActive(true);

                    break;
                }
            }
        }
    }
}
