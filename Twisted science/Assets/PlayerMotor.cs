using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {
    [SerializeField]
    static public float jumpForce = 8;

    static private bool isInAir = false;
    static private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    static public void Move(Vector2 moveVector)
    {

        rb.AddForce(moveVector, ForceMode2D.Force);
    }

    static public void Jump()
    {
        if (isInAir == false)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isInAir = true;
        }
    }
    
    void OnCollisionEnter2D()
    {
        isInAir = false;
    }

}
