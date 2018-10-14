using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour {

    public ParticleSystem deathEffect;
    public ParticleSystem LightJumpEffect;
    public AudioClip WinSound;
    public AudioClip DeathSound;
    private AudioSource audioSource;

    void Start ()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void StartDeathEffect()
    {
        Instantiate(deathEffect, transform);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void TriggerDying()
    {
        Debug.Log("Die triggered");
        GetComponentInParent<PlayerStats>().Die();
    }

    public void LightSpeedJump()
    {
        Instantiate(LightJumpEffect, transform.position, transform.rotation);
        foreach(var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().Die(false);
        }
        foreach(var bullet in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            Destroy(bullet);
        }
    }
    public void TriggerWin()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().WinMenu();
    }

    public void PlayLightJumpSound ()
    {
        audioSource.PlayOneShot(WinSound,1);
    }
    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(DeathSound,1);
    }
}
