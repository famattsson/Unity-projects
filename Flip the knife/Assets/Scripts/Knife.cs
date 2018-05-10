using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour {

    public Rigidbody rb;

    private Vector3 startSwipe;
    private Vector3 endSwipe;

    public float force = 11.5f;
    public float torque = 235;

    private bool isInAir = false;

    private double immunityTime = 0;

    public AudioClip impactClip;
    public AudioClip throwClip;

    private AudioSource audioSource;

    public GameManager gm;
	// Use this for initialization
	void Start () {
        rb.isKinematic = true;
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0))
        {
            startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        if(Input.GetMouseButtonUp(0))
        {
            endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            if (isInAir == false)
            {
                Swipe();
            }

        }
        if (immunityTime > 0)
        {

            immunityTime -= Time.deltaTime;
            Debug.Log(immunityTime);
        }
        if (transform.position.y < -3)
        {
            gm.GameOver();
        }
    }

    void Swipe()
    {
        audioSource.clip = throwClip;
        audioSource.loop = true;
        audioSource.pitch = 1f;
        audioSource.Play();
        rb.isKinematic = false;
        Vector3 swipe = endSwipe - startSwipe;
        rb.AddForce(swipe * force, ForceMode.Impulse);
        rb.AddTorque(torque, 0f, 0f, ForceMode.Impulse);
        isInAir = true;
        immunityTime = 0.1;
    }
    
    void OnTriggerEnter()
    {
        audioSource.clip = impactClip;
        audioSource.loop = false;
        audioSource.pitch = 1.55f;
        audioSource.Play();
        rb.isKinematic = true;
        isInAir = false;
    }

    void OnCollisionEnter(Collision collision)
    {

        isInAir = false;
        if (collision.gameObject.tag == "Grass")
        {
            gm.GameOver();
            return;
        }
        if (rb.isKinematic == false && immunityTime <= 0)
        {
            audioSource.clip = impactClip;
            audioSource.loop = false;
            audioSource.pitch = 0.9f;
            audioSource.Play();
            gm.GameOver();
            return;
        }
    }

}
