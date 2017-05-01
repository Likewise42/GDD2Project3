using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpin : MonoBehaviour {

    public GameObject player;
    public Camera cam;
    public float speed;
    public float maxSpeed;
    public float acceleration;
    private Vector3 camOriginal;

	// Use this for initialization
	void Start () {
        speed = 0.0f;
        //maxSpeed = 4.0f;
        camOriginal = cam.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f));
        speed += acceleration;

        //maximum speed
        if (speed >= maxSpeed)
        {
            speed = maxSpeed;
        }

        cam.transform.position = new Vector3(cam.transform.position.x, camOriginal.y + (speed / 2), camOriginal.z - (speed * 2));

        if (Input.GetKey("e"))
        {
            Slow();
        }
	}

    public void Slow()
    {
        speed = speed * .95f;
        speed -= 0.2f;
    }
}
