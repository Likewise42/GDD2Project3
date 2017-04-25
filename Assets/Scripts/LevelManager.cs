using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level Manager contains logic for when to spawn objects/start events and holds level state
/// </summary>
public class LevelManager : MonoBehaviour {

    public const int OBSTACLE_SPAWN_INTERVAL = 120;
    public const int RAMP_SPAWN_INTERVAL = 120;
    public const int LEVEL_END_SPAWN_INTERVAL = 3600;   // 1 minute level
    
    private int obstacleSpawnTimer;
    private int rampSpawnTimer;
    private int levelEndSpawnTimer;
    private bool stopSpawning;

    public GameObject SpawnerObj;
    private LevelSpawner spawner;

    // Use this for initialization
    void Start () {
        // Start timer a little bit along
        obstacleSpawnTimer = OBSTACLE_SPAWN_INTERVAL / 2;
        rampSpawnTimer = RAMP_SPAWN_INTERVAL / 2;
        levelEndSpawnTimer = 0;
        stopSpawning = false;

        spawner = SpawnerObj.GetComponent<LevelSpawner>();
	}
	
	// Update is called once per frame
	void Update () {

        if (!stopSpawning)
        {
            if (ShouldSpawnLevelEnd())
            {
                spawner.CreateLevelEnd();
                stopSpawning = true;
                return; // Don't bother running the rest of Update
            }

            if (ShouldSpawnObstacle())
            {
                spawner.CreateObstacle();
            }

            if (ShouldSpawnRamp())
            {
                spawner.CreateRamp();
            }

            obstacleSpawnTimer++;
            rampSpawnTimer++;
            levelEndSpawnTimer++;
        }
	}

    bool ShouldSpawnObstacle()
    {
        if (obstacleSpawnTimer == OBSTACLE_SPAWN_INTERVAL)
        {
            obstacleSpawnTimer = 0;
            return true;
        }

        return false;
    }

    bool ShouldSpawnRamp()
    {
        if (rampSpawnTimer == RAMP_SPAWN_INTERVAL)
        {
            rampSpawnTimer = 0;
            return true;
        }

        return false;
    }

    bool ShouldSpawnLevelEnd()
    {
        if (levelEndSpawnTimer == LEVEL_END_SPAWN_INTERVAL)
        {
            levelEndSpawnTimer = 0;
            return true;
        }

        return false;
    }
}
