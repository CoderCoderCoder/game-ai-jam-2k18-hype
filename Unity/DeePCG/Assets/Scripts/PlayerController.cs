using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.0f;

	void Update ()
    {
        Vector3 motionAmount = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        motionAmount = moveSpeed * Time.deltaTime * motionAmount.normalized;

        transform.position += motionAmount;
	}
}
