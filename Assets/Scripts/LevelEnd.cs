using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public GameObject LM;
    private LevelManager lManager;

    // Use this for initialization
    void Start()
    {
        LM = GameObject.FindGameObjectWithTag("LevelManager");
        lManager = LM.GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        GameObject otherObj = other.gameObject;
        if (otherObj.CompareTag("Despawner"))
        {
            lManager.EndLevel();
        }
    }
}
