using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet: MonoBehaviour {

    Rigidbody2D rb;
    public float bulletSpeed = 10;
    float timetolive;
    public float damage = 5;

	// Use this for initialization
	void Awake () {
        timetolive = 15f;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * (rb.mass *  bulletSpeed), ForceMode2D.Impulse);
    }

    void Update ()
    {
        timetolive -= Time.deltaTime;
        if (!Camera.main.rect.Contains(Camera.main.WorldToViewportPoint(transform.position)) || timetolive <= 0)
        {
            Destroy(this.gameObject,0.1f);
        }
    }
}
