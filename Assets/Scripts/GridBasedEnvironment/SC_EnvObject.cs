using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SC_EnvObject : MonoBehaviour
{
    private GameObject playerObject;
    private SC_EnvGrid environmentGrid;


    void Start() {
        playerObject = GameObject.Find("Player");
        environmentGrid = GameObject.Find("Env").GetComponent<SC_EnvGrid>();
    }

    void Update() {
        if (Application.isEditor) {
            StartCoroutine(AddItemToGrid());
        }
    }

    public void DropItemOnFreeCell() {
        Vector3 playerPos = playerObject.transform.position;
        Vector2 playerPosToGrid = environmentGrid.worldToGrid(playerPos);
        GridCell firstFreeCell = environmentGrid.findFirstFreeCell(playerPosToGrid);
        Vector3 firstFreeCellWorldPos = environmentGrid.gridToWorld(firstFreeCell.getPosition());
    
        gameObject.GetComponent<SC_Item>().DropItem();
        gameObject.transform.position = new Vector3(firstFreeCellWorldPos.x, playerPos.y + .5f, firstFreeCellWorldPos.z);
        StartCoroutine(AddItemToGrid());
    }

    IEnumerator AddItemToGrid() {
        yield return new WaitForSeconds(0.5f);

        environmentGrid.cellAddObject(gameObject);
    }
}