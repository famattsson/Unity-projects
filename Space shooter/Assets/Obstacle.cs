using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public float health;
    public GameObject deathEffect;
    public AudioClip deathSound;
    public float repulsionForce; 
    private bool hasDied = false;

    void Start()
    {
        hasDied = false;
    }

    public void Damage(float damage)
    {
        health -= damage;
        if(health <= 0 && !hasDied)
        {
            Die();
        }
    }

    void Die()
    {
        hasDied = true;
        Instantiate(deathEffect, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<AudioSource>().PlayOneShot(deathSound,0.3f);
        Destroy(gameObject, 1f);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
        if(collision.gameObject.tag == "Enemy")
        {

            Debug.Log("repulsed");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * repulsionForce, ForceMode2D.Force);
        }
    }
}
