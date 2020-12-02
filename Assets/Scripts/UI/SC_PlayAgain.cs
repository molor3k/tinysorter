using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SC_PlayAgain : MonoBehaviour {
    
    void Start() {
        gameObject.GetComponent<Button>().onClick.AddListener(RestartGame);
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
