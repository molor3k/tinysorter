using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_Timer : MonoBehaviour
{
    public GameObject gameOverWindow;
    public TMP_Text timerText;
    public AudioClip otherClip;
    public RuntimeAnimatorController anim;

    public float time = 1200;
    
    private bool playAudioAndStartAnimation = true;
    private bool stopAudioAndStartAnimation = true;
    
    void Start()
    {
        StartCoundownTimer();
    }
    
    void StartCoundownTimer()
    {
        if (timerText != null)
        {
            time = 720;
            timerText.text = "Time Left: 12:00";
            InvokeRepeating("UpdateTimer", 0.0f, 0.01667f);
        }
    }
    
    void UpdateTimer()
    {
        if (timerText != null)
        {
            if(time > 0)
            {
                time -= Time.deltaTime;
                string minutes = Mathf.Floor(time / 60).ToString("00");
                string seconds = (time % 60).ToString("00");
                timerText.text = "Time Left: " + minutes + ":" + seconds;

            } else
            {
                time = 0;
                string minutes = 0.ToString("00");
                string seconds = 0.ToString("00");
                timerText.text = "Time Left: " + minutes + ":" + seconds;

                gameOverWindow.transform.gameObject.SetActive(true);

                Time.timeScale = 0;

                if(stopAudioAndStartAnimation == true)
                {
                    Animator animator = this.gameObject.GetComponent<Animator>();
                    animator.runtimeAnimatorController = null as RuntimeAnimatorController;

                    AudioSource audio =  this.gameObject.GetComponent<AudioSource>();
                    audio.clip = null;
                    audio.Stop();

                    stopAudioAndStartAnimation = false;
                }
            }

            if(time <= 30)
            {
                if(playAudioAndStartAnimation == true)
                {
                    Animator animator = this.gameObject.GetComponent<Animator>();
                    animator.runtimeAnimatorController = anim as RuntimeAnimatorController;

                    AudioSource audio =  this.gameObject.GetComponent<AudioSource>();
                    audio.clip = otherClip;
                    audio.Play();

                    playAudioAndStartAnimation = false;
                }
            } 
        }
    }
}