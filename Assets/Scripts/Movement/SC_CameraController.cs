using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;
using static EasingFunction;

public class SC_CameraController : MonoBehaviour
{
    public CinemachineVirtualCameraBase vCamBase;
    private CinemachineVirtualCamera vCam;

    // Camera settings
    public float defaultCameraFOV = 25.0f;
    public float runCameraFOV = 35.0f;

    public enum CameraAxis {
        x,
        y,
        z
    }


    void Start() {
        vCam = vCamBase.GetComponent<CinemachineVirtualCamera>();
        vCam.m_Lens.FieldOfView = defaultCameraFOV;
    }
    
    public void SetCharacterCamera(bool isRunning) {
        var easingType = Ease.EaseOutQuad;

        ChangeFOV((isRunning ? runCameraFOV : defaultCameraFOV), easingType, .05f);
    }

    public void OpenInventory() {
        var easingType = Ease.Spring;

        ChangeFOV(20.0f, easingType, .05f);
        ChangePosition(CameraAxis.z, 57.0f, Ease.EaseInBack, .02f);
    }

    public void CloseInventory() {
        var easingType = Ease.Spring;

        ChangeFOV(defaultCameraFOV, easingType, .05f);
        ChangePosition(CameraAxis.z, 27.5f, Ease.EaseInBack, .02f);
    }

    private void ChangeFOV(float targetValue, Ease easingType, float easingSpeed) {
        StartCoroutine(
            FloatEase( 
                (result => vCam.m_Lens.FieldOfView = result), 
                vCam.m_Lens.FieldOfView, 
                targetValue,
                easingType,
                easingSpeed
            )
        );
    }

    private void ChangePosition(CameraAxis axis, float targetValue, Ease easingType, float easingSpeed) {
        CinemachineTransposer vCamPosition = vCam.GetCinemachineComponent<CinemachineTransposer>();
        
        switch(axis) {
            case CameraAxis.x:
                StartCoroutine(
                    FloatEase( 
                        (result => vCamPosition.m_FollowOffset.x = result), 
                        vCamPosition.m_FollowOffset.x, 
                        targetValue,
                        easingType,
                        easingSpeed
                    )
                );
            break;
            
            case CameraAxis.y:
                StartCoroutine(
                    FloatEase( 
                        (result => vCamPosition.m_FollowOffset.y = result), 
                        vCamPosition.m_FollowOffset.y, 
                        targetValue,
                        easingType,
                        easingSpeed
                    )
                );
            break;

            case CameraAxis.z:
                StartCoroutine(
                    FloatEase( 
                        (result => vCamPosition.m_FollowOffset.z = result), 
                        vCamPosition.m_FollowOffset.z, 
                        targetValue,
                        easingType,
                        easingSpeed
                    )
                );
            break;
        }
    }

    IEnumerator FloatEase(Action<float> valueToChange, float from, float to, Ease easingType, float interpolationIncrement) {
        float interpolationRatio = 0.0f;
        float interpolationProgress = 0.0f;

        Function easingFunc = GetEasingFunction(easingType);


        while (interpolationProgress < 1.0f) {
            interpolationRatio = easingFunc(from, to, interpolationProgress);
            interpolationProgress += interpolationIncrement;
            
            valueToChange(interpolationRatio);

            yield return new WaitForEndOfFrame();
        }
    }
}
