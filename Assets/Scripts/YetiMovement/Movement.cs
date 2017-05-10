using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private Vector3 acceleration;
    private Vector3 velocity;
    private bool applyGrav;
    private bool jump;
    private bool left;
    private bool right;

    private float yStart;
    private float xStart;
    private float zStart;
    public float boundaryLength;
    private float sideSpeed;
    public GameObject world;

    private float rotation;

    public LevelManager lManager;

    // A little janky to have this in player, but. . .
    public int coldCashMultiplier = 1;
    public float coldCashBonusTimer = 0;
    public const int COLD_CASH_BONUS_DURATION = 900; // 15 seconds

    private float lastJumpForce;

    private bool sideSpeedBoard;
    private bool magnetSeekBoard;
    private bool accelerationBoard;
    private bool coinValueBoard;

    private bool airborne = false;

    private Snowboard sb;
    private bool collidingWithRamp;
    private bool collidingWithObstacle;
    private bool justLeftRamp;
    private bool resetRampJumping;
    private bool finishedSpinningOut;
    private bool lockMovement;

    // Use this for initialization
    void Start()
    {
        yStart = transform.position.y;
        xStart = transform.position.x;
        zStart = transform.position.z;

        collidingWithRamp = false;
        justLeftRamp = false;
        jump = false;
        left = false;
        right = false;
        finishedSpinningOut = true;
        lockMovement = false;

        // Which board do I have?
        sb = gameObject.GetComponentInChildren<Snowboard>();
        sideSpeed = sb.sideSpeed;

        sideSpeedBoard = sb.sideSpeedBoard; // implemented
        magnetSeekBoard = sb.magnetSeekBoard; // not implemented
        accelerationBoard = sb.accelerationBoard; // not implemented
        coinValueBoard = sb.coinValueBoard; // implemented

        rotation = 0;

        if (sideSpeedBoard)
        {
            sideSpeed = 4;
        }

    }

    // Update is called once per frame
    void Update()
    {
        acceleration = Vector3.zero;


        // d key
        if (Input.GetKeyDown("d"))
        {
            acceleration += new Vector3(sideSpeed, 0, 0);
            right = true;
        }
        if (Input.GetKeyUp("d"))
        {
            acceleration -= new Vector3(sideSpeed, 0, 0);
            right = false;
        }

        // a key
        if (Input.GetKeyDown("a"))
        {
            acceleration += new Vector3(-sideSpeed, 0, 0);
            left = true;
        }
        if (Input.GetKeyUp("a"))
        {
            acceleration -= new Vector3(-sideSpeed, 0, 0);
            left = false;
        }


        // jumping
        if (transform.position.y > yStart) // if they are above the 'gound' apply gravity
        {
            applyGrav = true;
        }
        else // return the y velocity to 0
        {
            if (airborne)
            {
                lManager.endAirScore();
                airborne = false;
            }
            resetRampJumping = true;
            applyGrav = false;
            transform.position = new Vector3(transform.position.x, yStart, transform.position.z);
            velocity.y = 0f;
            acceleration.y = 0f;
        }

        // applies gravity
        if (applyGrav)
        {
            acceleration.y -= 0.05f;
        }
        

        // ramp collision resolution
        if (collidingWithRamp)
        {
            transform.Translate(new Vector3(0, 0.5f, 0));
            jump = true;
        }
        else if(!collidingWithRamp && justLeftRamp && resetRampJumping)
        {
            lManager.startAirScore();
            airborne = true;
            acceleration.y += world.GetComponent<WorldSpin>().speed / 9f + (transform.position.y - yStart) / 9f;
            justLeftRamp = false;
            jump = false;
            resetRampJumping = false;
        }

        // obstacle slowing
        if (!finishedSpinningOut)
        {
            lockMovement = true;
            world.GetComponent<WorldSpin>().Slow();
            if (rotation >= 680)
            {
                rotation = 0;
                finishedSpinningOut = true;
                lockMovement = false;
            }
            else
            {
                rotation += 11f;
            }
            gameObject.transform.Rotate(new Vector3(0, rotation * Time.deltaTime, 0), Space.Self);
        }
        else
        {
            gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }


        // changes velocity
        velocity += acceleration;

        // to lock horizontal movement when you hit a rock
        if (!lockMovement)
        {
            // update position every frame
            transform.Translate((velocity * Time.deltaTime) * 15, Space.World);
        }
        else
        {
            Vector3 tempVerticleMovementVec = new Vector3(0, velocity.y, 0); // to keep the player moving vertically while locked horizontally
            transform.Translate((tempVerticleMovementVec * Time.deltaTime) * 15, Space.World);
        }



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
        
        gameObject.GetComponent<Animator>().SetBool("Jumping", jump);
        gameObject.GetComponent<Animator>().SetBool("Left", left);
        gameObject.GetComponent<Animator>().SetBool("Right", right);
    }
    
    void ExitCollideWithObstacle()
    {
        collidingWithObstacle = false;
    }
    
    void ExitCollideWithRamp()
    {
        collidingWithRamp = false;
        justLeftRamp = true;
    }

    void CollideWithColdCash()
    {
        if(coinValueBoard)
        {
            lManager.addColdCash(2);
        }
        lManager.addColdCash(1);
        if(!this.GetComponent<AudioSource>().isPlaying)
        {
            this.GetComponent<AudioSource>().Play();
        }
        else
        {
            this.GetComponent<AudioSource>().Stop();
            this.GetComponent<AudioSource>().Play();
        }
    }

    void CollideWithCashBonus()
    {
        coldCashMultiplier = 2;
        coldCashBonusTimer = COLD_CASH_BONUS_DURATION;
    }

    void CollideWithLevelEnd()
    {
        lManager.EndLevel();
        world.GetComponent<AudioSource>().Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject otherObj = other.gameObject;
        switch (otherObj.tag)
        {
            case "Obstacle":
                finishedSpinningOut = false;
                break;
            case "Ramp":
                collidingWithRamp = true;
                break;
            case "LevelEnd":
                lManager.EndLevel();
                break;
            case "ColdCash":
                CollideWithColdCash();
                break;
            case "SlalomFlags":
                lManager.hitSlalom = true;
                otherObj.GetComponentInParent<SlalomObstacle>().Success();
                break;
            case "SlalomCheckpoint":
                lManager.procSlalom();
                break;
            case "Pickup_Boost":
                // Boost logic goes here - remember to add some sort of glow(?) to the player to indicate they have a powerup
                break;
            case "Pickup_CashBonus":
                CollideWithCashBonus();
                break;
            case "Pickup_Multiplier":
                lManager.scoreMultiplier += 0.25f;
                break;
            default:
                break;
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
 