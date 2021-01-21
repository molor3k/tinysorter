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
using static SC_ItemTypes;

public class SC_SortingCan : MonoBehaviour {
    
    public ItemType sortingType;
    public Material highlightMaterial;

    private Mesh mesh;
    private Renderer render;
    private Vector2[] meshUV;


    void Start() {
        mesh = FindMesh();
        meshUV = GetUV(mesh);
        render = transform.GetChild(0).GetComponent<Renderer>();

        ChangeColor();
    }

    public void OutlineCan(bool isSelected) {
        Material[] materialsList = render.materials;

        if (isSelected) {
            materialsList[1] = highlightMaterial;
        } else {
            materialsList[1] = null;
        }
        
        render.materials = materialsList;
    }

    void ChangeColor() {
        switch(sortingType) {
            case ItemType.Plastic:
                MoveUV(mesh, new Vector2(0.06f, 0.0f));
            break;

            case ItemType.Paper:
                MoveUV(mesh, new Vector2(0.0f, 0.0f));
            break;

            case ItemType.Metal:
                MoveUV(mesh, new Vector2(0.19f, 0.0f));
            break;

            case ItemType.Organic:
                MoveUV(mesh, new Vector2(0.22f, 0.0f));
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
