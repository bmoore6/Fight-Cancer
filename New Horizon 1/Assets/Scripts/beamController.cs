using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beamController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Destroy this object if the player is not holding down the left mouse button
	void Update () {

        if (!Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
		
	}
}
