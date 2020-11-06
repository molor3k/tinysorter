using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_Slot : MonoBehaviour
{
    public GameObject item;
    public Transform slotIconGo;
    public Sprite icon;

    public int ID;
    public string type;
    public string description;
    public bool empty;

    void Start ()
    {
        AllSet();
    }

    private void AllSet()
    {
        slotIconGo = transform.GetChild(0).GetChild(0);
    }

    // Update slot icons when add item
    public void UpdateSlot()
    {
        slotIconGo.GetComponent<Image>().sprite = icon;
        slotIconGo.GetComponent<Image>().enabled = true;
    }

    // Update slot icons when drop item
    public void DropUpdateSlot()
    {
        slotIconGo.GetComponent<Image>().sprite = null;
        slotIconGo.GetComponent<Image>().enabled = false;
    }
}
