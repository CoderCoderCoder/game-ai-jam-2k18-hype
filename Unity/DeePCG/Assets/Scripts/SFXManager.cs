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

    public void PlayGameOver()
    {
        FXSource.Stop();
        FXSource.clip = gameOverFX;
        FXSource.Play();
    }

    public void PlayLifeUp()
    {
        FXSource.Stop();
        FXSource.clip = extraLifeFX;
        FXSource.Play();
    }
}
