using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SC_Interactable : MonoBehaviour
{
    public KeyCode interactKey;
    public UnityEvent interactAction;

    private SC_Inventory inventory;

    private bool isInRange;

    void Start()
    {
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>(); //Find SC_Inventory for use values, functions, .. in this class
    }

    void Update()
    {
        //If colliding with object for interaction with object
        if(isInRange)
        {
            //Check if interaction key was pushed
            if(Input.GetKeyDown(interactKey))
            {
                interactAction.Invoke(); //Fire interaction
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //Check if item colliding with Player, set isInRange to true and open message panel
        if(other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            inventory.OpenMessagePanel("");
            
            //Debug.Log("Player now in range");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Check if item exit colliding with Player, set isInRange to false and close message panel
        if(other.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            inventory.CloseMessagePanel();
            
            //Debug.Log("Player now not in range");
        }
    }
}
