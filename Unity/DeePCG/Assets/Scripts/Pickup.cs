using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public float oscillationTime = 1.0f;
    private float oscillationTimer = 0.0f;
    public float shiftAmount = 0.2f;
    private bool down = false;

    public int scoreAmount = 200;

    private void Start()
    {
        oscillationTimer = Random.Range(0.0f, oscillationTime);
    }

    void Update ()
    {
        oscillationTimer += Time.deltaTime;

        if(oscillationTimer > oscillationTime)
        {
            down = !down;
            oscillationTimer = 0.0f;
            transform.position += shiftAmount * (down ? Vector3.down : Vector3.up);
        }
	}
}
