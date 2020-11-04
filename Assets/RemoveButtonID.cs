using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveButtonID : MonoBehaviour
{
    public int IDHolder;
    public int ID;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void checkButtonID()
    {
        IDHolder = ID;
        Debug.Log(IDHolder);
    }
}
