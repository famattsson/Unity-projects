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
    public float baseDamage = 10f;
    public float firerate = 2f;
    protected bool hasdied = false;
    public int spawnLimit;
    public int spawnDelay;
    public ParticleSystem deathEffect;
    protected bool isOutOfBounds = false;
    public Animator animator;
    protected GameObject Base;
    protected Rect BaseArea;
    protected GameManager gameManager;
    public AudioClip deathSound;

    void FixedUpdate () {
        fireTimer -= Time.fixedDeltaTime;
        rb.AddForce(new Vector2(0, -moveSpeed), ForceMode2D.Impulse);
        if (fireTimer <= 0)
        {
            Shoot();
        }
        rb.MoveRotation(Mathf.LerpAngle(originalAngle, rb.rotation, 0.01f * Time.fixedDeltaTime));

        if (Camera.main.WorldToViewportPoint(transform.position).y < Camera.main.rect.yMin)
        {
            isOutOfBounds = true;
            animator.SetTrigger("StartApproach");
        }
        if(isOutOfBounds)
        {
            ApproachBase();
        }
    }

    virtual public void Die(bool playSound, bool isOutOfBounds = false)
    {
        Debug.Log(playSound + " " + isOutOfBounds);
        Instantiate(deathEffect, transform.position, transform.rotation);
        if (isOutOfBounds) {
            Base.GetComponent<Base>().Damage(baseDamage);
        }
        else
        {
            gameManager.UpdateKilledEnemies();
        }

        if (playSound)
        {
            GetComponent<AudioSource>().PlayOneShot(deathSound);
        }
    }

    public void ApproachBase()
    {
        if (BaseArea.Contains(new Vector2(transform.position.x, transform.position.y)) && !hasdied)
        {
            hasdied = true;
            animator.SetTrigger("ApproachBase");
        }
        else if(!BaseArea.Contains(new Vector2(transform.position.x,transform.position.y)))
        {
            moveSpeed = 0.5f;
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(Base.transform.position.x, Base.transform.position.y), 4*Time.deltaTime);
        }

    }

    virtual public void Damage(float damage) { }

    protected void Shoot ()
    {
        if(!isOutOfBounds)
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
}
