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
using UnityEngine.SceneManagement;

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

    public void RestartButton() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("S_GameOver");
    }

    public void QuitButton() {
        Time.timeScale = 1f;

        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("Player"));
        SceneManager.LoadScene("S_MainMenu");
    }
}
