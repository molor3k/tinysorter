using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using static SC_States;

public class SC_ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {

    private Transform selectedSortingCan;
    private SC_Inventory inventory;
    private SC_Interactions interactionController;
    private SC_StateController stateController;


    void Start() {
        GameObject player = GameObject.Find("Player");

        inventory = player.GetComponent<SC_Inventory>();
        interactionController = player.GetComponent<SC_Interactions>();
        stateController = player.GetComponent<SC_StateController>();
    }
    
    public void OnDrag(PointerEventData eventData) {
        States currentState = stateController.getCurrentState();

        if (currentState == States.CLOSE_INVENTORY) {
            // TODO: когда кнопка не отжата и инвентарь закрывается - айтем замораживается на месте и не возвращается на место слота
            eventData.pointerDrag = null;
            ResetValues();
        } else {
            RaycastHit hit;

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
            OnDropItem();
        } else if (currentState == States.RECYCLE) {
            OnRecycleItem();
        }

        ResetValues();
    }

    public void ResetValues() {
        inventory.currentSlotID = -1;
        transform.localPosition = Vector3.zero;
        selectedSortingCan = null;
    }

    private void OnDropItem() {
        RectTransform invPanel = inventory.transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition)) {
            inventory.DropSelectedItem(true);
        }
    }

    private void OnRecycleItem() {
        if (selectedSortingCan) {
            SC_SortingCan sortingCan = selectedSortingCan.GetComponent<SC_SortingCan>();
            SC_Slot inventorySlot = inventory.GetSelectedItem();

            // Check if player sorted trash correctly
            if (sortingCan.sortingType == inventorySlot.GetItemType()) {
                inventory.pointsForSorting += 10;
                Debug.Log("Correct type, points: " + inventory.pointsForSorting);
            } else {
                inventory.pointsForSorting -= 10;
                Debug.Log("Incorrect type, points: " + inventory.pointsForSorting);
            }
            
            // Clear material of previously selected sorting can and drop current item
            sortingCan.OutlineCan(false);
            inventory.DropSelectedItem(false);
        }
    }
}
