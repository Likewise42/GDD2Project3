﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {
    
    public const int HALF_LEVEL_WIDTH = 40;

    public GameObject obstaclePrefab;
    public GameObject rampPrefab;
    public GameObject levelEndPrefab;
    public GameObject coldCashPrefab;

    public GameObject slalomFlagPrefab;
    public GameObject slalomCheckpointPrefab;

    private List<GameObject> obstacles;
    private List<GameObject> ramps;
    private List<GameObject> coldcash;
    private List<GameObject> slalomObjs;
    private GameObject levelObj;
    private Vector3 startPos;
    private Quaternion startRot;

    // Use this for initialization
    void Start()
    {
        obstacles = new List<GameObject>();
        ramps = new List<GameObject>();
        coldcash = new List<GameObject>();
        slalomObjs = new List<GameObject>();
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation;

        levelObj = GameObject.FindGameObjectWithTag("World");
    }

    // Update is called once per frame
    void Update()
    {
        CullObjects();
    }

    /// <summary>
    /// CullObstacles iterates over the obstacles list and removes any member for whom reachedEnd == true
    /// </summary>
    void CullObstacles()
    {
        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            GameObject obst = obstacles[i];
            Obstacle obstScript = obst.GetComponent<Obstacle>();
            if (obstScript.ReachedEnd)
            {
                obstacles.RemoveAt(i);
                Destroy(obst);
            }
        }
    }

    /// <summary>
    /// CullRamps iterates over the ramps list and removes any member for whom reachedEnd == true
    /// </summary>
    void CullRamps()
    {
        for (int i = ramps.Count - 1; i >= 0; i--)
        {
            GameObject ramp = ramps[i];
            Ramp rampScript = ramp.GetComponent<Ramp>();
            if (rampScript.ReachedEnd)
            {
                ramps.RemoveAt(i);
                Destroy(ramp);
            }
        }
    }

    /// <summary>
    /// CullCash iterates over the coldcash list and removes any member for whom reachedEnd == true
    /// </summary>
    void CullCash()
    {
        for (int i = coldcash.Count - 1; i >= 0; i--)
        {
            GameObject coldcashObj = coldcash[i];
            ColdCash coldcashScript = coldcashObj.GetComponent<ColdCash>();
            if (coldcashScript.ReachedEnd || coldcashScript.PickedUp)
            {
                coldcash.RemoveAt(i);
                Destroy(coldcashObj);
            }
        }
    }

    /// <summary>
    /// CullCash iterates over the slalomObjs list and removes any member for whom reachedEnd == true
    /// </summary>
    void CullSlalomObjs()
    {
        for (int i = slalomObjs.Count - 1; i >= 0; i--)
        {
            GameObject slalomObj = slalomObjs[i];
            Obstacle slalomScript = slalomObj.GetComponent<Obstacle>();
            if (slalomScript.ReachedEnd)
            {
                slalomObjs.RemoveAt(i);
                Destroy(slalomObj);
            }
        }
    }

    /// <summary>
    /// CullLists is a convenience method so we can call a single method to cull all lists
    /// </summary>
    void CullObjects()
    {
        CullObstacles();
        CullRamps();
        CullCash();
        CullSlalomObjs();
    }

    /// <summary>
    /// MoveObject is a helper function to move a given GameObject in the level.
    ///  Movement amounts are hard-coded in the function appropriate for the level.
    /// </summary>
    /// <param name="obst">The GameObject to move</param>
    void MoveObject(GameObject obst)
    {
        Vector3 obstPos = obst.transform.position;
        obstPos.x += 0.1f;
        obst.transform.position = obstPos;
    }

    /// <summary>
    /// CreateObstacle creates an instance of the obstacle prefab defined in the spawner
    /// </summary>
    /// <returns>A new obstacle GameObject</returns>
    public GameObject CreateObstacle()
    {
        GameObject newObstacle = Instantiate(obstaclePrefab,
                                                startPos,
                                                startRot);

        // Randomize obstacle's starting x position
        Vector3 obstacleStartPos = startPos;
        obstacleStartPos.x += Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH;
        obstacleStartPos.y += 3;

        newObstacle.transform.position = obstacleStartPos;
        newObstacle.transform.parent = levelObj.transform;

        obstacles.Add(newObstacle);

        return newObstacle;
    }

    /// <summary>
    /// CreateRamp creates an instance of the ramp prefab defined in the spawner
    /// </summary>
    /// <returns>A new ramp GameObject</returns>
    public GameObject CreateRamp()
    {
        GameObject newRamp = Instantiate(rampPrefab,
                                                startPos,
                                                startRot);

        // Randomize obstacle's starting x position
        Vector3 rampStartPos = startPos;
        rampStartPos.x += Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH;
        rampStartPos.y += 3;

        newRamp.transform.position = rampStartPos;
        newRamp.transform.parent = levelObj.transform;

        ramps.Add(newRamp);

        return newRamp;
    }

    /// <summary>
    /// CreateColdCash creates an instance of the coldcash prefab defined in the spawner
    /// </summary>
    /// <returns>A new ColdCash GameObject</returns>
    public GameObject CreateColdCash()
    {
        GameObject newCash = Instantiate(coldCashPrefab,
                                                startPos,
                                                startRot);

        // Randomize obstacle's starting x position
        Vector3 cashStartPos = startPos;
        cashStartPos.x += Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH;
        cashStartPos.y += 3;

        newCash.transform.position = cashStartPos;
        newCash.transform.parent = levelObj.transform;

        coldcash.Add(newCash);

        return newCash;
    }

    /// <summary>
    /// CreateLevelEnd creates an instance of the level end prefab defined in the spawner
    /// </summary>
    /// <returns>A new level end GameObject</returns>
    public GameObject CreateLevelEnd()
    {
        GameObject levelEnd = Instantiate(levelEndPrefab,
                                                startPos,
                                                startRot);

        // Randomize obstacle's starting x position
        Vector3 levelEndStartPos = startPos;
        levelEndStartPos.y += 3;

        levelEnd.transform.position = levelEndStartPos;
        levelEnd.transform.parent = levelObj.transform;

        return levelEnd;
    }

    public GameObject CreateSlalomFlag()
    {
        GameObject slalomFlag = Instantiate(slalomFlagPrefab,
                                                    startPos,
                                                    startRot);

        Vector3 adjust = new Vector3(Random.Range(-0.8f, 0.8f) * HALF_LEVEL_WIDTH, 0, 0);
        slalomFlag.transform.Translate(adjust);
        slalomFlag.transform.parent = levelObj.transform;

        slalomObjs.Add(slalomFlag);

        return slalomFlag;
    }

    public GameObject CreateSlalomCheckpoint()
    {
        GameObject slalomCheckpoint = Instantiate(slalomCheckpointPrefab,
                                                    startPos,
                                                    startRot);
        
        slalomCheckpoint.transform.parent = levelObj.transform;

        slalomObjs.Add(slalomCheckpoint);

        return slalomCheckpoint;
    }
}
