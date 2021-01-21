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
