using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {
    
    public const int HALF_LEVEL_WIDTH = 45;

    public GameObject obstaclePrefab;
    public GameObject rampPrefab;
    public GameObject levelEndPrefab;

    private List<GameObject> obstacles;
    private List<GameObject> ramps;
    private GameObject levelObj;
    private Vector3 startPos;
    private Quaternion startRot;

    // Use this for initialization
    void Start()
    {
        obstacles = new List<GameObject>();
        ramps = new List<GameObject>();
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
    /// CullLists is a convenience method so we can call a single method to cull all lists
    /// </summary>
    void CullObjects()
    {
        CullObstacles();
        CullRamps();
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

        newObstacle.transform.position = obstacleStartPos;
        newObstacle.transform.parent = levelObj.transform;

        obstacles.Add(newObstacle);

        return newObstacle;
    }

    /// <summary>
    /// CreateaRamp creates an instance of the ramp prefab defined in the spawner
    /// </summary>
    /// <returns>A new obstacle GameObject</returns>
    public GameObject CreateRamp()
    {
        GameObject newRamp = Instantiate(rampPrefab,
                                                startPos,
                                                startRot);

        // Randomize obstacle's starting x position
        Vector3 rampStartPos = startPos;
        rampStartPos.x += Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH;

        newRamp.transform.position = rampStartPos;
        newRamp.transform.parent = levelObj.transform;

        ramps.Add(newRamp);

        return newRamp;
    }

    public GameObject CreateLevelEnd()
    {
        GameObject levelEnd = Instantiate(levelEndPrefab,
                                                startPos,
                                                startRot);

        // Randomize obstacle's starting x position
        Vector3 levelEndStartPos = startPos;
        levelEndStartPos.x += Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH;

        levelEnd.transform.position = levelEndStartPos;
        levelEnd.transform.parent = levelObj.transform;

        return levelEnd;
    }
}
