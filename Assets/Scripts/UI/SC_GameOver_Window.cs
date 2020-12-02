using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_GameOver_Window : MonoBehaviour {

    public SC_Timer timer;
    
    private TMP_Text scoreText;
    private SC_Inventory inventory;


    void Start() {
        scoreText = gameObject.GetComponent<TMP_Text>();
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>();
    }

    void Update() {
        showgameOverWindow();
    }

    private void showgameOverWindow() {
        if(timer.time == 0) {
            scoreText.text = "Your score is: " + inventory.pointsForSorting;
        }
    }
}
