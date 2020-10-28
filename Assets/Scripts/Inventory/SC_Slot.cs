﻿using System.Collections;
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
        slotIconGo = transform.GetChild(0);
    }

    public void UpdateSlot()
    {
        slotIconGo.GetComponent<Image>().sprite = icon;
    }
}