using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Pause : MonoBehaviour {

    public GameObject pauseMenu;

    private bool paused = false;


    void Update() {
        if(Input.GetButtonDown("PauseButton")) {
            paused = togglePause();
        }
     }
     
    private void OnGUI() {
        if(paused) {
            pauseMenu.SetActive(true);
        } else {
            pauseMenu.SetActive(false);
        }
    }
     
    private bool togglePause() {
        if(Time.timeScale == 0f) {
            Time.timeScale = 1f;
            return false;
        } else {
            Time.timeScale = 0f;
            return true;    
        }
    }

    public void ResumeButton() {
        if(paused == true && Time.timeScale == 0f) {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            paused = false;
        }
    }

    public void QuitButton() {
        Application.Quit();
    }
}
