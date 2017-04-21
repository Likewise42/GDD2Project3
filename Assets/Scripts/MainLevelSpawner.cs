using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLevelSpawner : MonoBehaviour {

    private const int SPAWN_INTERVAL = 120;
    private const int HALF_LEVEL_WIDTH = 45;

    private List<GameObject> obstacles;
    private GameObject levelObj;
    private int spawnTimer;
    private Vector3 startPos;
    private Quaternion startRot;

    public GameObject obstaclePrefab;

	// Use this for initialization
	void Start () {
        obstacles = new List<GameObject>();
        spawnTimer = SPAWN_INTERVAL / 2;
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation;

        levelObj = GameObject.FindGameObjectWithTag("World");
    }
	
	// Update is called once per frame
	void Update () {
        CullList();
        
        if (spawnTimer <= 0)
        {
            obstacles.Add(CreateObstacle());

            spawnTimer = SPAWN_INTERVAL;
        }
        else
        {
            spawnTimer--;
        }
    }

    /// <summary>
    /// CullList iterates over the obstacles list and removes any member for whom reachedEnd == true
    /// </summary>
    void CullList()
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
    GameObject CreateObstacle()
    {
        GameObject newObstacle = Instantiate(obstaclePrefab,
                                                startPos,
                                                startRot);

        // Randomize obstacle's starting x position
        Vector3 obstacleStartPos = startPos;
        obstacleStartPos.x += Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH;  

        newObstacle.transform.position = obstacleStartPos;
        newObstacle.transform.parent = levelObj.transform;

        return newObstacle;
    }
}
