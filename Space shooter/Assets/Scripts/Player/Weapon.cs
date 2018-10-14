using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    Transform firePoints;
    public GameObject particle;
    public AudioClip audioClip;

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform)
        {
            if(child.name.Contains("Fire"))
            {
                firePoints = child;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            Shoot();
        }
	}

    void Shoot()
    {
        foreach(Transform firepoint in firePoints)
        {
            if(firepoint.name.Contains("Fire"))
            {
                GetComponent<AudioSource>().PlayOneShot(audioClip, 0.2f);
                Instantiate(particle, firepoint.position, transform.rotation);
            }         
        }
    }
}
