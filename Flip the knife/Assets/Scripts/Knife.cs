using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour {

    public Rigidbody rb;

    private Vector3 startSwipe;
    private Vector3 endSwipe;

    public float force = 11.5f;
    public float torque = 235;

	// Use this for initialization
	void Start () {
		
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
            Swipe();
        }
	}

    void Swipe()
    {
        rb.isKinematic = false;
        Vector3 swipe = endSwipe - startSwipe;
        rb.AddForce(swipe * force, ForceMode.Impulse);
        rb.AddTorque(torque, 0f, 0f, ForceMode.Impulse);
    }
    
    void OnTriggerEnter()
    {
        rb.isKinematic = true;
    }
}
