using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour {

    Rigidbody2D rb;
    public float bulletSpeed = 10;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(0, bulletSpeed), ForceMode2D.Impulse);
    }

    void Update ()
    {
        if (transform.position.y > Camera.main.ViewportToWorldPoint(new Vector3(0, Camera.main.rect.yMax)).y)
        {
            Debug.Log("Destroyed");
            Destroy(this.gameObject,0.1f);
        }
    }
}
