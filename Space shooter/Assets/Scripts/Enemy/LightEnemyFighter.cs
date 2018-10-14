using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemyFighter : Enemy {

    static int count = 0;
    static public int countGetter { get { return count; } }

    protected override void Start()
    {
        base.Start();
        count++;
    }

    override public void Die(bool playSound, bool isOutOfBounds = false)
    {
        if (count > 0)
        {
            count--;
        }
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 0.5f);
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
