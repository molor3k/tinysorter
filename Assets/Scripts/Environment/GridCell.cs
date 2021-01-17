using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell {
    Vector2 cellPosition;
    GameObject cellObject;

    public GridCell(Vector2 pos, GameObject obj) {
        cellPosition = pos;
        cellObject = obj;
    }

    public void setObject(GameObject obj) {
        cellObject = obj;
    }

    public GameObject getObject() {
        return cellObject;
    }

    public Vector2 getPosition() {
        return cellPosition;
    }

    public void Clear() {
        cellObject = null;
    }
}