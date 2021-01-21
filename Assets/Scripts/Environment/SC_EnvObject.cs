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