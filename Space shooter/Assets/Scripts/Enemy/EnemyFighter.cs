using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighter : Enemy {

    static int count = 0;
    static public int countGetter { get { return count; }}


    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Base = GameObject.FindGameObjectWithTag("Base");
        count++;
        fireTimer = 1 / firerate;
        rb = GetComponent<Rigidbody2D>();
        BaseArea = new Rect(Base.transform.position.x-1, Base.transform.position.y-1, 2, 2);
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Fire"))
            {
                firePoints = child;
            }
        }
        originalAngle = rb.rotation;
    }

    override public void Die(bool playSound, bool isOutOfBounds = false)
    {
        if (count > 0)
        {
            count--;
        }
        Destroy(gameObject, 0.3f);
        base.Die(playSound,isOutOfBounds);
    }

    override public void Damage(float damage)
    {
        health -= damage;
        if (health == 0 && !hasdied)
        {
            Die(true);
        }
    }
}
