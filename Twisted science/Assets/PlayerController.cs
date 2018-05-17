﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 10.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetButtonDown("Jump"))
        {
            PlayerMotor.Jump();
        }
        if (Input.GetButton("Horizontal"))
        {
            PlayerMotor.Move( new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0 ));
        }

    }
}
