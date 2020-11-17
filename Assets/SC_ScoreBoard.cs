using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_ScoreBoard : MonoBehaviour
{
    public TMP_Text scoreText;
    public SC_Timer timer;
    public SC_ItemDropHandler dropHandler;

    void Update()
    {
        printScoreBoard();
    }

    private void printScoreBoard()
    {
        if(timer.time == 0)
        {
            scoreText.text = "Your score is: " + dropHandler.pointsForSorting;
        }
    }
}
