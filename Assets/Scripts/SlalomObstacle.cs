using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlalomObstacle : MonoBehaviour {

    public bool reachedEnd;

    public GameObject pole1;
    public GameObject pole2;

    public Material successMaterial;

    public bool ReachedEnd
    {
        get { return reachedEnd; }
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

    public void Success()
    {
        pole1.GetComponent<MeshRenderer>().material = successMaterial;
        pole2.GetComponent<MeshRenderer>().material = successMaterial;
    }
    

    void OnTriggerEnter(Collider other)
    {
        GameObject otherObj = other.gameObject;
        if (otherObj.CompareTag("Despawner"))
        {
            reachedEnd = true;
        }
    }
}
