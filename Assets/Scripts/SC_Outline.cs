using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Outline : MonoBehaviour
{
    // Toggle between Diffuse and Transparent/Diffuse shaders
    // when space key is pressed

    Shader shader1;
    Shader shader2;
    Renderer rend;


    void Start()
    {
        rend = GetComponent<Renderer> ();
        shader1 = Shader.Find("Diffuse");
        shader2 = Shader.Find("Transparent/Diffuse");
    }

    void Update()
    {
        
            if (rend.material.shader == shader1)
            {
                rend.material.shader = shader2;
            }
            else
            {
                rend.material.shader = shader1;
            }
        
    }
}
