using System;
using System.Collections;
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
    private bool speeding;
    private float timer;
    private float oldMaxSpeed;
    private bool slowingDown;

	// Use this for initialization
	void Start () {
        speeding = false;
        slowingDown = false;

        //maxSpeed = 4.0f;
        camOriginal = cam.transform.position;
        sb = GameObject.FindGameObjectWithTag("Snowboard").GetComponent<Snowboard>();

        speed = sb.maxSpeed;

        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {

        this.transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f));

        speed += player.GetComponent<Movement>().boardBasedAcceleration;


        //maximum speed
        if (speed >= sb.maxSpeed)
        {
            speed = sb.maxSpeed;

            if (speeding && Math.Abs(timer - Time.time) > 5)
            {
                Deccelerate();
            }
        }

        Debug.Log(speed);

        if (slowingDown)
        {
            sb.maxSpeed *= .95f;

            if(sb.maxSpeed <= oldMaxSpeed)
            {
                sb.maxSpeed = oldMaxSpeed;
                slowingDown = false;
                oldMaxSpeed = 0;
            }
        }

        cam.transform.position = new Vector3(cam.transform.position.x, camOriginal.y + 7, camOriginal.z - 20);
	}

    public void Slow()
    {
        speed = speed * .95f;
    }

    public void SpeedBoost()
    {
        //only set old max speed if its not set
        if(oldMaxSpeed == 0)
        {
            oldMaxSpeed = sb.maxSpeed;
        }
        
        sb.maxSpeed *= 1.5f;
        timer = Time.time;
        speeding = true;
    }

    private void Deccelerate()
    {
        slowingDown = true;
        speeding = false;
    }
}
