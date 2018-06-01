using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHazard : MonoBehaviour {

    public float swimSpeed = 4.0f;
    public ParticleSystem deathFX;
    public SpriteRenderer sprite;
    private bool dead = false;
    private Vector3 direction = Vector3.right;

	void Update ()
    {
        if(!dead)
            transform.position += direction * swimSpeed * Time.deltaTime;      
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>())
        {
            gameObject.tag = "Untagged";
            Destroy(gameObject, 1.0f);
            sprite.enabled = false;
            dead = true;
            deathFX.Play();
        }
        else if (collision.gameObject.GetComponent<PlayerController>() == null)
        {
            direction = -direction;
            sprite.flipX = !sprite.flipX;
        }
    }
}
