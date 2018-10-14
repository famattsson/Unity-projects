using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet {
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Damage(damage);
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<Obstacle>().Damage(damage);
        }
        Destroy(this.gameObject);
    }
}
