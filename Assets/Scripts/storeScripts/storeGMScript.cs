using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeGMScript : MonoBehaviour
{

    public Camera mainCamera;
    public Camera leftCamera;
    public Camera rightCamera;
    public Camera clerkCamera;

    // Use this for initialization
    void Start()
    {

        mainCamera.enabled = true;
        leftCamera.enabled = false;
        rightCamera.enabled = false;
        clerkCamera.enabled = false;

    }

    void disableAllCameras()
    {
        mainCamera.enabled = false;
        leftCamera.enabled = false;
        rightCamera.enabled = false;
        clerkCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            disableAllCameras();
            mainCamera.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            disableAllCameras();
            clerkCamera.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            disableAllCameras();
            leftCamera.enabled = true;
        } else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            disableAllCameras();
            rightCamera.enabled = true;
        }
    }
}