/*
MIT License

Copyright (c) 2021 IBPM Team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SC_RandomSpawnGarbage : MonoBehaviour
{
    public List<GameObject> SpawnGarbage;
    private SC_EnvGrid environmentGrid;

    public int garbageDropDelay = 500;
    private int garbageDropTime = 0;

    void Start() {
        garbageDropTime = garbageDropDelay;
        environmentGrid = GameObject.Find("Env").GetComponent<SC_EnvGrid>();
    }

    void Update() {
        if (garbageDropTime <= 0) {
            Debug.Log("DROP");
            SpawnItemOnFreeCell();
            garbageDropTime = garbageDropDelay;
        } else {
            garbageDropTime--;
        }
    }

    private void SpawnItemOnFreeCell() {
        GameObject randomItem = SpawnGarbage[Random.Range(0, SpawnGarbage.Count)];
        Vector3 AIPos = transform.position;
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
