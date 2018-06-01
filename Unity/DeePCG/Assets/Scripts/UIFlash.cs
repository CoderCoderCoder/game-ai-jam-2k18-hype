using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlash : MonoBehaviour
{
    public Text target;
    public float timeOn = 1.0f;
    public float timeOff = 0.25f;

    private float timer = 0.0f;
    private bool on = true;

	void Update ()
    {
        timer += Time.deltaTime;

        if(on && timer >= timeOn)
        {
            timer = 0.0f;
            on = false;
            target.enabled = false;
        }
        else if(!on && timer >= timeOff)
        {
            timer = 0.0f;
            on = true;
            target.enabled = true;
        }
	}
}
