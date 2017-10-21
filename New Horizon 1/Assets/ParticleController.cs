using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

	// Use this for initialization
	void Start () {

        ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
        em.enabled = true;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
