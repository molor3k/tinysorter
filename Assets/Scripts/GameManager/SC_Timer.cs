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