using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_Interactions : MonoBehaviour
{
    public GameObject actionPanel;
    public GameObject itemNamePanel;
    public Camera cam;

    private SC_Inventory inventory;
    private SC_InputController inputController;
    private SC_StateController stateController;

    private SC_Item mItemToPickup = null;
    private GameObject currentItem = null;

    public bool startRecycle;


    void Start() {
        AllSet();
    }

    void Update()
    {
        if (stateController.getCurrentState() != SC_StateController.States.OPEN_INVENTORY) {
            CheckInteraction();
            MovePanel(actionPanel, new Vector3(0.0f, 30.0f, 0.0f));
            MovePanel(itemNamePanel, new Vector3(0.0f, 100.0f, 0.0f));
        } else {
            currentItem = null;
            ClosePanel(actionPanel);
            ClosePanel(itemNamePanel);
        }
    }

    private void AllSet()
    {
        inventory = gameObject.GetComponent<SC_Inventory>();
        inputController = gameObject.GetComponent<SC_InputController>();
        stateController = gameObject.GetComponent<SC_StateController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentItem == null) {
            currentItem = other.gameObject;

            switch(other.tag) {
                case "Item":
                    SC_Item item = other.GetComponent<SC_Item>();

                    if (item != null) {
                        OpenPanel(actionPanel, "");
                        OpenPanel(itemNamePanel, other.GetComponent<SC_Item>().itemName);
                        mItemToPickup = item;
                    }
                break;

                case "SortingCan":
                    OpenPanel(actionPanel, "");
                    OpenPanel(itemNamePanel, other.GetComponent<SC_SortingCan>().sortingType.ToString());
                    //startRecycle = true;
                break;

                case "Can":
                    OpenPanel(actionPanel, "");
                    OpenPanel(itemNamePanel, "trash can");
                break;

                case "Bush":
                    if (stateController.getCurrentState() ==  SC_StateController.States.RUN) {
                        other.GetComponent<ParticleSystem>().Emit(15);
                    }
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentItem == other.gameObject) {
            currentItem = null;

            switch(other.tag)
            {
                case "Item":
                    SC_Item item = other.GetComponent<SC_Item>();

                    if(item != null)
                    {
                        ClosePanel(actionPanel);
                        ClosePanel(itemNamePanel);
                        mItemToPickup = null;
                    }
                break;

                case "SortingCan":
                    ClosePanel(actionPanel);
                    ClosePanel(itemNamePanel);
                    startRecycle = false;
                break;

                case "Can":
                    ClosePanel(actionPanel);
                    ClosePanel(itemNamePanel);
                    startRecycle = false;
                break;
            }
        }
    }

    private void CheckInteraction() 
    {
        if (inputController.isAction) {
            if (mItemToPickup != null && mItemToPickup.isPickedUp != true) {
                inventory.AddItemToFreeSlot(mItemToPickup);
                stateController.onPickItem();

                ClosePanel(actionPanel);
                ClosePanel(itemNamePanel);

                mItemToPickup = null;
                currentItem = null;
            } 
        }
    }

    TMP_Text getPanelTextComponent(GameObject panel) {
        return panel.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
    }

    void MovePanel(GameObject panel, Vector3 offset) {
        if (currentItem != null) {
            panel.transform.position = cam.WorldToScreenPoint(currentItem.transform.position);
            panel.transform.position += offset;
        }
    }

    private void OpenPanel(GameObject panel, string text) {
        panel.SetActive(true);

        if (text != "") {
            TMP_Text textComponent = getPanelTextComponent(panel);
            textComponent.text = text;
        }
    }

    private void ClosePanel(GameObject panel) {
        panel.SetActive(false);
    }
}