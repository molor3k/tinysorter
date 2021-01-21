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

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using static EasingFunction;
using static SC_States;

public class SC_Inventory : MonoBehaviour {

    public GameObject inventory;
    public GameObject slotHolder;

    public SC_CameraController cameraController;

    public int currentSlotID = -1;
    // TODO: remove it to other script
    public int pointsForSorting = 0;
    
    private List<SC_Slot> slotList;
    private SC_InputController inputController;
    private SC_StateController stateController;

    private bool inventoryEnabled;
    private int slotsNumber = 7;
    private int itemsNumberMax = 8;


    void Start() {
        FillList();

        cameraController = gameObject.GetComponent<SC_CameraController>();
        inputController = gameObject.GetComponent<SC_InputController>();
        stateController = gameObject.GetComponent<SC_StateController>();
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update() {
        CheckInventory();
    }

    private void FillList() {
        slotList = new List<SC_Slot>();

        for(int i = 0; i < slotsNumber; i++) {
            slotList.Add(slotHolder.transform.GetChild(i).GetComponent<SC_Slot>());
        }
    }

    public bool AddItemToFreeSlot(SC_Item item) {
        int freeSlotID = -1;
        int currentSlotID = 0;

        foreach (var slot in slotList) {
            if (slot.itemID == item.ID) {
                if (slot.itemObjects.Count < itemsNumberMax) {
                    // pridame
                    // slot.AddItem(item);
                    // return false;
                    freeSlotID = currentSlotID;
                    break;
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
        if (freeSlotID != -1) {
            SC_Slot freeSlot = slotList.ElementAt(freeSlotID);
            freeSlot.AddItem(item);

            // Set state to "Pick Item"
            return true;
        }

        return false;
    }

    public void DropSelectedItem(bool needToDrop, bool isDroppingStack) {
        SC_Slot currentSlot = slotList.ElementAt(currentSlotID);
        if (currentSlot.DropItem(needToDrop, isDroppingStack)) {
            stateController.onDropItem();
            currentSlotID = -1;
        } else {
            stateController.onNono();
        }

    }

    public SC_Slot GetSelectedItem() {
        return slotList.ElementAt(currentSlotID);
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
    private void CheckInventory() {
        States currentState = stateController.getCurrentState();
        bool isInInventory = (currentState == States.OPEN_INVENTORY) || (currentState == States.RECYCLE);

        bool isOpening = isInInventory && !inventoryEnabled;
        bool isClosing = (stateController.getCurrentState() == States.CLOSE_INVENTORY) && inventoryEnabled;

        if (isOpening || isClosing) {
            inventoryEnabled = !inventoryEnabled;
            StartCoroutine(InventoryAnimation(inventoryEnabled));
        }
    }
}
