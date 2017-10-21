using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    public GameObject player;

    private Vector3 offset;

	// Use this for initialization
	void Start () {

        offset = transform.position - player.transform.position;
		
	}
	
	// This method runs after all items have already run in Update Method
	void LateUpdate () {

        transform.position = player.transform.position + offset;
		
	}
}
