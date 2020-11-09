using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Recycling : MonoBehaviour
{
    private SC_Interactions interaction;
    private SC_Inventory inventory;

    void Start()
    {
        AllSet();
    }

    void Update()
    {
        
    }

    private void AllSet()
    {
        interaction = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Interactions>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Inventory>();
    }
}
