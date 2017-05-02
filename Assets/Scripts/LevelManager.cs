using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level Manager contains logic for when to spawn objects/start events and holds level state
/// </summary>
public class LevelManager : MonoBehaviour {

    public const int OBSTACLE_SPAWN_INTERVAL = 120;
    public const int RAMP_SPAWN_INTERVAL = 120;
    public const int CASH_SPAWN_INTERVAL = 80;
    public const int TIME_TO_SLALOM = 600;
    public const int LEVEL_END_SPAWN_INTERVAL = 3600;   // 1 minute level

    // Slalom-specific constants
    public const int NUMBER_OF_SLALOMS = 10;
    private const int SLALOM_CHECKPOINT_TIMER = 10;
    public const int TIME_BETWEEN_SLALOMS = 120;

    private uint gameEndTimer;
    private int obstacleSpawnTimer;
    private int rampSpawnTimer;
    private int cashSpawnTimer;
    private int levelEndSpawnTimer;
    private int slalomTimer;

    public bool hitSlalom;
    private bool canSpawnSlalomCheckpoint;

    private uint score;
    private bool stopSpawning;
    private bool slalomEvent;
    private int currentSlalomCount;

    public GameScreenScript gameScreen;
    public GameObject SpawnerObj;
    private LevelSpawner spawner;

    // Use this for initialization
    void Start () {
        // Start timer a little bit along
        obstacleSpawnTimer = OBSTACLE_SPAWN_INTERVAL / 2;
        rampSpawnTimer = RAMP_SPAWN_INTERVAL / 2 - 10;
        cashSpawnTimer = 0;
        levelEndSpawnTimer = 0;
        slalomTimer = 0;
        currentSlalomCount = 0;

        slalomEvent = false;
        stopSpawning = false;

        spawner = SpawnerObj.GetComponent<LevelSpawner>();
	}
	
	// Update is called once per frame
	void Update () {

        if (slalomEvent)
        {
            //  --  Spawning    --
            if (currentSlalomCount <= NUMBER_OF_SLALOMS)
            {
                if (slalomTimer == 0)
                {
                    spawner.CreateSlalomFlag();
                    canSpawnSlalomCheckpoint = true;
                    currentSlalomCount++;
                    slalomTimer++;
                }
                else if (slalomTimer >= TIME_BETWEEN_SLALOMS)
                {
                    slalomTimer = 0;
                }
                else if (slalomTimer >= SLALOM_CHECKPOINT_TIMER && canSpawnSlalomCheckpoint)
                {
                    spawner.CreateSlalomCheckpoint();
                    canSpawnSlalomCheckpoint = false;   // So we only spawn 1 checkpoint per slalom
                }
                else
                {
                    slalomTimer++;
                }
            }
            else
            {
                slalomEvent = false;
            }
        }
        else if (!stopSpawning)
        {
            if (ShouldSpawnLevelEnd())
            {
                spawner.CreateLevelEnd();
                stopSpawning = true;
                return; // Don't bother running the rest of Update
            }

            if (ShouldStartSlalom())
            {
                slalomEvent = true;
                canSpawnSlalomCheckpoint = false;
                slalomTimer = -1;
            }

            if (ShouldSpawnObstacle())
            {
                spawner.CreateObstacle();
            }

            if (ShouldSpawnRamp())
            {
                spawner.CreateRamp();
            }

            if (ShouldSpawnCash())
            {
                spawner.CreateColdCash();
            }

            obstacleSpawnTimer++;
            rampSpawnTimer++;
            cashSpawnTimer++;
            levelEndSpawnTimer++;
            slalomTimer++;

            gameScreen.SetDistancePercent((float)levelEndSpawnTimer / (float)LEVEL_END_SPAWN_INTERVAL);
        }
	}

    bool ShouldStartSlalom()
    {
        if (slalomTimer >= TIME_TO_SLALOM)
        {
            slalomTimer = 0;
            slalomEvent = true;
            hitSlalom = false;
            return true;
        }

        return false;
    }

    bool ShouldSpawnObstacle()
    {
        if (obstacleSpawnTimer >= OBSTACLE_SPAWN_INTERVAL)
        {
            obstacleSpawnTimer = 0;
            return true;
        }

        return false;
    }

    bool ShouldSpawnRamp()
    {
        if (rampSpawnTimer >= RAMP_SPAWN_INTERVAL)
        {
            rampSpawnTimer = 0;
            return true;
        }

        return false;
    }

    bool ShouldSpawnCash()
    {
        if (cashSpawnTimer >= CASH_SPAWN_INTERVAL)
        {
            cashSpawnTimer = 0;
            return true;
        }

        return false;
    }

    bool ShouldSpawnLevelEnd()
    {
        if (levelEndSpawnTimer >= LEVEL_END_SPAWN_INTERVAL)
        {
            levelEndSpawnTimer = 0;
            return true;
        }

        return false;
    }

    void addColdCash(uint amount)
    {
        YetiGameData.ColdCash += amount;
        gameScreen.SetColdCash(YetiGameData.ColdCash);
    }

    void addScore(uint score)
    {
        this.score += score;
        gameScreen.SetScore(score);
    }
}
