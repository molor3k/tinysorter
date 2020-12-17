using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_GameOver : MonoBehaviour
{
    public GameObject GameOver;
    public TMP_Text scoreText;

    private SC_Timer timer;
    private SC_Inventory inventory;


    void Start()
    {
        GameOverButton();
        timer = GameObject.Find("GameManager").GetComponent<SC_Timer>();
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>();
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("Player"));
    }

    void Update() {
        showgameOverWindow();
    }

    private void showgameOverWindow() {
        if(timer.time == 0) {
            scoreText.text = "Your score is: " + inventory.pointsForSorting;
        }
    }

    public void PlayAgainButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("S_Main");
    }

    public void MainMenuButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("S_MainMenu");
    }

    public void GameOverButton()
    {
        GameOver.SetActive(true);
    }
}
