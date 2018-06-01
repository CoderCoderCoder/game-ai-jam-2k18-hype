using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is possibly the lowest-quality script I've ever written in my life.
//But it's a game jam.
//Sue me.
public class SFXManager : MonoBehaviour
{
    public AudioClip warpFX;
    public AudioClip deathFX;
    public AudioClip killFX;
    public AudioClip mineFX;
    public AudioClip pickupFX;
    public AudioClip gameOverFX;
    public AudioClip extraLifeFX;

    public AudioSource FXSource;

    public void PlayWarp()
    {
        FXSource.Stop();
        FXSource.PlayOneShot(warpFX);
    }

    public void PlayDeath()
    {
        FXSource.Stop();
        FXSource.PlayOneShot(deathFX);
    }

    public void PlayKill()
    {
        FXSource.Stop();
        FXSource.PlayOneShot(killFX);
    }

    public void PlayMine()
    {
        FXSource.Stop();
        FXSource.PlayOneShot(mineFX);
    }

    public void PlayPickup()
    {
        FXSource.Stop();
        FXSource.PlayOneShot(pickupFX);
    }

    public void PlayGameOver()
    {
        FXSource.Stop();
        FXSource.PlayOneShot(gameOverFX);
    }

    public void PlayLifeUp()
    {
        FXSource.Stop();
        FXSource.PlayOneShot(extraLifeFX);
    }
}
