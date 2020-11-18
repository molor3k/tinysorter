using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_ItemTypes;

public class SC_SortingCan : MonoBehaviour {
    public ItemType sortingType;

    private Mesh mesh;
    private Vector2[] meshUV;


    void Start() {
        mesh = FindMesh();
        meshUV = GetUV(mesh);

        ChangeColor();
    }

    void ChangeColor() {
        switch(sortingType) {
            case ItemType.Plastic:
                MoveUV(mesh, new Vector2(0.15f, 0.0f));
            break;

            case ItemType.Paper:
                MoveUV(mesh, new Vector2(0.0f, 0.0f));
            break;

            case ItemType.Metal:
                MoveUV(mesh, new Vector2(0.12f, 0.0f));
            break;

            case ItemType.Organic:
                MoveUV(mesh, new Vector2(0.06f, 0.0f));
            break;
        }
    }

    Mesh FindMesh() {
        MeshFilter meshComponent = transform.GetChild(0).GetComponent<MeshFilter>() as MeshFilter;
        return meshComponent.mesh;
    }

    Vector2[] GetUV(Mesh m) {
        return m.uv;
    }

    void MoveUV(Mesh mesh, Vector2 offset) {
        Vector2[] uv = new Vector2[mesh.vertices.Length];

        // Offset all the UV's 
        for (int i = 0; i < uv.Length; i++) {
            uv[i] = new Vector2(meshUV[i].x + offset.x, meshUV[i].y + offset.y);  
        }

        // Apply the modded UV's 
        mesh.uv = uv;   
    }
}
