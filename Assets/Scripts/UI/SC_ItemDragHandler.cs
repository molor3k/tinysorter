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
using UnityEngine.EventSystems;

using static SC_States;

public class SC_ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {

    private Transform selectedSortingCan;
    private SC_Inventory inventory;
    private SC_InputController inputController;
    private SC_Interactions interactionController;
    private SC_StateController stateController;
    private bool isDraggingStack = false;


    void Start() {
        GameObject player = GameObject.Find("Player");

        inventory = player.GetComponent<SC_Inventory>();
        inputController = player.GetComponent<SC_InputController>();
        interactionController = player.GetComponent<SC_Interactions>();
        stateController = player.GetComponent<SC_StateController>();
    }
    
    public void OnDrag(PointerEventData eventData) {
        States currentState = stateController.getCurrentState();

        if (currentState == States.CLOSE_INVENTORY) {
            // TODO: Item doesn't return to it's initial position, if inventory is closing and mouse button is still pressed
            eventData.pointerDrag = null;
            ResetValues();
        } else {
            RaycastHit hit;

            // Get isDraggingStack value
            if (inventory.currentSlotID == -1) {
                isDraggingStack = inputController.isSelectingStack; ///////
            }

            // Dragging and changing inventory slot
            transform.position = Input.mousePosition;
            inventory.currentSlotID = transform.parent.parent.GetComponent<SC_Slot>().slotID;

            // Recycling
            if (currentState == States.RECYCLE) {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                // Find a sorting can on mouse position
                if (Physics.Raycast(ray, out hit)) {
                    var selection = hit.transform;

                    if (selection != selectedSortingCan) {
                        // Clear material of previously selected sorting can
                        selectedSortingCan?.GetComponent<SC_SortingCan>().OutlineCan(false);

                        // Apply material to newly selected sorting can
                        if (selection.CompareTag("SortingCan")) {
                            SC_SortingCan sortingCan = selection.GetComponent<SC_SortingCan>();
                            selectedSortingCan = selection;

                            sortingCan.OutlineCan(true);
                            interactionController.OpenPanel(interactionController.recycleTypePanel, sortingCan.sortingType.ToString());
                        } else {
                            selectedSortingCan = null;
                            interactionController.ClosePanel(interactionController.recycleTypePanel);
                        }
                    }
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        States currentState = stateController.getCurrentState();

        if (currentState == States.OPEN_INVENTORY) {
            OnDropItem(isDraggingStack);
        } else if (currentState == States.RECYCLE) {
            OnRecycleItem(isDraggingStack);
        }

        ResetValues();
    }

    public void ResetValues() {
        inventory.currentSlotID = -1;
        transform.localPosition = Vector3.zero;
        selectedSortingCan = null;
    }

    private void OnDropItem(bool isDroppingStack) {
        RectTransform invPanel = inventory.transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition)) {
            inventory.DropSelectedItem(true, isDroppingStack);
        }
    }

    private void OnRecycleItem(bool isDroppingStack) {
        if (selectedSortingCan) {
            SC_SortingCan sortingCan = selectedSortingCan.GetComponent<SC_SortingCan>();
            SC_Slot inventorySlot = inventory.GetSelectedItem();

            GameObject gameManager = GameObject.Find("GameManager");
            AudioClip buzzerSFX = gameManager.GetComponent<SC_SFX>().wrongItemBuzzerSFX;

            // Check if player sorted trash correctly
            if (sortingCan.sortingType == inventorySlot.GetItemType()) {
                inventory.pointsForSorting += 10;
            } else {
                inventory.pointsForSorting -= 10;
                gameManager.GetComponent<AudioSource>().PlayOneShot(buzzerSFX);
            }
    
            selectedSortingCan.GetComponent<AudioSource>().Play();
            
            // Clear material of previously selected sorting can and drop current item
            sortingCan.OutlineCan(false);
            inventory.DropSelectedItem(false, isDroppingStack);
        }
    }
}
