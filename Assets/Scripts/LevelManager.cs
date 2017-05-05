﻿using System.Collections;
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
    public const int LEVEL_END_SPAWN_INTERVAL = 100;   // 1 minute level

    // Slalom-specific constants
    public const int NUMBER_OF_SLALOMS = 10;
    private const int SLALOM_CHECKPOINT_TIMER = 10;
    public const int TIME_BETWEEN_SLALOMS = 120;

    public const float SLALOM_MULT_DELTA = .05f;

    public uint slalomBasePoints = 100;
    private uint gameEndTimer;
    private int obstacleSpawnTimer;
    private int rampSpawnTimer;
    private int cashSpawnTimer;
    private int levelEndSpawnTimer;
    private int slalomTimer;

    public bool hitSlalom;
    private bool canSpawnSlalomCheckpoint;
    private bool levelRunning = true;

    private float timeInSeconds = 0.0f;

    public uint coldCashScore = 100;

    private uint currentColdCash;
    private uint score;
    private float slalomMultiplier = 0;
    private float scoreMultiplier = 1;
    private bool stopSpawning;
    private bool slalomEvent;
    private int currentSlalomCheckpointCount;

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
        currentSlalomCheckpointCount = 0;

        slalomEvent = false;
        stopSpawning = false;

        spawner = SpawnerObj.GetComponent<LevelSpawner>();
	}
	
	// Update is called once per frame
	void Update () {


        if (!levelRunning)
        {
            return;
        }

        timeInSeconds += Time.deltaTime;
        if (slalomEvent)
        {
            //  --  Spawning    --
            if (currentSlalomCheckpointCount <= NUMBER_OF_SLALOMS)
            {
                if (slalomTimer == 0)
                {
                    spawner.CreateSlalomFlag();
                    canSpawnSlalomCheckpoint = true;
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
                    currentSlalomCheckpointCount++;
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

    public void EndLevel()
    {
        if (!levelRunning)
        {
            return;
        }
        levelRunning = false;
        bool nhs = score > YetiGameData.HighScore;
        gameScreen.EndGameUI((int)timeInSeconds, score, currentColdCash, nhs);
        if (nhs)
        {
            YetiGameData.HighScore = score;
        }

    }

    public void addScore(uint score)
    {
        this.score += (uint)(scoreMultiplier * score);
        gameScreen.SetScore(this.score);
    }

    public void addColdCash(uint coldCashAmount)
    {
        YetiGameData.ColdCash += (uint)YetiGameData.coldCashMultiplier;
        currentColdCash += coldCashAmount;
        addScore(coldCashScore);
    }

    public void procSlalom()
    {
        if (hitSlalom)
        {
            addScore(slalomBasePoints);
            scoreMultiplier += SLALOM_MULT_DELTA;
            slalomMultiplier += SLALOM_MULT_DELTA;
            hitSlalom = false;
        }
        else
        {
            scoreMultiplier -= slalomMultiplier;
            slalomMultiplier = 0;
        }
    }
}
