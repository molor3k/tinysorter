using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_Timer : MonoBehaviour {
    
    public float time = 1200;
    
    private TMP_Text timerText;
    

    void Start() {
        timerText = GameObject.Find("Timer").transform.GetChild(0).GetComponent<TMP_Text>();
        
        StartCoundownTimer();
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    void StartCoundownTimer() {
        if (timerText != null) {
            time = 720;
            timerText.text = "Time Left: 12:00";
            InvokeRepeating("UpdateTimer", 0.0f, 0.01667f);
        }
    }
    
    void UpdateTimer() {
        if (timerText != null) {
            if(time > 0) {
                time -= Time.deltaTime;
                string minutes = Mathf.Floor(time / 60).ToString("00");
                string seconds = (time % 60).ToString("00");
                timerText.text = "Time Left: " + minutes + ":" + seconds;

            } else {
                time = 0;
                string minutes = 0.ToString("00");
                string seconds = 0.ToString("00");
                timerText.text = "Time Left: " + minutes + ":" + seconds;

                UnityEngine.SceneManagement.SceneManager.LoadScene("S_GameOver");
            }
        }
    }
}