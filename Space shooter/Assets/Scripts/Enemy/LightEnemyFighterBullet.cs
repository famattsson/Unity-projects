﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemyFighterBullet : Bullet {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStats>().Damage(damage);
        }
        Destroy(this.gameObject);
    }
}
