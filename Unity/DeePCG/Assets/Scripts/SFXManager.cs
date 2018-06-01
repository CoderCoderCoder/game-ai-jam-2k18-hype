using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip warpFX;
    public AudioClip deathFX;
    public AudioClip killFX;
    public AudioClip mineFX;
    public AudioClip pickupFX;

    public AudioSource FXSource;

    public void PlayWarp()
    {
        FXSource.Stop();
        FXSource.clip = warpFX;
        FXSource.Play();
    }

    public void PlayDeath()
    {
        FXSource.Stop();
        FXSource.clip = deathFX;
        FXSource.Play();
    }

    public void PlayKill()
    {
        FXSource.Stop();
        FXSource.clip = killFX;
        FXSource.Play();
    }

    public void PlayMine()
    {
        FXSource.Stop();
        FXSource.clip = mineFX;
        FXSource.Play();
    }

    public void PlayPickup()
    {
        FXSource.Stop();
        FXSource.clip = pickupFX;
        FXSource.Play();
    }
}
