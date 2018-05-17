using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Vector3 offset;
    public Transform player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        offset = new Vector3(0, 0.2f, 0);
        Debug.Log(transform.position);
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.5f);
        transform.position = smoothedPosition;
        Debug.Log(transform.position);
    }
}
