using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : Enemy {

    static int count = 0;
    static public int countGetter { get { return count; }} 

    void Start()
    {
        count++;
        fireTimer = 1 / firerate;
        rb = GetComponent<Rigidbody2D>();

        foreach (Transform child in transform)
        {
            if (child.name.Contains("Fire"))
            {
                firePoints = child;
            }
        }
        originalAngle = rb.rotation;
    }

    override protected void Die()
    {
        if (count > 0)
        {
            count--;
        }
        Destroy(gameObject, 0.1f);
    }

    override public void Damage(float damage)
    {
        Debug.Log(damage);
        Debug.Log(health);
        health -= damage;
        if (health == 0 && !hasdied)
        {
            Die();
        }
    }
}
