using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public List<Transform> firePoints;
    public GameObject particle;

	// Use this for initialization
	/*void Start () {
        foreach (var firePoint in GameObject.FindGameObjectsWithTag("FirePoint" + particle.name));
	}*/
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            Shoot();
        }
	}

    void Shoot()
    {
        foreach(var firepoint in firePoints)
        {
            Instantiate(particle, firepoint.position, transform.rotation);
        }
    }
}
