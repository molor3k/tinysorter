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
using TMPro;

public class SC_Timer : MonoBehaviour {
    
    public GameObject gameOverMenu;
    public float time = 1200;
    public SC_Inventory inventory;
    
    private TMP_Text timerText;
    private TMP_Text pointsText;
    

    void Start() {
        GameObject player = GameObject.Find("Player");
        inventory = player.GetComponent<SC_Inventory>();

        timerText = GameObject.Find("Timer").transform.GetChild(3).GetComponent<TMP_Text>();
        pointsText = GameObject.Find("Points").transform.GetChild(3).GetComponent<TMP_Text>();

        StartCoundownTimer();
    }

    void Update() {
        pointsText.text = inventory.pointsForSorting.ToString();
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    void StartCoundownTimer() {
        if (timerText != null) {
            time = 720;
            timerText.text = "12:00";
            InvokeRepeating("UpdateTimer", 0.0f, 0.01667f);
        }
    }
    
    void UpdateTimer() {
        if (timerText != null) {
            if(time > 0) {
                time -= Time.deltaTime;
                string minutes = Mathf.Floor(time / 60).ToString("00");
                string seconds = (time % 60).ToString("00");
                timerText.text = minutes + ":" + seconds;

            } else {
                time = 0;
                string minutes = 0.ToString("00");
                string seconds = 0.ToString("00");
                timerText.text = minutes + ":" + seconds;

                Time.timeScale = 0f;
                gameOverMenu.SetActive(true);
                gameOverMenu.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text = pointsText.text;
                //UnityEngine.SceneManagement.SceneManager.LoadScene("S_GameOver");
            }
        }
    }
}