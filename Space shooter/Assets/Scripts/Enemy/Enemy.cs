using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

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
    public AudioClip shootSound; 

    [Header("Pathfinding")]
    public float moveSpeed = 0.5f;
    protected Seeker seeker;
    protected Path currentPath;
    protected Vector2 target;
    public float UpdateRate = 10f;
    protected int CurrentWayPoint = 1;
    protected bool pathIsEnded;

    protected virtual void Start ()
    {
        seeker = GetComponent<Seeker>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Base = GameObject.FindGameObjectWithTag("Base");
        fireTimer = 1 / firerate;
        rb = GetComponent<Rigidbody2D>();
        BaseArea = new Rect(Base.transform.position.x - 1, Base.transform.position.y - 1, 2, 2);
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Fire"))
            {
                firePoints = child;
            }
        }
        originalAngle = rb.rotation;

        Vector2 levelBottom = Camera.main.ViewportToWorldPoint(new Vector2(0, Camera.main.rect.yMin));

        target = new Vector2(transform.position.x, levelBottom.y);

        seeker.StartPath(transform.position, target, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath ()
    {
        seeker.StartPath(transform.position, target, OnPathComplete);

        yield return new WaitForSeconds(1 / UpdateRate);

        StartCoroutine(UpdatePath());
    }

    protected virtual void OnPathComplete(Path path)
    {
        currentPath = path;
        CurrentWayPoint = 1;
    }

    void FixedUpdate () {
        if(pathIsEnded)
        {
            rb.AddForce(new Vector2(0, -moveSpeed), ForceMode2D.Impulse);
        }
        else
        {
            Vector3 dir = new Vector3(0,0,0);
            if (currentPath != null)
            {
                dir = (currentPath.vectorPath[CurrentWayPoint] - transform.position).normalized;
                rb.AddForce(dir * moveSpeed, ForceMode2D.Impulse);

                if(Vector3.Distance(dir, currentPath.vectorPath[CurrentWayPoint]) < 2)
                {
                    CurrentWayPoint++;
                }
            }
        }

        fireTimer -= Time.fixedDeltaTime;
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
            GetComponent<AudioSource>().PlayOneShot(deathSound,0.8f);
        }
    }

    public void ApproachBase()
    {
        pathIsEnded = true;
        if (BaseArea.Contains(new Vector2(transform.position.x, transform.position.y)) && !hasdied)
        {
            hasdied = true;
            animator.SetTrigger("ApproachBase");
        }
        else if(!BaseArea.Contains(new Vector2(transform.position.x,transform.position.y)))
        {
            moveSpeed = 0.5f;
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y),
                new Vector2(Base.transform.position.x, Base.transform.position.y), 4*Time.deltaTime);
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
                    GetComponent<AudioSource>().PlayOneShot(shootSound, 0.2f);
                    Instantiate(bullet, firepoint.position, firepoint.rotation);
                }
            }
        }
    }
}
