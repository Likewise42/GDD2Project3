using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeGMScript : MonoBehaviour
{

    public Camera mainCamera;
    public Camera startingCamera;
    public Camera leftCamera;
    public Camera rightCamera;
    public Camera clerkCamera;

    public Transform startingCameraTransform;
    public Transform leftCameraTransform;
    public Transform rightCameraTransform;
    public Transform clerkCameraTransform;

    public Transform targetTransform;

    //public Vector3 acceleration;
    public Vector3 velocity;

    //public Vector3 accelerationRot;
    //public Vector3 velocityRot;

    public float maxSpeed;
    public float maxForce;
    public float closeDist;

    public float rotationSpeed;
    
    // 1 is whole shop, 2 is Yetis, 3 is snowboards, 4 is guy
    private int whichView;

    //script for when the yeti shop script is done;
    public yetiShop shopScript;


    // Use this for initialization
    void Start()
    {



        //targetTransform = startingCameraTransform;

        mainCamera.enabled = true;
        startingCamera.enabled = false;
        leftCamera.enabled = false;
        rightCamera.enabled = false;
        clerkCamera.enabled = false;


        whichView = 1;
    }

    //void disableAllCameras()
    //{
    //    mainCamera.enabled = false;
    //    leftCamera.enabled = false;
    //    rightCamera.enabled = false;
    //    clerkCamera.enabled = false;
    //}

    //https://gamedev.stackexchange.com/questions/121469/unity3d-smooth-rotation-for-seek-steering-behavior
    void seek(Transform target)
    {
        Vector3 desired = target.position - mainCamera.transform.position;
        if (desired.sqrMagnitude > closeDist)
        {
            desired.Normalize();
            desired *= maxSpeed;

            Vector3 steer = desired - velocity;

            if (steer.sqrMagnitude > maxForce * maxForce)
            {
                steer.Normalize();
                steer *= maxForce;
            }

            velocity = steer;

            Debug.DrawLine(mainCamera.transform.position, mainCamera.transform.position + velocity);
        }
        else
        {
            velocity = new Vector3();

            mainCamera.transform.position = target.position;
        }

    }

    public void moveRightCam(string direction)
    {
        if (direction == "left")
        {
            Vector3 moveZ = new Vector3(0, 0, 2);

            rightCameraTransform.position += moveZ;
            
        }
        else if(direction == "right")
        {
            Vector3 moveZ = new Vector3(0, 0, 2);

            rightCameraTransform.position -= moveZ;
        }
    }

    public void moveLeftCam(string direction)
    {
        if (direction == "left")
        {
            Vector3 moveZ = new Vector3(0, 0, 2);

            leftCameraTransform.position -= moveZ;

        }
        else if (direction == "right")
        {
            Vector3 moveZ = new Vector3(0, 0, 2);

            leftCameraTransform.position += moveZ;
        }
    }

    void rightCameraControls()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && rightCameraTransform.position.z < -4)
        {
            moveRightCam("left");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && (rightCameraTransform.position.z > -12))
        {
            moveRightCam("right");
        }
    }

    void leftCameraControls()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && leftCameraTransform.position.z < -4)
        {
            moveLeftCam("right");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && (leftCameraTransform.position.z > -12))
        {
            moveLeftCam("left");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //slerp rotation
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetTransform.rotation, rotationSpeed * Time.deltaTime);

        //seek the target then updatethe position
        seek(targetTransform);
        mainCamera.transform.position += (velocity * Time.deltaTime);

        //switching camera
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            //starting
            targetTransform = startingCameraTransform;

            whichView = 1;

            shopScript.setAllViewsInactive();

            //disableAllCameras();
            //mainCamera.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            //yetis
            targetTransform = rightCameraTransform;

            whichView = 2;

            shopScript.setAllViewsInactive();

            //disableAllCameras();
            //clerkCamera.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            //snowbaords
            targetTransform = leftCameraTransform;

            whichView = 3;

            shopScript.setAllViewsInactive();

            //disableAllCameras();
            //leftCamera.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            targetTransform = clerkCameraTransform;

            whichView = 4;

            shopScript.setAllViewsInactive();

            //disableAllCameras();
            //rightCamera.enabled = true;
        }

        //decide which controls to use
        //if looking at yetis
        if(whichView == 2)
        {
            rightCameraControls();

            shopScript.setYetiView("Jim", 3, "DESC");
        }
        //else if looking at boards
        else if (whichView == 3)
        {
            leftCameraControls();

            shopScript.setBoardView("Rad", 3000000, "DESC32");
        }


    }
}