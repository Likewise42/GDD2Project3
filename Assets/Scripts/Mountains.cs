﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountains : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Translate(0f, 0f, Time.deltaTime * -30);
	}
}
