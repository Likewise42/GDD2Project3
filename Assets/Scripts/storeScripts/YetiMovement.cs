using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiMovement : MonoBehaviour {

    private Vector3 rotation;
    private bool isWalking;
    private bool isRunning;
    private KeyCode run;
    private float speed;

	// Use this for initialization
	void Start () {
        rotation = new Vector3(0, 0, 0);
        run = KeyCode.LeftShift;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(run))
        {
            isRunning = true;
            speed = 4;
        }
        else
        {
            isRunning = false;
            speed = 2;
        }

        //forward
        if (Input.GetKey("w"))
        {
            isWalking = true;
            gameObject.transform.position += (gameObject.transform.forward * speed * Time.deltaTime);
        }
        //backward
        else if (Input.GetKey("s"))
        {
            isWalking = true;
            gameObject.transform.position += (gameObject.transform.forward * -speed * Time.deltaTime);
        }
        else
        {
            isWalking = false;
        }

        //rotate clockwise
        if (Input.GetKey("d"))
        {
            rotation = new Vector3(0, 5, 0);
            gameObject.transform.Rotate(rotation, Space.Self);
        }

        //rotate counterclockwise
        if (Input.GetKey("a"))
        {
            rotation = new Vector3(0, -5, 0);
            gameObject.transform.Rotate(rotation, Space.Self);
        }

        gameObject.GetComponent<Animator>().SetBool("Walking", isWalking);
        gameObject.GetComponent<Animator>().SetBool("Running", isRunning);
    }
}
