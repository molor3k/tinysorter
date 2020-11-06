using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SC_ItemDropHandler : MonoBehaviour, IDropHandler
{
    SC_Inventory inventory;

    void Start() 
    {
        AllSet();
    }

    private void AllSet()
    {
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>(); // Use SC_Inventory values, functions, .. in this class
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if(!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            inventory.DropItem();
        }
    }
}
