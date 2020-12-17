using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_GameInstructions : MonoBehaviour {
    
    public GameObject instructions;

    public int count = 0;


    void Start() {
        instructions.SetActive(true);
    }

    void Update() {
        if (count > 750) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("S_Main");
        }

        count++;
    }
}
