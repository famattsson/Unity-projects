using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollision2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
        }
    }
}
