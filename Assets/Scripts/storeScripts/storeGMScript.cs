using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board
{
    public string name;
    public uint cost;
    public string desc;
    public GameObject boardObject;
    public YetiGameData.BoardType boardType;

    public board(string Name, uint Cost, string Desc, GameObject Board, YetiGameData.BoardType BoardType)
    {
        name = Name;
        cost = Cost;
        desc = Desc;
        boardObject = Board;
        boardType = BoardType;
    }
}

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

    public GameObject snowBoard1;
    public GameObject snowBoard2;
    public GameObject snowBoard3;
    public GameObject snowBoard4;
    public GameObject snowBoard5;

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

   // private ArrayList yetiArray;
    private board[] boardArray = new board[5];

    private int currentBoard;

    // Use this for initialization
    void Start()
    {

        currentBoard = 0;



        //targetTransform = startingCameraTransform;

        mainCamera.enabled = true;
        startingCamera.enabled = false;
        leftCamera.enabled = false;
        rightCamera.enabled = false;
        clerkCamera.enabled = false;

        

        //default board
        boardArray[0] = new board("Basic Board", 0, "A basic board to start you off.", snowBoard1, YetiGameData.BoardType.NormalBoard);

        //sideSpeed board
        boardArray[1] = new board("Agile Board", 50, "A board that handles well, allowing you to move side to side easier.", snowBoard2, YetiGameData.BoardType.YetiBoard);

        //accel board
        boardArray[2] = new board("Sleek Board", 100, "This board is slimmer and sleeker then the others, allowing you to reach your top speeds faster!", snowBoard3, YetiGameData.BoardType.WampBoard);

        //mag board
        boardArray[3] = new board("Magnetic Board", 300, "This board has a built in Cold Cash magnet!", snowBoard4, YetiGameData.BoardType.ATATBoard);

        //c val board
        boardArray[4] = new board("Money Board", 500, "A board with the innate ability to make Cold Cash more valuable.", snowBoard5, YetiGameData.BoardType.CashBoard);


        whichView = 1;
    }

    public void buyBoard()
    {

        if(YetiGameData.ColdCash >= boardArray[currentBoard].cost && !YetiGameData.boardBoughtArray[currentBoard])
        {
            YetiGameData.ColdCash -= boardArray[currentBoard].cost;

            YetiGameData.SelectedBoard = boardArray[currentBoard].boardType;

            YetiGameData.boardBoughtArray[currentBoard] = true;

            Debug.Log(YetiGameData.SelectedBoard);
        }else if (YetiGameData.boardBoughtArray[currentBoard])
        {
            YetiGameData.SelectedBoard = boardArray[currentBoard].boardType;

            Debug.Log(YetiGameData.SelectedBoard);
        }
        

    }

    //void disableAllCameras()
    //{
    //    mainCamera.enabled = false;
    //    leftCamera.enabled = false;
    //    rightCamera.enabled = false;
    //    clerkCamera.enabled = false;
    //}

    public void disableRotating()
    {
        boardArray[0].boardObject.GetComponent<RotateObjects>().rotating = false;
        boardArray[1].boardObject.GetComponent<RotateObjects>().rotating = false;
        boardArray[2].boardObject.GetComponent<RotateObjects>().rotating = false;
        boardArray[3].boardObject.GetComponent<RotateObjects>().rotating = false;
        boardArray[4].boardObject.GetComponent<RotateObjects>().rotating = false;
    }
        
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
        if (direction == "left" && rightCameraTransform.position.z < -4)
        {
            Vector3 moveZ = new Vector3(0, 0, 2);

            rightCameraTransform.position += moveZ;
            
        }
        else if(direction == "right" && rightCameraTransform.position.z > -12)
        {
            Vector3 moveZ = new Vector3(0, 0, 2);

            rightCameraTransform.position -= moveZ;
        }
    }

    public void moveLeftCam(string direction)
    {
        if (direction == "left" && leftCameraTransform.position.z > -12)
        {
            Vector3 moveZ = new Vector3(0, 0, 2);

            leftCameraTransform.position -= moveZ;

            currentBoard++;

        }
        else if (direction == "right" && leftCameraTransform.position.z < -4)
        {
            Vector3 moveZ = new Vector3(0, 0, 2);

            leftCameraTransform.position += moveZ;

            currentBoard--;
        }
    }

    void rightCameraControls()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveRightCam("left");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveRightCam("right");
        }
    }

    void leftCameraControls()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveLeftCam("right");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
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

        disableRotating();

        //decide which controls to use
        //if looking at yetis
        if (whichView == 2)
        {
            rightCameraControls();


            shopScript.setYetiView("Jim", 3, "DESC");
        }
        //else if looking at boards
        else if (whichView == 3)
        {
            leftCameraControls();

            shopScript.setBoardView(boardArray[currentBoard].name, (int) boardArray[currentBoard].cost, boardArray[currentBoard].desc, YetiGameData.boardBoughtArray[currentBoard]);

            boardArray[currentBoard].boardObject.GetComponent<RotateObjects>().rotating = true;
        }


    }
}