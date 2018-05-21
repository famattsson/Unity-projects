using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {


    [Header("Text")]
    public Text healthText;
    public Text shieldsText;

    [Header("Stats")]
    public float health = 100f;
    public float shields = 50f;
    public float healthRegen = 0f;
    public float shieldRegen = 5f;
    public float shieldRegenDelay = 1f;

    public void Damage(float damage)
    {
        if(shields > 0)
        {
            shields -= damage;
            shieldsText.text = "Shields: " + shields;
        }
        else
        {
            health -= damage;
            if(health <= 0)
            {
                Die();
            }
            healthText.text = "Hull: " + health;
        }
    }

    void Die()
    {
        //TODO: Game Over
        Destroy(gameObject);
    }

	void Start () {
        healthText.text = "Hull: " + health;
        shieldsText.text = "Shields: " + shields;
    }

}
