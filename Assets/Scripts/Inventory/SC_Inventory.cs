using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using static EasingFunction;

public class SC_Inventory : MonoBehaviour
{
    public GameObject inventory;
    public GameObject slotHolder;

    public SC_CameraController cameraController;
    public List<SC_Slot> slotList;

    public int currentSlotID = -1;
    
    private bool inventoryEnabled;
    private int slotsNumber = 7;
    private int itemsNumberMax = 9;

    private SC_InputController inputController;
    private SC_StateController stateController;


    void Start() {
        FillList();

        cameraController = gameObject.GetComponent<SC_CameraController>();
        inputController = gameObject.GetComponent<SC_InputController>();
        stateController = gameObject.GetComponent<SC_StateController>();
    }

    void Update() {
        CheckInventory();
    }

    private void FillList() {
        slotList = new List<SC_Slot>();

        for(int i = 0; i < slotsNumber; i++)
        {
            slotList.Add(slotHolder.transform.GetChild(i).GetComponent<SC_Slot>());
        }

    }

    public void AddItemToFreeSlot(SC_Item item) {
        int freeSlotID = 0;
        int currentSlotID = 0;

        foreach (var slot in slotList)
        {
            if (slot.itemID == item.ID) {
                if (slot.itemObjects.Count < itemsNumberMax) {
                    // pridame
                    slot.AddItem(item);
                    return;
                }
            } else {
                if (slot.itemObjects.Count == 0) {
                    // pocet nula tak prvy volny
                    freeSlotID = currentSlotID;
                    break;
                }
            }

            currentSlotID++;
        }

        // pridat ak nebol najdeny ziadny element
        SC_Slot freeSlot = slotList.ElementAt(freeSlotID);
        freeSlot.AddItem(item);
    }

    public void DropSelectedItem() {
        SC_Slot currentSlot = slotList.ElementAt(currentSlotID);
        currentSlot.DropItem();

        stateController.onDropItem();
        currentSlotID = -1;
    }

    IEnumerator InventoryAnimation(bool isOpen) {
        Vector3 fromScale = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 toScale = new Vector3(1.0f, 1.0f, 1.0f);

        Vector3 fromPosition = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 toPosition = new Vector3(0.0f, 200.0f, 0.0f);

        Ease easeType = Ease.Spring;

        if (isOpen) {
            inventory.SetActive(true);
            cameraController.OpenInventory();
        } else {
            var fromScaleOG = fromScale;
            var fromPositionOG = fromPosition;

            fromScale = toScale;
            toScale = fromScaleOG;

            fromPosition = toPosition;
            toPosition = fromPositionOG;

            cameraController.CloseInventory();
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
    private void CheckInventory()
    {
        //bool isOpening = (stateController.getCurrentState() == SC_StateController.States.OPEN_INVENTORY) && !inventoryEnabled;
        //bool isClosing = (stateController.getCurrentState() != SC_StateController.States.OPEN_INVENTORY) && inventoryEnabled;
        
        if (inputController.isOpeningInventory) {
            inventoryEnabled = !inventoryEnabled;
            StartCoroutine(InventoryAnimation(inventoryEnabled));
        }
    }
}
