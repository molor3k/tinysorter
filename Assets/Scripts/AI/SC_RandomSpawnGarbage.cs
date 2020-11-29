using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SC_RandomSpawnGarbage : MonoBehaviour
{
<<<<<<< Updated upstream
    public List<GameObject> spawnGameobject;
=======
    public List<GameObject> SpawnGarbage;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        if(timeToSpawnGameobject >= 500)
        {
=======
        if(timeToSpawnGarbage >= 500 && canSpawnGarbage) {
>>>>>>> Stashed changes
            SpawnItemOnFreeCell();
            timeToSpawnGarbage = 0;
        }

        timeToSpawnGarbage++;
    }

    private void SpawnItemOnFreeCell() {
<<<<<<< Updated upstream
        GameObject randomItem = spawnGameobject[Random.Range(0, (spawnGameobject.Count - 1))];
=======
        GameObject randomItem = SpawnGarbage[Random.Range(0, (SpawnGarbage.Count - 1))];
>>>>>>> Stashed changes
        Vector3 AIPos = AIObject.transform.position;
        Vector2 AIPosToGrid = environmentGrid.worldToGrid(AIPos);
        GridCell firstFreeCell = environmentGrid.findFirstFreeCell(AIPosToGrid);
        Vector3 firstFreeCellWorldPos = environmentGrid.gridToWorld(firstFreeCell.getPosition());
    
<<<<<<< Updated upstream
        Vector3 spawnGameobjectPosition = new Vector3(firstFreeCellWorldPos.x, AIPos.y + .5f, firstFreeCellWorldPos.z);
        Instantiate(randomItem, spawnGameobjectPosition, Quaternion.identity);
=======
        Vector3 SpawnGarbagePosition = new Vector3(firstFreeCellWorldPos.x, AIPos.y + .5f, firstFreeCellWorldPos.z);
        Instantiate(randomItem, SpawnGarbagePosition, Quaternion.identity);
>>>>>>> Stashed changes
        environmentGrid.cellAddObject(randomItem);
    }
}
