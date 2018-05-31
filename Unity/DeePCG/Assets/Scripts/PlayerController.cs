using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectile;

    public float weaponCool = 0.5f;
    private float weaponTimer = 0.0f;
    public float moveSpeed = 0.0f;
    public float deathTime = 2.0f;
    private float deathTimer = 0.0f;

    private SpriteRenderer sprite;
    private Animator anim;
    private Vector3 lastAim = Vector3.left;

    private bool cooling = false;
    private bool dead = false;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Respawn()
    {
        dead = false;
        deathTimer = 0.0f;

        transform.position = Vector3.zero;
        anim.ResetTrigger("Die");
        anim.Play("PlayerSwim");
    }

    void FixedUpdate ()
    {
        if(!dead)
        {
            Vector3 motionAmount = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (motionAmount == Vector3.zero)
                return;

            lastAim = motionAmount.normalized;
            motionAmount = moveSpeed * Time.deltaTime * motionAmount.normalized;

            if (motionAmount.x > 0)
                sprite.flipX = true;
            else if (motionAmount.x < 0)
                sprite.flipX = false;

            transform.position += motionAmount;
        } 
	}

    private void Update()
    {
        if(dead)
        {
            deathTimer += Time.deltaTime;

            if(deathTimer > deathTime)
                Respawn();
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space) && !cooling)
            {
                cooling = true;
                Projectile newProjectile = (Instantiate(projectile) as GameObject).GetComponent<Projectile>();
                newProjectile.Launch(transform.position + lastAim * 0.5f, lastAim);
                weaponTimer = 0.0f;
            }

            if(cooling)
            {
                weaponTimer += Time.deltaTime;

                if (weaponTimer >= weaponCool)
                    cooling = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            anim.SetTrigger("Die");
            dead = true;
            deathTimer = 0.0f;
        }
    }
}
