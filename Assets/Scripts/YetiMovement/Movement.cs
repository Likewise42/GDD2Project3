using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private Vector3 acceleration;
    private Vector3 velocity;
    private bool applyGrav;
    private bool left;
    private bool right;

    private float yStart;
    private float xStart;
    private float zStart;
    public float boundaryLength;
    private float sideSpeed;
    public GameObject world;

    public YetiGameData.YetiType yetiType;

    private float rotation;

    public LevelManager lManager;

    // A little janky to have this in player, but. . .
    public uint coldCashMultiplier;
    public float coldCashBonusTimer = 0;
    public const float COLD_CASH_BONUS_DURATION = LevelManager.PICKUP_SPAWN_INTERVAL - 5; // same as how long it takes to spawn a new one so they don't overlap

    private float lastJumpForce;

    public float boardBasedAcceleration;

    private bool airborne = false;

    private Snowboard sb;
    private bool collidingWithRamp;
    private bool collidingWithObstacle;
    private bool justLeftRamp;
    private bool resetRampJumping;
    private bool finishedSpinningOut;
    private bool lockMovement;

    // MODELS
    public GameObject YetiBoard;
    public GameObject CashBoard;
    public GameObject ATATBoard;
    public GameObject WampBoard;
    public GameObject NormalBoard;

    // Use this for initialization
    void Start()
    {
        yStart = transform.position.y;
        xStart = transform.position.x;
        zStart = transform.position.z;

        collidingWithRamp = false;
        justLeftRamp = false;
        left = false;
        right = false;
        lockMovement = false;
        finishedSpinningOut = true;
        

        rotation = 0;


        // reset everything to default values
        sideSpeed = 2;
        coldCashMultiplier = 5000;
        boardBasedAcceleration = 0.1f;
        YetiBoard.SetActive(false);
        CashBoard.SetActive(false);
        ATATBoard.SetActive(false);
        WampBoard.SetActive(false);
        NormalBoard.SetActive(false);
        
        switch (YetiGameData.SelectedBoard)
        {
            case (YetiGameData.BoardType.YetiBoard): // agile board
                sideSpeed = 4;
                YetiBoard.SetActive(true);
                break;
            case (YetiGameData.BoardType.CashBoard): // cash board
                coldCashMultiplier *= 2;
                CashBoard.SetActive(true);
                break;
            case (YetiGameData.BoardType.ATATBoard): // magnet board
                //coldCashMultiplier = 2;
                Debug.Log("Using the magnet board");
                ATATBoard.SetActive(true);
                break;
            case (YetiGameData.BoardType.WampBoard): // acceleration board
                boardBasedAcceleration = 0.2f;
                WampBoard.SetActive(true);
                break;
            case (YetiGameData.BoardType.NormalBoard): // default board
                NormalBoard.SetActive(true);
                break;
            default:
                NormalBoard.SetActive(true);
                break;
        } 

    }

    // Update is called once per frame
    void Update()
    {
        acceleration = Vector3.zero;


        // Handle cashbonus real quick, if necessary
        if (coldCashBonusTimer > 0)
        {
            coldCashBonusTimer -= 100 * Time.deltaTime;
            if (coldCashBonusTimer <= 0)
            {
                coldCashMultiplier -= 1;  // If pickup duration over, reset cold cash to normal values
            } 
        }

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
        }
        else if(!collidingWithRamp && justLeftRamp && resetRampJumping)
        {
            lManager.startAirScore();
            airborne = true;
            acceleration.y += world.GetComponent<WorldSpin>().speed / 9f + (transform.position.y - yStart) / 9f;
            justLeftRamp = false;
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

        lManager.addColdCash(coldCashMultiplier);

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
        coldCashMultiplier += 1;
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
                lManager.scoreMultiplier = 1;
                lManager.addScore(0);
                finishedSpinningOut = false;
                break;
            case "Ramp":
                collidingWithRamp = true;
                break;
            case "LevelEnd":
                CollideWithLevelEnd();
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
                lManager.addScore(0);
                // Debug.Log(lManager.scoreMultiplier);
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
 