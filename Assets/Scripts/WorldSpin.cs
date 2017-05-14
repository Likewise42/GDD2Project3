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

	// Use this for initialization
	void Start () {
        
        //maxSpeed = 4.0f;
        camOriginal = cam.transform.position;
        sb = GameObject.FindGameObjectWithTag("Snowboard").GetComponent<Snowboard>();

        speed = sb.maxSpeed;
    }
	
	// Update is called once per frame
	void Update () {

        this.transform.Rotate(new Vector3(0f, speed * Time.deltaTime, 0f));

        speed += GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().boardBasedAcceleration;


        //maximum speed
        if (speed >= sb.maxSpeed)
        {
            speed = sb.maxSpeed;
        }

        cam.transform.position = new Vector3(cam.transform.position.x, camOriginal.y + 7, camOriginal.z - 20);
	}

    public void Slow()
    {
        speed = speed * .95f;
    }
}
