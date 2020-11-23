using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SC_RandomSpawnGarbage : MonoBehaviour
{
    public GameObject spawnGameobject;
    private GameObject AIObject;
    private SC_EnvGrid environmentGrid;

    public int timeToSpawnGameobject = 0;

    void Start() {
        AIObject = GameObject.Find("AI");
        environmentGrid = GameObject.Find("Env").GetComponent<SC_EnvGrid>();
    }

    void Update() {
        if (Application.isEditor) {
            StartCoroutine(AddItemToGrid());
        }

        if(timeToSpawnGameobject >= 500)
        {
            SpawnItemOnFreeCell();
            timeToSpawnGameobject = 0;
        }

        timeToSpawnGameobject++;
    }

    private void SpawnItemOnFreeCell() {
        Vector3 AIPos = AIObject.transform.position;
        Vector2 AIPosToGrid = environmentGrid.worldToGrid(AIPos);
        GridCell firstFreeCell = environmentGrid.findFirstFreeCell(AIPosToGrid);
        Vector3 firstFreeCellWorldPos = environmentGrid.gridToWorld(firstFreeCell.getPosition());
    
        Vector3 spawnGameobjectPosition = new Vector3(firstFreeCellWorldPos.x, AIPos.y + .5f, firstFreeCellWorldPos.z);
        Instantiate(spawnGameobject, spawnGameobjectPosition, Quaternion.identity);
        StartCoroutine(AddItemToGrid());
    }

    IEnumerator AddItemToGrid() {
        yield return new WaitForSeconds(0.5f);

        environmentGrid.cellAddObject(spawnGameobject);
    }
}
