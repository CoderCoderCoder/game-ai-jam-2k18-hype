using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHazard : MonoBehaviour {

    public float swimSpeed = 4.0f;
    public SpriteRenderer sprite;

    private Vector3 direction = Vector3.left;

	void Update () {
        transform.position += direction * swimSpeed * Time.deltaTime;
	}

    private void OnTriggerEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Projectile>())
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.GetComponent<PlayerController>() == null)
        {
            direction = -direction;
            sprite.flipX = !sprite.flipX;
        }      
    }
}
