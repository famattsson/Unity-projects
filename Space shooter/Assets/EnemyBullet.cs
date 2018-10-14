using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerShield")
        {
            collision.gameObject.GetComponent<PlayerStats>().Damage(damage);
        }
        Destroy(this.gameObject);
    }
}
