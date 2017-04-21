using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpin : MonoBehaviour {

    public GameObject player;
    public Camera cam;
    private float speed;
    private float maxSpeed;
    private float camOriginalZ;

	// Use this for initialization
	void Start () {
        speed = 0.0f;
        maxSpeed = 4.0f;
        camOriginalZ = cam.transform.position.z;
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

        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, camOriginalZ - (speed * 8));
	}

    public void Slow()
    {
        speed = speed / 4;
    }
}
