using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_GameInstructions : MonoBehaviour {
    
    public int count = 0;


    void Start() {
        gameObject.SetActive(true);
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Time.timeScale = 0;
    }

    void Update() {
        if (count > 0) {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        count++;
    }
}
