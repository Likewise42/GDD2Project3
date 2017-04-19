using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpin : MonoBehaviour {

    public GameObject player;
    private float speed;
    private float maxSpeed;

	// Use this for initialization
	void Start () {
        speed = 0.0f;
        maxSpeed = 4.0f;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f));
        speed += .02f;

        //maximum speed
        if (speed >= maxSpeed)
        {
            speed = maxSpeed;
        }
	}
}
