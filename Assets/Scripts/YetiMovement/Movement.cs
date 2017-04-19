using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private Vector3 acceleration;
    private Vector3 velocity;
    private bool aDown;
    private bool dDown;
    private bool wDown;
    private bool applyGrav;
    // the 'ground'
    private float yStart;

    private float lastJumpForce;


    // Use this for initialization
    void Start () {
        yStart = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {

        acceleration = Vector3.zero;

        // w key
        if (Input.GetKeyDown("w"))
        {
            acceleration += new Vector3(0, 1, 0);
            wDown = true;
            print("w down");
        }
        if (Input.GetKeyUp("w"))
        {
            acceleration -= new Vector3(0, 1, 0);
            wDown = false;
            print("w up");
        }

        // d key
        if (Input.GetKeyDown("d"))
        {
            acceleration += new Vector3(1, 0, 0);
            dDown = true;
            print("d down");
        }
        if (Input.GetKeyUp("d"))
        {
            acceleration -= new Vector3(1, 0, 0);
            dDown = false;
            print("d up");
        }

        // a key
        if (Input.GetKeyDown("a"))
        {
            acceleration += new Vector3(-1, 0, 0);
            aDown = true;
            print("a down");
        }
        if (Input.GetKeyUp("a"))
        {
            acceleration -= new Vector3(-1, 0, 0);
            aDown = false;
            print("a up");
        }

        // jumping
        if (transform.position.y > yStart) // if they are above the 'gound' apply gravity
        {
            applyGrav = true;
        }
        else // return the y velocity to 0
        {
            applyGrav = false;
            velocity.y += lastJumpForce;
            lastJumpForce = 0;
        }
        
        // creates gravity
        if(applyGrav)
        {
            acceleration.y -= 0.05f;
            lastJumpForce += 0.05f;
        }


        // changes velocity
        velocity += acceleration;
        
        // update position every frame
        transform.Translate((velocity * Time.deltaTime) * 15);

    }
}
