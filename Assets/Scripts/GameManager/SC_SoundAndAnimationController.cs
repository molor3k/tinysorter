using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_SoundAndAnimationController : MonoBehaviour
{
    public GameObject timerObject;
    public AudioClip otherClip;
    public RuntimeAnimatorController anim;

    private SC_Timer timer;

    private bool playAudioAndStartAnimation = true;
    private bool stopAudioAndStartAnimation = true;


    void Start() {        
        timer = gameObject.GetComponent<SC_Timer>();
    }

    void Update() {
        if(timer.time < 0) {
            if(stopAudioAndStartAnimation == true) {
                Animator animator = timerObject.GetComponent<Animator>();
                animator.runtimeAnimatorController = null as RuntimeAnimatorController;

                AudioSource audio = timerObject.GetComponent<AudioSource>();
                audio.clip = null;
                audio.Stop();

                stopAudioAndStartAnimation = false;
            }
        } else if(timer.time < 30 && timer.time > 0) {
                if(playAudioAndStartAnimation == true) {
                    Animator animator = timerObject.GetComponent<Animator>();
                    animator.runtimeAnimatorController = anim as RuntimeAnimatorController;

                    AudioSource audio = timerObject.GetComponent<AudioSource>();
                    audio.clip = otherClip;
                    audio.Play();

                    playAudioAndStartAnimation = false;
                }
        } 
    }
}
