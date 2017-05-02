using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdCash : MonoBehaviour {

    private bool reachedEnd;
    private bool pickedUp;

    public bool ReachedEnd
    {
        get { return reachedEnd; }
    }

    public bool PickedUp
    {
        get { return pickedUp; }
    }

    // Use this for initialization
    void Start()
    {
        reachedEnd = false;
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
            reachedEnd = true;
        }
        else if (otherObj.CompareTag("Player"))
        {
            pickedUp = true;
        }
    }
}
