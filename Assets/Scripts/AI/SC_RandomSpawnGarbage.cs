using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SC_RandomSpawnGarbage : MonoBehaviour
{
    public List<GameObject> SpawnGarbage;
    private GameObject AIObject;
    private SC_EnvGrid environmentGrid;

    public int garbageDropDelay = 500;
    private int garbageDropTime = 0;

    void Start() {
        garbageDropTime = garbageDropDelay;
        AIObject = GameObject.Find("AI");
        environmentGrid = GameObject.Find("Env").GetComponent<SC_EnvGrid>();
    }

    void Update() {
        if (garbageDropTime <= 0) {
            SpawnItemOnFreeCell();
            garbageDropTime = garbageDropDelay;
        } else {
            garbageDropTime--;
        }
    }

    private void SpawnItemOnFreeCell() {
        GameObject randomItem = SpawnGarbage[Random.Range(0, SpawnGarbage.Count)];
        Vector3 AIPos = AIObject.transform.position;
        Vector2 AIPosToGrid = environmentGrid.worldToGrid(AIPos);
        GridCell firstFreeCell = environmentGrid.findFirstFreeCell(AIPosToGrid);

        if (firstFreeCell != null) {
            Vector3 firstFreeCellWorldPos = environmentGrid.gridToWorld(firstFreeCell.getPosition());
    
            var randomItemPrefab = Instantiate(randomItem, AIPos + new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
            environmentGrid.cellAddObject(randomItemPrefab, firstFreeCellWorldPos);
            
            randomItemPrefab.GetComponent<SC_Item>().targetPosition = firstFreeCellWorldPos;
            randomItemPrefab.GetComponent<SC_Item>().DropItem();    
        }
    }
}
