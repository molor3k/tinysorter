using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SC_EnvObject : MonoBehaviour {
    
    private GameObject playerObject;
    private SC_EnvGrid environmentGrid;


    void Start() {
        playerObject = GameObject.Find("Player");
        environmentGrid = GameObject.Find("Env").GetComponent<SC_EnvGrid>();

        StartCoroutine(AddObjectToGrid());
    }

    public GridCell GetFirstFreeCell() {
        Vector3 playerPos = playerObject.transform.position;
        Vector2 playerPosToGrid = environmentGrid.worldToGrid(playerPos);
        GridCell firstFreeCell = environmentGrid.findFirstFreeCell(playerPosToGrid);

        return firstFreeCell;
    }

    public bool DropItemOnFreeCell() {
        Vector3 playerPos = playerObject.transform.position;
        GridCell firstFreeCell = GetFirstFreeCell();

        if (firstFreeCell != null) {
            Vector3 firstFreeCellWorldPos = environmentGrid.gridToWorld(firstFreeCell.getPosition());
            Vector3 targetPosition = new Vector3(firstFreeCellWorldPos.x, playerPos.y + .5f, firstFreeCellWorldPos.z);

            gameObject.transform.position = playerPos + new Vector3(0.0f, 2.0f, 0.0f);
            gameObject.GetComponent<SC_Item>().targetPosition = targetPosition;
            gameObject.GetComponent<SC_Item>().DropItem();

            environmentGrid.cellAddObject(gameObject, targetPosition);

            return true;
        }

        return false;
    }

    IEnumerator AddObjectToGrid() {
        yield return new WaitForSeconds(0.5f);

        environmentGrid.cellAddObject(gameObject, gameObject.transform.position);
    }
}