using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SC_ItemDropHandler : MonoBehaviour, IDropHandler
{
    public int pointsForSorting;

    private SC_Inventory inventory;
    // private SC_Slot slot;
    private SC_Interactions interaction;

    void Start() 
    {
        AllSet();
    }

    private void AllSet()
    {
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>();
        // slot = GameObject.Find("Slot").GetComponent<SC_Slot>();
        interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Interactions>();
        pointsForSorting = 0;
    }

    public void OnDrop(PointerEventData eventData)
    {
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

            if(selection.CompareTag("Can"))
            {
                // inventory.DropSelectedItem();

                if(/*slot.itemType == "Organic" && is dropped to yellow can && */interaction.startRecycle == true)
                {
                    pointsForSorting++;
                    Debug.Log("Organic");

                } else
                {
                    pointsForSorting--;
                }

                if(/*slot.itemType == "Metal" && is dropped to green can && */interaction.startRecycle == true)
                {
                    pointsForSorting++;
                    Debug.Log("Metal");

                } else
                {
                    pointsForSorting--;
                }

                if(/*slot.itemType == "Paper" && is dropped to blue can */interaction.startRecycle == true)
                {
                    pointsForSorting++;
                    Debug.Log("Paper");

                } else
                {
                    pointsForSorting--;
                }

                if(/*slot.itemType == "Plastic" && is dropped to white can */interaction.startRecycle == true)
                {
                    pointsForSorting++;
                    Debug.Log("Plastic");

                } else
                {
                    pointsForSorting--;
                }
            }
        }
    }
    
}
