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
    private bool jump;

    private float yStart;
    private float xStart;
    private float zStart;
    public float boundaryLength;
    public GameObject world;

    private float rotation;

    public LevelManager lManager;

    private float lastJumpForce;

    private float sideSpeed;
    private float jumpSpeed;


    private Snowboard sb;
    private bool collidingWithRamp;
    private bool collidingWithObstacle;
    private bool justLeftRamp;
    private bool finishedSpinningOut;


    // Use this for initialization
    void Start()
    {
        yStart = transform.position.y;
        xStart = transform.position.x;
        zStart = transform.position.z;
        //boundaryLength = 8f;

        collidingWithRamp = false;
        justLeftRamp = false;
        jump = false;
        finishedSpinningOut = true;

        sb = gameObject.GetComponentInChildren<Snowboard>();

        sideSpeed = sb.sideSpeed;
        jumpSpeed = sb.jumpSpeed;

        rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = Vector3.zero;

        // d key
        if (Input.GetKeyDown("d"))
        {
            acceleration += new Vector3(sideSpeed, 0, 0);
            dDown = true;
        }
        if (Input.GetKeyUp("d"))
        {
            acceleration -= new Vector3(sideSpeed, 0, 0);
            dDown = false;
        }

        // a key
        if (Input.GetKeyDown("a"))
        {
            acceleration += new Vector3(-sideSpeed, 0, 0);
            aDown = true;
        }
        if (Input.GetKeyUp("a"))
        {
            acceleration -= new Vector3(-sideSpeed, 0, 0);
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
            transform.position = new Vector3(transform.position.x, yStart, transform.position.z);
            velocity.y = 0f;
            acceleration.y = 0f;
        }

        // applies gravity
        if (applyGrav)
        {
            acceleration.y -= 0.05f;
            //lastJumpForce += 0.05f;
        }
        

        // ramp collision resolution
        if (collidingWithRamp)
        {
            Debug.Log("Colliding with ramp");
            transform.Translate(new Vector3(0, 0.5f, 0));
            jump = true;
        }
        else if(!collidingWithRamp && justLeftRamp)
        {
            print("just left ramp");
            acceleration.y += world.GetComponent<WorldSpin>().speed / 9f + (transform.position.y - yStart) / 9f;
            justLeftRamp = false;
            jump = false;
        }

        // obstacle slowing
        /*if(collidingWithObstacle)
        {
            world.GetComponent<WorldSpin>().Slow();
        }*/
        if (!finishedSpinningOut)
        {
            world.GetComponent<WorldSpin>().Slow();
            rotation += 10f;
            if (rotation >= 720)
            {
                rotation = 0;
                finishedSpinningOut = true;
            }
            gameObject.transform.Rotate(new Vector3(0, rotation * Time.deltaTime, 0), Space.Self);
        }
        else
        {
            gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        

        // changes velocity
        velocity += acceleration;

        // update position every frame
        transform.Translate((velocity * Time.deltaTime) * 15, Space.World);

        // left and right boundaries
        if (transform.position.x > xStart + boundaryLength)
        {
            Vector3 overflowVec = new Vector3(-1 * (transform.position.x - (xStart + boundaryLength)), 0, 0);
            transform.Translate(overflowVec, Space.World);
        }
        else if (transform.position.x < xStart - boundaryLength)
        {
            Vector3 overflowVec = new Vector3(-1 * (transform.position.x + (xStart + boundaryLength)), 0, 0);
            transform.Translate(overflowVec, Space.World);
        }


        //collisionDetection();
        
        gameObject.GetComponent<Animator>().SetBool("Jumping", jump);
    }

    void CollideWithObstacle()
    {
        finishedSpinningOut = false;
    }
    void ExitCollideWithObstacle()
    {
        Debug.Log("Exiting ramp");
        collidingWithObstacle = false;
    }

    void CollideWithRamp()
    {
        Debug.Log("Hit a ramp");
        collidingWithRamp = true;
    }

    void ExitCollideWithRamp()
    {
        Debug.Log("Exiting ramp");
        collidingWithRamp = false;
        justLeftRamp = true;
    }

    void CollideWithColdCash()
    {
        lManager.addColdCash(1);
    }
    
    void CollideWithLevelEnd()
    {
        lManager.EndLevel();

    }

    void OnTriggerEnter(Collider other)
    {
        GameObject otherObj = other.gameObject;
        if (otherObj.CompareTag("Obstacle"))
        {
            CollideWithObstacle();
        }
        else if (otherObj.CompareTag("Ramp"))
        {
            CollideWithRamp();
        }
        else if (otherObj.CompareTag("LevelEnd"))
        {
            CollideWithLevelEnd();
        }
        else if (otherObj.CompareTag("ColdCash"))
        {
            CollideWithColdCash();
        }
        else if (otherObj.CompareTag("SlalomFlags"))
        {
            lManager.hitSlalom = true;
        }
        else if (otherObj.CompareTag("SlalomCheckpoint"))
        {
            lManager.procSlalom();
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject otherObj = other.gameObject;
        if (otherObj.CompareTag("Ramp"))
        {
            ExitCollideWithRamp();
        }
        if (otherObj.CompareTag("Obstacle"))
        {
            ExitCollideWithObstacle();
        }
    }

}
 