using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    bool isMoving = false;
    public Rigidbody2D rb;
    public float speed = 20;
    public float rotSpeed = 2;
    public static bool isGrounded = true;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            isMoving = true;
        }
        if(Input.GetButtonUp("Fire1"))
        {
            isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        if(isMoving)
        {
            if (isGrounded)
                rb.AddForce(transform.right * speed * Time.fixedDeltaTime * 100f, ForceMode2D.Force);
            else
                rb.AddTorque(rotSpeed * rotSpeed * Time.fixedDeltaTime * 100f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
