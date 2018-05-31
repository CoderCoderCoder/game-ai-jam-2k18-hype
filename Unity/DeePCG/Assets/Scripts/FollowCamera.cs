using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject playerSprite;

	void Update ()
    {
        Vector3 camPosition = transform.position;
        Vector3 playerPosition = playerSprite.transform.position;

        camPosition.x = playerPosition.x;
        camPosition.y = playerPosition.y;

        transform.position = camPosition;
	}
}
