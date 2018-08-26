using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {

    public float RotateSpeed = 0.05f;
    public float Radius = 7f;

    public Transform centreTransform;
    private Vector2 centre;
    private float _angle;

    private void Start()
    {
        centre = transform.position;
    }

    private void Update()
    {

        _angle += RotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        transform.position = centre + offset;
    }

}
