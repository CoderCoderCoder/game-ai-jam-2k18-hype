using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.0f;

    private SpriteRenderer sprite;
    private Animator anim;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate ()
    {
        Vector3 motionAmount = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        motionAmount = moveSpeed * Time.deltaTime * motionAmount.normalized;

        if (motionAmount.x > 0)
            sprite.flipX = true;
        else if (motionAmount.x < 0)
            sprite.flipX = false;

        transform.position += motionAmount;
	}
}
