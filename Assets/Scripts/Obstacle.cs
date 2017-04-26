using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    private bool reachedEnd;

    public bool ReachedEnd
    {
        get { return reachedEnd; }
    }

	// Use this for initialization
	void Start () {
        reachedEnd = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider other)
    {
        GameObject otherObj = other.gameObject;
        if (otherObj.CompareTag("Despawner"))
        {
            reachedEnd = true;
        }
    }
}
