using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {
    
    public const int HALF_LEVEL_WIDTH = 30;

    public GameObject obstaclePrefab;
    public GameObject rampPrefab;
    public GameObject levelEndPrefab;
    public GameObject coldCashPrefab;

    public GameObject slalomFlagPrefab;
    public GameObject slalomCheckpointPrefab;

    public GameObject BoostPickupPrefab;
    public GameObject CashBonusPickupPrefab;
    public GameObject MultiplierPickupPrefab;

    private List<GameObject> obstacles;
    private List<GameObject> ramps;
    private List<GameObject> coldcash;
    private List<GameObject> slalomObjs;
    private List<GameObject> slalomCheckpointObjs;
    private List<GameObject> pickups;
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
        slalomCheckpointObjs = new List<GameObject>();
        pickups = new List<GameObject>();
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
            
            SlalomObstacle slalomScript = slalomObj.GetComponent<SlalomObstacle>();
            Debug.Log(slalomScript.ReachedEnd);
            if (slalomScript.ReachedEnd)
            {
                slalomObjs.RemoveAt(i);
                Destroy(slalomObj);
            }
        }
    }

    /// <summary>
    /// CullCash iterates over the slalomObjs list and removes any member for whom reachedEnd == true
    /// </summary>
    void CullSlalomCheckpointObjs()
    {
        for (int i = slalomCheckpointObjs.Count - 1; i >= 0; i--)
        {
            GameObject slalomCheckpointObj = slalomCheckpointObjs[i];

            Obstacle slalomScript = slalomCheckpointObj.GetComponent<Obstacle>();
            Debug.Log(slalomScript.ReachedEnd);
            if (slalomScript.ReachedEnd)
            {
                slalomCheckpointObjs.RemoveAt(i);
                Destroy(slalomCheckpointObj);
            }
        }
    }

    void CullPickups()
    {
        for (int i = pickups.Count - 1; i >= 0; i--)
        {
            GameObject pickupObj = pickups[i];
            Pickup pickupScript = pickupObj.GetComponent<Pickup>();
            if (pickupScript.ReachedEnd || pickupScript.PickedUp)
            {
                pickups.RemoveAt(i);
                Destroy(pickupObj);
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
        CullSlalomCheckpointObjs();
        CullPickups();
    }
    
    public GameObject CreateObstacle()
    {
        return CreateObject(obstaclePrefab,
                            new Vector3(Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH, 0, 0),
                            obstacles);
    }
    
    public GameObject CreateRamp()
    {
        return CreateObject(rampPrefab,
                            new Vector3(Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH, 0, 0),
                            ramps);
    }
    
    public GameObject CreateColdCash()
    {
        return CreateObject(coldCashPrefab,
                            new Vector3(Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH, 3, 0),
                            coldcash);
    }
    
    public GameObject CreateLevelEnd()
    {
        GameObject newObject = Instantiate(levelEndPrefab, startPos, startRot);

        newObject.transform.Translate(new Vector3(0, 10, 0));
        newObject.transform.Rotate(new Vector3(0, 1, 0), 90);
        newObject.transform.parent = levelObj.transform;
        
        return newObject;
    }

    public GameObject CreateSlalomFlag()
    {
        return CreateObject(slalomFlagPrefab,
                            new Vector3(Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH, 0, 0),
                            slalomObjs);
    }

    public GameObject CreateSlalomCheckpoint()
    {
        return CreateObject(slalomCheckpointPrefab,
                            new Vector3(0, 0, 0),
                            slalomCheckpointObjs);
    }

    public GameObject CreateBoostPickup()
    {
        return CreateObject(BoostPickupPrefab,
                            new Vector3(Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH, 3, 0),
                            pickups);
    }

    public GameObject CreateCashBonusPickup()
    {
        return CreateObject(CashBonusPickupPrefab,
                            new Vector3(Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH, 3, 0),
                            pickups);
    }

    public GameObject CreateMultiplierPickup()
    {
        return CreateObject(MultiplierPickupPrefab,
                            new Vector3(Random.Range(-1.0f, 1.0f) * HALF_LEVEL_WIDTH, 3, 0),
                            pickups);
    }

    private GameObject CreateObject(GameObject prefab, Vector3 adjustmentVector, List<GameObject> objectList)
    {
        GameObject newObject = Instantiate(prefab, startPos, startRot);

        newObject.transform.Translate(adjustmentVector);
        newObject.transform.parent = levelObj.transform;

        if (objectList != null)
        {
            objectList.Add(newObject);
        }

        return newObject;
    }

}
