using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    ParticleSystem part;

    // Use this for initialization
    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }
	
	// Remove bullet particle from game whenever system has fully cycled
	void Update () {
        if (part)
        {
            if (!part.IsAlive())
            {
                Destroy(gameObject);
            }
        }
		
	}
}
