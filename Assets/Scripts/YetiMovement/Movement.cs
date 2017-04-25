using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private Vector3 acceleration;
    private Vector3 velocity;
    private bool aDown;
    private bool dDown;
    private bool wDown;
    private bool applyGrav;
    private bool jump;

    private float yStart;
    private float xStart;
    public float boundaryLength;

    private float lastJumpForce;


    // Use this for initialization
    void Start()
    {
        yStart = transform.position.y;
        xStart = transform.position.x;
        //boundaryLength = 8f;

        jump = false;
    }

    // Update is called once per frame
    void Update()
    {

        acceleration = Vector3.zero;

        // w key
        if (Input.GetKeyDown("w"))
        {
            acceleration += new Vector3(0, 2, 0);
            wDown = true;
            jump = true;
        }
        if (Input.GetKeyUp("w"))
        {
            acceleration -= new Vector3(0, 2, 0);
            wDown = false;
            jump = false;
        }

        // d key
        if (Input.GetKeyDown("d"))
        {
            acceleration += new Vector3(1, 0, 0);
            dDown = true;
        }
        if (Input.GetKeyUp("d"))
        {
            acceleration -= new Vector3(1, 0, 0);
            dDown = false;
        }

        // a key
        if (Input.GetKeyDown("a"))
        {
            acceleration += new Vector3(-1, 0, 0);
            aDown = true;
        }
        if (Input.GetKeyUp("a"))
        {
            acceleration -= new Vector3(-1, 0, 0);
            aDown = false;
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
        if (applyGrav)
        {
            acceleration.y -= 0.05f;
            lastJumpForce += 0.05f;
        }


        // changes velocity
        velocity += acceleration;

        // update position every frame
        transform.Translate((velocity * Time.deltaTime) * 15);

        // left and right boundaries
        if (transform.position.x > xStart + boundaryLength)
        {
            Vector3 overflowVec = new Vector3(-1 * (transform.position.x - (xStart + boundaryLength)), 0, 0);
            transform.Translate(overflowVec);
        }
        else if (transform.position.x < xStart - boundaryLength)
        {
            Vector3 overflowVec = new Vector3(-1 * (transform.position.x + (xStart + boundaryLength)), 0, 0);
            transform.Translate(overflowVec);
        }


        //collisionDetection();
        
        gameObject.GetComponent<Animator>().SetBool("Jumping", jump);
    }

    void CollideWithObstacle()
    {
        Debug.Log("Hit an obstacle");
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject otherObj = other.gameObject;
        if (otherObj.CompareTag("Obstacle"))
        {
            CollideWithObstacle();
        }
    }
    
}