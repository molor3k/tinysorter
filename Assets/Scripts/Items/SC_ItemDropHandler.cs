using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SC_ItemDropHandler : MonoBehaviour, IDropHandler
{
    SC_Slot slot;

    void Start() 
    {
        AllSet();
    }

    private void AllSet()
    {
        slot = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Slot>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if(!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            slot.DropItem();
        }
    }
}
