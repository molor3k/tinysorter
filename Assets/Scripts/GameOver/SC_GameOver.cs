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
using UnityEngine.SceneManagement;
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
        /*GameOverButton();
        timer = GameObject.Find("GameManager").GetComponent<SC_Timer>();
        inventory = GameObject.Find("Player").GetComponent<SC_Inventory>();*/
        Destroy(GameObject.Find("GameManager"));
        Destroy(GameObject.Find("Player"));

        SceneManager.LoadScene("S_Main");
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
