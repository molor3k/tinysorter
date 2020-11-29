using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_DayNightCycle : MonoBehaviour {

    void Update() {
        transform.RotateAround(Vector3.zero, Vector3.right, Time.deltaTime / 2f);
        transform.LookAt(Vector3.zero);
    }
}
