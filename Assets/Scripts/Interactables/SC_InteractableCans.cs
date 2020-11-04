using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SC_InteractableCans : MonoBehaviour
{
    public KeyCode interactKey;
    public UnityEvent interactAction;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("You are colliding with Can");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("You are no longer colliding with Can");
        }
    }
}
