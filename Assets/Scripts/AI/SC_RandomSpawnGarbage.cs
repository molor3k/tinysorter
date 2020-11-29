using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SC_RandomSpawnGarbage : MonoBehaviour
{
    public List<GameObject> SpawnGarbage;
    private GameObject AIObject;
    private SC_EnvGrid environmentGrid;

    private int timeToSpawnGarbage = 0;
    private bool canSpawnGarbage;

    void Start() {
        AIObject = GameObject.Find("AI");
        environmentGrid = GameObject.Find("Env").GetComponent<SC_EnvGrid>();
        canSpawnGarbage = true;
    }

    void Update() {
        if(timeToSpawnGarbage >= 500 && canSpawnGarbage) {
            SpawnItemOnFreeCell();
            timeToSpawnGarbage = 0;
        }

        timeToSpawnGarbage++;
    }

    private void SpawnItemOnFreeCell() {
        GameObject randomItem = SpawnGarbage[Random.Range(0, (SpawnGarbage.Count - 1))];
        Vector3 AIPos = AIObject.transform.position;
        Vector2 AIPosToGrid = environmentGrid.worldToGrid(AIPos);
        GridCell firstFreeCell = environmentGrid.findFirstFreeCell(AIPosToGrid);
        Vector3 firstFreeCellWorldPos = environmentGrid.gridToWorld(firstFreeCell.getPosition());
    
        Vector3 SpawnGarbagePosition = new Vector3(firstFreeCellWorldPos.x, AIPos.y + .5f, firstFreeCellWorldPos.z);
        Instantiate(randomItem, SpawnGarbagePosition, Quaternion.identity);
        environmentGrid.cellAddObject(randomItem);
    }
}
