using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    public float speed = 10.0f;
    public Rigidbody2D rb;
    public float lifetime = 1.0f;
    private float lifeTimer = 0.0f;

	void Update ()
    {
        lifeTimer += Time.deltaTime;
        if(lifeTimer >= lifetime)
        {
            DestroyImmediate(gameObject);
        }
	}

    public void Launch(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        float angle = Vector3.SignedAngle(Vector3.left, direction, Vector3.forward);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.velocity = speed * direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.GetComponent<Tilemap>())
        {
            Vector3 hitPos = Vector3.zero;
            CaveGenerator cave = FindObjectOfType<CaveGenerator>();

            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPos.x = hit.point.x - 0.01f * hit.normal.x;
                hitPos.y = hit.point.y - 0.01f * hit.normal.y;
                cave.TryDestroyTile(hitPos);
            }

            Destroy(gameObject);
        }
    }
}
