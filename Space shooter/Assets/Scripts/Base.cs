using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Base : MonoBehaviour {

    private float health;
    private float maxHealth;
    public Text healthText;
    public Image healthBar;
    private bool hasDied;
    private GameManager gameManager;
    public ParticleSystem deathEffect;
    private Sprite defaultSprite;
    private Transform OriginalTransform;

	// Use this for initialization
	void Start () {
        maxHealth = 200;
        hasDied = false;
        health = maxHealth;
        UpdateHealth();
        defaultSprite = GetComponent<SpriteRenderer>().sprite;
        OriginalTransform = transform;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateHealth(float delta = 0)
    {
        health += delta;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.fillAmount = health / maxHealth;
        healthText.text = Math.Round(health, 2).ToString();
    }

    public void Damage(float damage)
    {
        if(!hasDied)
        {
            UpdateHealth(-damage);
            if (health <= 0)
            {
                StartDying();
            }
        }
    }

    public void StartDying()
    {
        hasDied = true;
        gameManager.PauseWinCondition();
        GetComponent<Animator>().ResetTrigger("Reset");
        GetComponent<Animator>().SetTrigger("Destroyed");
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<Spawner>().PauseSpawn();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().moveSpeed = 0;
            enemy.GetComponent<Enemy>().firerate = 0;          
        }

    }

    public void Reset()
    {
        GetComponent<Animator>().ResetTrigger("Reset");
        GetComponent<Animator>().SetTrigger("Reset");
        
        health = maxHealth;
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
        transform.SetPositionAndRotation(OriginalTransform.position, OriginalTransform.rotation);
        transform.localScale = OriginalTransform.localScale;
        UpdateHealth();
        hasDied = false;
    }
    public void StartDeathEffect()
    {
        Instantiate(deathEffect, transform.position, transform.rotation);
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().Die(false);

        }
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerGraphics>().StartDeathEffect();
    }

    public void playSound(AudioClip clip)
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(clip);
    }


    public void Die()
    {
        gameManager.GameOver();
    }
}
