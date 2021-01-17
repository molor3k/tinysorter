﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridCell;

public class SC_EnvGrid : MonoBehaviour {

    public bool isDrawGizmos = false;

    public Transform startPoint;
    public Transform endPoint;
    public float cellSize = 4.0f;

    private int numberOfRows = 0;
    private int numberOfColumns = 0;

    private Vector3 startPosition;
    private List<GridCell> cells;


    void Start() {
        startPosition = startPoint.position;

        numberOfRows = (int)Mathf.Abs(Mathf.Floor((endPoint.position.z - startPosition.z) / cellSize));
        numberOfColumns = (int)Mathf.Abs(Mathf.Floor((endPoint.position.x - startPosition.x) / cellSize));

        cells = new List<GridCell>();
        fillCells();
    }

    void fillCells() {
        for(int row = 0; row < numberOfRows; row++) {
            for(int col = 0; col < numberOfColumns; col++) {
                cells.Add(new GridCell(new Vector2(col, row), null));
            }
        }
    }

    public void cellAddObject(GameObject obj, Vector3 targetPos) {
        // todo: add pointer to particular cell for item, so it could easily free cell later
        Vector2 gridPos = worldToGrid(targetPos);
        GridCell gridCell = findCellByPosition(gridPos);

        gridCell.setObject(obj);

        // Add cell pointer if it's Item
        SC_Item item = obj.GetComponent<SC_Item>();
        if (item) {
            item.cell = gridCell;
        }
    }

    public GridCell findCellByPosition(Vector2 gridPosition) {
        foreach(GridCell cell in cells) {
            Vector2 pos = cell.getPosition();

            if (pos.x == gridPosition.x) {
                if (pos.y == gridPosition.y) {
                    return cell;
                }
            }
        }

        return null;
    }

    public bool IsCellFree(GridCell cell) {
        var isCellFree = false;

        if (System.Object.ReferenceEquals(cell.getObject(), null)) {
            isCellFree = true;
        }
        
        //Debug.Log("IsCellFree: " + isCellFree);
        return isCellFree;
    }

    public GridCell findFirstFreeCell(Vector2 gridPosition) {
        Vector2 gridPositionWithOffset = gridPosition + new Vector2(1, 1);

        for(var i = 0; i < 3; i++) {
            for(var j = 0; j < 3; j++) {
                var curPos = gridPositionWithOffset - new Vector2(i, j);

                if (curPos != gridPosition) {
                    var cell = findCellByPosition(curPos);

                    if (cell != null) {
                        if (IsCellFree(cell)) {
                            //Debug.Log("Cell is choosed - " + curPos);
                            return cell;
                        } else {
                            //Debug.Log("Cell is filled - " + curPos);
                        }
                    } else {
                        //Debug.Log("Cell is null - " + curPos);
                    }
                }
            }
        }

        return null;
    }

    public Vector2 worldToGrid(Vector3 worldPosition) {
        float x = (int)Mathf.Floor((worldPosition.x - startPosition.x) / cellSize);
        float y = (int)Mathf.Floor((startPosition.z - worldPosition.z) / cellSize);

        return new Vector2(x, y);
    }

    public Vector3 gridToWorld(Vector2 gridPosition) {
        float x = startPosition.x + ((gridPosition.x + 0.5f) * cellSize);
        float z = startPosition.z - ((gridPosition.y + 0.5f) * cellSize);

        return new Vector3(x, startPosition.y, z);
    }

    void OnDrawGizmos() {
        if (isDrawGizmos) {
            foreach(GridCell cell in cells) {
                Vector2 gridPos = cell.getPosition();
                Vector3 worldPos = gridToWorld(gridPos);

                if (cell.getObject() == null) {
                    Gizmos.color = new Color(0, 0, 1, 0.5f);
                } else {
                    Gizmos.color = new Color(1, 0, 0, 0.5f);
                }

                float cubeSize = cellSize / 2;
                Gizmos.DrawCube(worldPos, new Vector3(cubeSize, cubeSize, cubeSize));
            }
        }
    }
}