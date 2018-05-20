using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float moveSpeed = 0.5f;
    public GameObject bullet;
    protected Transform firePoints;
    protected float fireTimer;
    protected Rigidbody2D rb;
    protected float originalAngle = 0;
    public float health = 10;
    public float firerate = 2f;
    protected bool hasdied = false;

	void FixedUpdate () {
        fireTimer -= Time.fixedDeltaTime;
        rb.AddForce(new Vector2(0, -moveSpeed), ForceMode2D.Impulse);
        if (fireTimer <= 0)
        {
            Shoot();
        }
        rb.MoveRotation(Mathf.LerpAngle(originalAngle, rb.rotation, 0.01f * Time.fixedDeltaTime));

        if (Camera.main.WorldToViewportPoint(transform.position).y < Camera.main.rect.yMin && !hasdied)
        {
            hasdied = true;
            Die();
        }
    }

    virtual protected void Die() { }

    virtual public void Damage(float damage) { }

    protected void Shoot ()
    {
        fireTimer = 1 / firerate;
        foreach (Transform firepoint in firePoints)
        {
            if(firepoint.name.Contains("Fire"))
            {
                Instantiate(bullet, firepoint.position, firepoint.rotation);
            }
        }
    }
}
