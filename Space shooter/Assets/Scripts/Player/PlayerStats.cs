using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    private GameManager gameManager;
    public GameObject graphics;

    [Header("GUI")]
    public Image healthbar;
    public Image shieldbar;
    public Text healthText;
    public Text shieldText;

    [Header("Stats")]
    public float maxHealth = 100f;
    private float health;
    public float maxShield = 50f;
    private float shield;
    public float healthRegen = 1f;
    public float shieldRegen = 4f;
    public float shieldRegenDelay = 2f;

    public Animator animator;
    private bool hasDied = false;
    private Vector2 originalPosition;
    private float shieldRegenTimer = 0f;
    
    
    public void FixedUpdate()
    {
        shieldRegenTimer += Time.fixedDeltaTime;

        if(shieldRegenTimer >= shieldRegenDelay && shield < maxShield)
        {
            UpdateShield(shieldRegen * 0.02f);
        }
        if(health < maxHealth)
        {
            UpdateHealth(healthRegen * 0.02f);
        }
    }

    private void UpdateHealth(float delta = 0)
    {
        health += delta;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthbar.fillAmount = health / maxHealth;
        healthText.text = Math.Round(health, 2).ToString();
    }

    private void UpdateShield(float delta = 0)
    {
        shield += delta;
        shield = Mathf.Clamp(shield, 0, maxShield);
        shieldbar.fillAmount = shield / maxShield;
        shieldText.text = Math.Round(shield, 2).ToString();
    }


    public void Update()
    {
        UpdateShield();
        UpdateHealth();
    }

    public void Damage(float damage)
    {
        if(shield > 0)
        {
            UpdateShield(-damage);
            shieldRegenTimer = 0f;
        }
        else
        {
            UpdateHealth(-damage);
            if(health <= 0 && !hasDied)
            {
                StartDying();
            }
        }
    }

    void StartDying()
    {
        Debug.Log("You have started dying");
        hasDied = true;
        TriggerAnim("Die");
    }

    public void Die()
    {
        Debug.Log("Died");
        SetActive(false);
        gameManager.GameOver();
    }

    private void SetActive(bool isActive)
    {
        if(isActive)
        {
            graphics.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<PlayerController>().enabled = true;
            gameObject.GetComponent<Weapon>().enabled = true;
        }
        else
        {
            graphics.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<PlayerController>().enabled = false;
            gameObject.GetComponent<Weapon>().enabled = false;
        }
    }

	void Start () {
        originalPosition = transform.position;
        health = maxHealth;
        shield = maxShield;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public Vector2 GetOriginalPos()
    {
        return originalPosition;
    }

    public void Reset()
    {
        SetActive(true);
        transform.position = originalPosition;
        health = maxHealth;
        shield = maxShield;
        hasDied = false;
        animator.SetTrigger("Reset");
    }

    public void TriggerAnim(string name)
    {
        animator.ResetTrigger("Reset");
        animator.SetTrigger(name);
    }
}
