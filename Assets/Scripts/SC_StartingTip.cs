using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_StartingTip : MonoBehaviour
{
    private int i = 0;

    void Start()
    {
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Time.timeScale = 0;
    }

    void Update()
    {
        if(i >= 500)
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        i++;
    }
}
