using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour {
    
    public const int HALF_LEVEL_WIDTH = 37;

    public GameObject obstaclePrefab;
    public GameObject rampPrefab;
    public GameObject levelEndPrefab;
    public GameObject coldCashPrefab;

    public GameObject slalomFlagPrefab;
    public GameObject slalomCheckpointPrefab;

    public GameObject BoostPickupPrefab;
    public GameObject CashPickupPrefab;
    public GameObject MultiplierPickupPrefab;

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
        return CreateObject(levelEndPrefab,
                            new Vector3(0, 3, 0),
                            null);
    }

    public GameObject CreateSlalomFlag()
    {
        return CreateObject(slalomFlagPrefab,
                            new Vector3(Random.Range(-0.8f, 0.8f) * HALF_LEVEL_WIDTH, 0, 0),
                            slalomObjs);
    }

    public GameObject CreateSlalomCheckpoint()
    {
        return CreateObject(slalomCheckpointPrefab,
                            new Vector3(0, 0, 0),
                            slalomObjs);
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
