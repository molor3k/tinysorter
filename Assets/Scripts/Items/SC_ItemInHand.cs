using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_ItemInHand : MonoBehaviour
{
    private GameObject itemInHand;
    private SC_Interactions interaction;

    void Start()
    {
        AllSet();
    }

    void Update()
    {
        SetItemInHand();
    }

    private void AllSet()
    {
        if(this.transform.GetChild(0).gameObject != null)
        {
            itemInHand = this.transform.GetChild(0).gameObject;
        } 

        interaction = GameObject.Find("Player").GetComponent<SC_Interactions>(); // Use SC_Inventory values, functions, .. in this class
    }

    public void SetItemInHand()
    {
        if(itemInHand != null)
        {
            if(itemInHand.CompareTag("WastePicker"))
            {
                interaction.wastePicker = true;
                interaction.hands = false;
                interaction.rake = false;

                Debug.Log(itemInHand.tag);

            } else if(itemInHand.CompareTag("Rake"))
            {
                interaction.wastePicker = false;
                interaction.hands = false;
                interaction.rake = true;

                Debug.Log(itemInHand.tag);
            } 
        } else 
        {
            interaction.wastePicker = false;
            interaction.hands = true;
            interaction.rake = false;

            Debug.Log(itemInHand.tag);
        }
    }
}
