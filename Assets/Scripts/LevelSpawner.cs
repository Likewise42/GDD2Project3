using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {

    private const int SPAWN_INTERVAL = 180;
    private const int LEVEL_WIDTH = 7;

    private List<GameObject> obstacles;
    private int spawnTimer;
    private Vector3 startPos;

    public GameObject obstaclePrefab;

	// Use this for initialization
	void Start () {
        obstacles = new List<GameObject>();
        spawnTimer = SPAWN_INTERVAL;
        startPos = new Vector3(-12.0f, 0.55f, 0);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(spawnTimer);
        Debug.Log(obstacles.Count);

        foreach (GameObject obst in obstacles)
        {
            Vector3 obstPos = obst.transform.position;
            obstPos.x += 0.1f;
            obst.transform.position = obstPos;
        }

        if (spawnTimer <= 0)
        {
            GameObject newObstacle = Instantiate(obstaclePrefab, obstaclePrefab.transform.position, Quaternion.identity);
            Vector3 obstacleStartPos = startPos;
            obstacleStartPos.z += Random.Range(-1.0f, 1.0f) * LEVEL_WIDTH;  // Randomize obstacle's starting z position

            newObstacle.transform.position = obstacleStartPos;

            obstacles.Add(newObstacle);

            spawnTimer = SPAWN_INTERVAL;
        }
        else
        {
            spawnTimer--;
        }
    }
}
