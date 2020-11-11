/*using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using static EasingFunction;

public class SC_Inventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject slotHolder;

    public GameObject defaultPosition;

    public GameObject toolHolder;
    public GameObject toolHand;

    public SC_CameraController vCam;

    private GameObject[] slot;

    private GameObject[] stackItems;
    private TMP_Text[] stackItemsCounter;

    private GameObject[] itemHolder;

    private GameObject[] toolSlot;
    private GameObject tool;

    private SC_Interactions interaction;

    private bool inventoryEnabled;
    private int allSlots = 7;
    
    private bool toolEnabled;
    private int allToolSlots = 2;

    private bool dropAllStackItems;

    private int[] counter;

    void Start()
    {
        AllSet();

        // Camera
        vCam = GetComponent<SC_CameraController>();
    }

    void Update()
    {
        // Tip: maybe replace with coroutine
        OpenInventory();
        DropAllStack();
        SetTool();
    }

    private void AllSet()
    {
        // Arrays
        slot = new GameObject[allSlots];

        itemHolder = new GameObject[allSlots];

        stackItems = new GameObject[allSlots];
        stackItemsCounter = new TMP_Text[allSlots];

        toolSlot = new GameObject[allToolSlots];

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

        for(int i = 0; i < allToolSlots; i++)
        {
            toolSlot[i] = toolHolder.transform.GetChild(i).gameObject;
        }

        interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Interactions>();
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryEnabled = !inventoryEnabled;
            StartCoroutine(InventoryAnimation(inventoryEnabled));

        } else if (interaction.startRecycle == true && Input.GetKeyDown(KeyCode.E))
        {
            inventoryEnabled = !inventoryEnabled;
            StartCoroutine(InventoryAnimation(inventoryEnabled));
        } 
    }

    private void DropAllStack()
    {
        if (Input.GetButton("Drop"))
        {
            dropAllStackItems = true;

        } else
        {
            dropAllStackItems = false;
        }
    }

    private void SetTool()
    {
        try
        {
            tool = toolHand.transform.GetChild(0).gameObject;
        }
        catch
        {
            Debug.Log("You don't have tool in your hand");
        }

        if(tool != null)
        {
            if(tool.CompareTag("WastePicker"))
            {
                interaction.wastePicker = true;
                interaction.hands = false;
                interaction.rake = false;

            } else if(tool.CompareTag("Rake"))
            {
                interaction.wastePicker = false;
                interaction.hands = false;
                interaction.rake = true;
            } 
        } else 
        {
            interaction.wastePicker = false;
            interaction.hands = true;
            interaction.rake = false;
        }
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
                    slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = itemHolder[i].GetComponent<SC_Item>().icon;
                    slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = true; // Set item picked up value to true

                    itemHolder[i].SetActive(false); // Change visibility of picked up item to non-visible
                    itemHolder[i].transform.parent = slot[i].transform;

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
                    slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = itemHolder[i].GetComponent<SC_Item>().icon;
                    slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = true; // Set item picked up value to true

                    itemHolder[i].SetActive(false); // Change visibility of picked up item to non-visible
                    itemHolder[i].transform.parent = slot[i].transform;

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
                    slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = itemHolder[i].GetComponent<SC_Item>().icon;
                    slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;

                    itemHolder[i].GetComponent<SC_Item>().isPickedUp = true; // Set item picked up value to true

                    itemHolder[i].SetActive(false); // Change visibility of picked up item to non-visible
                    itemHolder[i].transform.parent = slot[i].transform;

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
            if(slot[i].GetComponent<SC_Slot>().ID == 1 && dropAllStackItems == false)
            {
                if(!(slot[i].GetComponent<SC_Slot>().empty))
                { 
                    if(counter[i] <= 1)
                    {
                        slot[i].GetComponent<SC_Slot>().itemObject = null;
                        slot[i].GetComponent<SC_Slot>().icon = null;
                        slot[i].GetComponent<SC_Slot>().ID = 0;
                        slot[i].GetComponent<SC_Slot>().type = null;
                        slot[i].GetComponent<SC_Slot>().description = null;
                        slot[i].GetComponent<SC_Slot>().empty = true;
                        slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
                        slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false; 
                        slot[i].transform.GetChild(2).gameObject.GetComponent<SC_Item>().isPickedUp = false;
                        slot[i].transform.GetChild(2).gameObject.SetActive(true);
                        slot[i].transform.GetChild(2).parent = defaultPosition.transform; // Need to be fixed

                        counter[i]--;
                        stackItemsCounter[i].text = counter[i].ToString();

                        if(counter[i] <= 0)
                        {
                            stackItemsCounter[i].text = null;
                        }

                        break;

                    } else
                    {
                        slot[i].transform.GetChild(2).gameObject.GetComponent<SC_Item>().isPickedUp = false;
                        slot[i].transform.GetChild(2).gameObject.SetActive(true);
                        slot[i].transform.GetChild(2).parent = defaultPosition.transform; // Need to be fixed

                        counter[i]--;
                        stackItemsCounter[i].text = counter[i].ToString();

                        break;
                    }
                }
            } else if(slot[i].GetComponent<SC_Slot>().ID == 1 && dropAllStackItems == true)
            {
                if(!(slot[i].GetComponent<SC_Slot>().empty))
                {
                    while(true)
                    {     
                        slot[i].GetComponent<SC_Slot>().itemObject = null;
                        slot[i].GetComponent<SC_Slot>().icon = null;
                        slot[i].GetComponent<SC_Slot>().ID = 0;
                        slot[i].GetComponent<SC_Slot>().type = null;
                        slot[i].GetComponent<SC_Slot>().description = null;
                        slot[i].GetComponent<SC_Slot>().empty = true;
                        slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
                        slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false; 
                        slot[i].transform.GetChild(2).gameObject.GetComponent<SC_Item>().isPickedUp = false;
                        slot[i].transform.GetChild(2).gameObject.SetActive(true);
                        slot[i].transform.GetChild(2).parent = defaultPosition.transform; // Need to be fixed

                        counter[i]--;
                        stackItemsCounter[i].text = counter[i].ToString();

                        if(counter[i] <= 0)
                        {
                            stackItemsCounter[i].text = null;
                            break;
                        }
                    }
                }
            } else if(slot[i].GetComponent<SC_Slot>().ID != 1 && dropAllStackItems == false)
            {
                // Check empty slots if true then move item to slot holder with his parameters (icon, ID, type, ...)
                if(!(slot[i].GetComponent<SC_Slot>().empty))
                {     
                    slot[i].GetComponent<SC_Slot>().itemObject = null;
                    slot[i].GetComponent<SC_Slot>().icon = null;
                    slot[i].GetComponent<SC_Slot>().ID = 0;
                    slot[i].GetComponent<SC_Slot>().type = null;
                    slot[i].GetComponent<SC_Slot>().description = null;
                    slot[i].GetComponent<SC_Slot>().empty = true;
                    slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
                    slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false; 
                    slot[i].transform.GetChild(2).gameObject.GetComponent<SC_Item>().isPickedUp = false;
                    slot[i].transform.GetChild(2).gameObject.SetActive(true);
                    slot[i].transform.GetChild(2).parent = defaultPosition.transform; // Need to be fixed

                    break;
                }
            }
        }
    }

    public void TransferWastePicker()
    {
        toolEnabled = !toolEnabled;

        if(toolEnabled == true)
        {
            toolSlot[0].transform.GetChild(2).parent = toolHand.transform;
            toolHand.transform.GetChild(0).gameObject.SetActive(true);
            toolSlot[0].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;

        } else if(toolEnabled == false)
        {
            toolHand.transform.GetChild(0).parent = toolSlot[0].transform;
            toolSlot[0].transform.GetChild(2).gameObject.SetActive(false);
            toolSlot[0].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;

            tool = null;
        }
    }

    public void TransferRake()
    {
        toolEnabled = !toolEnabled;

        if(toolEnabled == true)
        {
            toolSlot[1].transform.GetChild(2).parent = toolHand.transform;
            toolHand.transform.GetChild(0).gameObject.SetActive(true);
            toolSlot[1].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;

        } else if(toolEnabled == false)
        {
            toolHand.transform.GetChild(0).parent = toolSlot[1].transform;
            toolSlot[1].transform.GetChild(2).gameObject.SetActive(false);
            toolSlot[1].transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;

            tool = null;
        }
    }
}*/