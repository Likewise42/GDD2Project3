using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level Manager contains logic for when to spawn objects/start events and holds level state
/// </summary>
public class LevelManager : MonoBehaviour {

    public const float OBSTACLE_SPAWN_INTERVAL = 50;
    public const float RAMP_SPAWN_INTERVAL = 240;
    public const float CASH_SPAWN_INTERVAL = 80;
    public const float PICKUP_SPAWN_INTERVAL = 480;
    public const float TIME_TO_SLALOM = 1100;
    public const float LEVEL_END_SPAWN_INTERVAL = 4200;

    // Slalom-specific constants
    public const int NUMBER_OF_SLALOMS = 17;
    private const float SLALOM_CHECKPOINT_TIMER = 10;
    public const float TIME_BETWEEN_SLALOMS = 70;

    public const float SLALOM_MULT_DELTA = .05f;

    public uint slalomBasePoints = 100;
    private uint gameEndTimer;
    private float obstacleSpawnTimer;
    private float rampSpawnTimer;
    private float cashSpawnTimer;
    private float pickupSpawnTimer;
    private float levelEndSpawnTimer;
    private float slalomTimer;

    public bool hitSlalom;
    private bool canSpawnSlalomCheckpoint;
    private bool levelRunning = true;

    private float timeInSeconds = 0.0f;

    public uint coldCashScore = 100;

    private uint airborneScore = 0;

    public uint airSizeMaxScore;

    private bool airborne = false;

    public int airScorePerNFrames;
    public int airScoreNFramesTick;
    public uint airScoreBase = 100;

    private uint timesPassedAirScore = 1;
    private int airscoreFrameCount = 0;

    private GameObject levelObj;

    private uint currentColdCash;
    private uint score;
    private float slalomMultiplier = 0;
    public float scoreMultiplier = 1;
    private bool stopSpawning;
    private bool slalomEvent;
    private int currentSlalomCheckpointCount;

    public GameScreenScript gameScreen;
    public GameObject SpawnerObj;
    private LevelSpawner spawner;

    public GameObject OGYeti;
    public GameObject LankyYeti;
    public GameObject FemaleYeti;


    // Use this for initialization
    void Start()
    {
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

        levelObj = GameObject.FindGameObjectWithTag("World");
        // QualitySettings.vSyncCount = 0;  // VSync must be disabled
        // Application.targetFrameRate = 15;
        
        OGYeti.SetActive(false);
        LankyYeti.SetActive(false);
        FemaleYeti.SetActive(false);

        switch (YetiGameData.SelectedYeti)
        {
            case YetiGameData.YetiType.NormalYeti:
                OGYeti.SetActive(true);
                break;
            case YetiGameData.YetiType.LankyYeti:
                LankyYeti.SetActive(true);
                break;
            case YetiGameData.YetiType.FemaleYeti:
                FemaleYeti.SetActive(true);
                break;
            default:
                OGYeti.SetActive(true);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {

        float timePassed = 100 * Time.deltaTime * levelObj.GetComponent<WorldSpin>().speed / GameObject.FindGameObjectWithTag("Snowboard").GetComponent<Snowboard>().maxSpeed;

        if (!levelRunning)
        {
            return;
        }

        if (airborne)
        {
            if(airscoreFrameCount == airScoreNFramesTick)
            {
                addAirborneScore(airScoreBase * timesPassedAirScore);
                airscoreFrameCount = 0;
                timesPassedAirScore++;
            }
            else
            {
                airscoreFrameCount++;
            }
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
                    slalomTimer += timePassed;
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
                    slalomTimer += timePassed;
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

            if (ShouldSpawnPickup())
            {
                // Randomly choose a pickup to spawn		
                float decision = Random.Range(0, 1.0f);
                if (decision < 0.33f)
                {
                    spawner.CreateBoostPickup();
                }
                else if (decision < 0.66f)
                {
                    spawner.CreateCashBonusPickup();
                }
                else
                {
                    spawner.CreateMultiplierPickup();
                }
            }
            
            obstacleSpawnTimer += timePassed;
            rampSpawnTimer += timePassed;
            cashSpawnTimer += timePassed;
            pickupSpawnTimer += timePassed;
            levelEndSpawnTimer += timePassed;
            slalomTimer += timePassed;

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

    bool ShouldSpawnPickup()
    {
        if (pickupSpawnTimer >= PICKUP_SPAWN_INTERVAL)
        {
            pickupSpawnTimer = 0;
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

    public void startAirScore()
    {
        airborne = true;
        airscoreFrameCount = airScoreNFramesTick;
    }

    public void endAirScore()
    {
        airborne = false;
        flushAirborneScore();
        timesPassedAirScore = 1;
    }

    private void addAirborneScore(uint score)
    {
        airborneScore += score;
        gameScreen.setAirScore(airborneScore, (float)airborneScore/airSizeMaxScore);
    }

    private void flushAirborneScore()
    {
        addScore(airborneScore);
        airborneScore = 0;
        airscoreFrameCount = 0;
        gameScreen.hideAirScore();
    }

    public void addScore(uint score)
    {
        this.score += (uint)(scoreMultiplier * score);
        gameScreen.SetScore(this.score, scoreMultiplier);
    }

    public void addColdCash(uint coldCashAmount)
    {
        YetiGameData.ColdCash += coldCashAmount /*(uint)YetiGameData.coldCashMultiplier*/;
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
            resetSlalomMult();
        }
    }

    public void resetSlalomMult()
    {
        scoreMultiplier -= slalomMultiplier;
        slalomMultiplier = 0;
    }
}
