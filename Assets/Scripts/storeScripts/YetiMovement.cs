using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiMovement : MonoBehaviour {

    private Vector3 rotation;
    private bool isWalking;
    private bool isRunning;
    private KeyCode run;
    private float speed;
    private float initalY;

	// Use this for initialization
	void Start () {
        rotation = new Vector3(0, 0, 0);
        run = KeyCode.LeftShift;
        initalY = gameObject.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
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

        //running check
        //must be walking to run
        if (Input.GetKey(run) && isWalking)
        {
            isRunning = true;
            speed = 4;
        }
        else
        {
            isRunning = false;
            speed = 2;
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

        //reset y
        Vector3 currentPos = gameObject.transform.position;
        currentPos.y = initalY;
        gameObject.transform.position = currentPos;

        //reset rotation
        Quaternion currentRot = gameObject.transform.rotation;
        currentRot.x = 0;
        currentRot.z = 0;
        gameObject.transform.rotation = currentRot;

        gameObject.GetComponent<Animator>().SetBool("Walking", isWalking);
        gameObject.GetComponent<Animator>().SetBool("Running", isRunning);
    }
}
