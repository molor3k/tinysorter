using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// OBSOLETE CLASS!!!!
public class SC_ItemDropHandler : MonoBehaviour, IDropHandler
{
    public int pointsForSorting = 0;

    private SC_Inventory inventory;
    // private SC_Slot slot;
    private SC_Interactions interaction;

    void Start() 
    {
        AllSet();
    }

    private void AllSet()
    {
        GameObject player = GameObject.Find("Player");

        inventory = player.GetComponent<SC_Inventory>();
        interaction = player.GetComponent<SC_Interactions>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        /*
        RectTransform invPanel = transform as RectTransform;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            inventory.DropSelectedItem();
        }

        if(Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;

            if(selection.CompareTag("SortingCan")) {
                if(interaction.canRecycle) //todo: slot.itemType == can.sortingType
                {
                    pointsForSorting++;
                } else {
                    pointsForSorting--;
                }
            }
        }*/
    }
}