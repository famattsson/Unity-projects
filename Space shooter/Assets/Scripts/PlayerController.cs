using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    float moveSpeed = 2.7f;
    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetButton("Horizontal"))
        {
            rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed,0), ForceMode2D.Impulse);
        }
        if(Input.GetButton("Vertical"))
        {
            rb.AddForce(new Vector2(0, Input.GetAxis("Vertical") * moveSpeed), ForceMode2D.Impulse);
        }
	}
}
