using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesBottomLeft : MonoBehaviour {
    [SerializeField]
    GameObject[] BottomLeftPigs;

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
       Transform playerTransform =  GameObject.Find("Player").GetComponent<Transform>();

        if (Vector3.Distance(transform.position, playerTransform.position ) < .4f)
        {
            Instantiate(BottomLeftPigs[0], new Vector3(115, -69, 0), Quaternion.identity);
            Instantiate(BottomLeftPigs[1], new Vector3(115, -74, 0), Quaternion.identity);
            Instantiate(BottomLeftPigs[2], new Vector3(115, -78, 0), Quaternion.identity);
            Instantiate(BottomLeftPigs[3], new Vector3(115, -82, 0), Quaternion.identity);
        }

    }
}
