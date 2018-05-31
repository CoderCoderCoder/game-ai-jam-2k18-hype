using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineHazard : MonoBehaviour {

    public ParticleSystem particles;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>() != null)
        {
            particles.Play();
            GetComponent<Renderer>().enabled = false;
            Destroy(gameObject, 1.0f);
        }  
    }
}
