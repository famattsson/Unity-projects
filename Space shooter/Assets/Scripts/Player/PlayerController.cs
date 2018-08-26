using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float originalAngle = 0;
    public float moveSpeed = 2.7f;

    public Animator animator;
    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        originalAngle = rb.rotation;
    }

    void Update ()
    {
        ConstrainPosition();
    }

	// Update is called once per frame
	void FixedUpdate () {
        Move();
        rb.MoveRotation(Mathf.LerpAngle(originalAngle, rb.rotation, 0.01f * Time.fixedDeltaTime) );
    }

    void ConstrainPosition ()
    {
        var viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
        var desiredPos = new Vector2(Mathf.Clamp(viewPortPos.x, Camera.main.rect.xMin, Camera.main.rect.xMax),
            Mathf.Clamp(viewPortPos.y, Camera.main.rect.yMin, Camera.main.rect.yMax));
        transform.position = (Vector2)Camera.main.ViewportToWorldPoint(desiredPos);
    }

    void Move ()
    {
        if(Input.GetButton("Horizontal")  || Input.GetButton("Vertical"))
        {
            if (Input.GetButton("Horizontal"))
            {
                rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0), ForceMode2D.Impulse);
                if(Input.GetAxis("Horizontal") < 0)
                {
                    triggerAnimBool("MovingLeft", true);
                    triggerAnimBool("MovingRight", false);
                }
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    triggerAnimBool("MovingLeft", false);
                    triggerAnimBool("MovingRight", true);
                }
            }
            if (Input.GetButton("Vertical"))
            {
                rb.AddForce(new Vector2(0, Input.GetAxis("Vertical") * moveSpeed), ForceMode2D.Impulse);
            }
            return;
        }

        triggerAnimBool("MovingLeft", false);
        triggerAnimBool("MovingRight", false);

    }

    void triggerAnimBool (string name, bool tof)
    {
        animator.SetBool(name, tof);
    }
}
