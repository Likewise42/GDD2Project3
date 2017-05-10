﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpin : MonoBehaviour {

    public GameObject player;
    public Camera cam;
    public float speed;
    private float maxSpeed;
    private float acceleration;
    private Vector3 camOriginal;
    private Snowboard sb;

	// Use this for initialization
	void Start () {
        speed = 0.0f;
        //maxSpeed = 4.0f;
        camOriginal = cam.transform.position;
        sb = GameObject.FindGameObjectWithTag("snowboard").GetComponent<Snowboard>();
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f));
        speed += sb.acceleration;

        //maximum speed
        if (speed >= sb.maxSpeed)
        {
            speed = sb.maxSpeed;
        }

        cam.transform.position = new Vector3(cam.transform.position.x, camOriginal.y + 7 /*(speed / 2)*/, camOriginal.z - 20 /*(speed * 2) */);
	}

    public void Slow()
    {
        speed = speed * .95f;
    }
}
