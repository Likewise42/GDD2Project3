using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjects : MonoBehaviour {

    public bool rotating;
    private Vector3 startRot;
    private float startY;

	// Use this for initialization
	void Start () {
        rotating = false;
        startRot = this.transform.eulerAngles;
        startY = this.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (rotating)
        {
            this.transform.Rotate(new Vector3(0, 40 * Time.deltaTime, 0), Space.World);
            this.transform.Translate(0, Mathf.Sin(Time.time * 2) * .005f, 0, Space.World);
        }
        else
        {
            this.transform.eulerAngles = startRot;
            this.transform.position = new Vector3(this.transform.position.x, startY, this.transform.position.z);
        }
	}
}
