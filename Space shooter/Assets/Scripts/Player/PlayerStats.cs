using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

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
        }
        else
        {
            health -= damage;
            if(health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        //TODO: Game Over
        Destroy(gameObject);
    }

	void Start () {
		
	}
	
}
