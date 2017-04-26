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

    // Use this for initialization
    void Start()
    {

        //targetTransform = startingCameraTransform;

        mainCamera.enabled = true;
        startingCamera.enabled = false;
        leftCamera.enabled = false;
        rightCamera.enabled = false;
        clerkCamera.enabled = false;

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
        }

    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetTransform.rotation, rotationSpeed * Time.deltaTime);

        seek(targetTransform);
        mainCamera.transform.position += (velocity * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            targetTransform = startingCameraTransform;

            //disableAllCameras();
            //mainCamera.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            targetTransform = clerkCameraTransform;

            //disableAllCameras();
            //clerkCamera.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            targetTransform = leftCameraTransform;

            //disableAllCameras();
            //leftCamera.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            targetTransform = rightCameraTransform;

            //disableAllCameras();
            //rightCamera.enabled = true;
        }
    }
}