using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SC_RandomSpawnGarbage : MonoBehaviour
{
    public List<GameObject> spawnGameobject;
    private GameObject AIObject;
    private SC_EnvGrid environmentGrid;

    public int timeToSpawnGameobject = 0;

    void Start() {
        AIObject = GameObject.Find("AI");
        environmentGrid = GameObject.Find("Env").GetComponent<SC_EnvGrid>();
    }

    void Update() {
        if(timeToSpawnGameobject >= 500)
        {
            SpawnItemOnFreeCell();
            timeToSpawnGameobject = 0;
        }

        timeToSpawnGameobject++;
    }

    private void SpawnItemOnFreeCell() {
        GameObject randomItem = spawnGameobject[Random.Range(0, (spawnGameobject.Count - 1))];
        Vector3 AIPos = AIObject.transform.position;
        Vector2 AIPosToGrid = environmentGrid.worldToGrid(AIPos);
        GridCell firstFreeCell = environmentGrid.findFirstFreeCell(AIPosToGrid);
        Vector3 firstFreeCellWorldPos = environmentGrid.gridToWorld(firstFreeCell.getPosition());
    
        Vector3 spawnGameobjectPosition = new Vector3(firstFreeCellWorldPos.x, AIPos.y + .5f, firstFreeCellWorldPos.z);
        Instantiate(randomItem, spawnGameobjectPosition, Quaternion.identity);
        environmentGrid.cellAddObject(randomItem);
    }
}
