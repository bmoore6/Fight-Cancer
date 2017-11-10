using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUnits : MonoBehaviour {

    // all particles created for the flock
    [SerializeField]
    public List<GameObject> units = new List<GameObject>();

    // prefab for the little pig
    [SerializeField]
    GameObject unitPrefab;

    // number of little pigs to be in the flock
    [SerializeField]
    int numUnits = 100;

    // size of the flock (in screen space)
    [SerializeField]
    Vector3 range = new Vector3(5, 5, 5);

    // these control the general behavior of ALL particles (little pigs)
    public bool seekGoal = true;
    public bool obedient = true;
    public bool willful = false;

    // determine which other litte pigs are your neighbors
    [Range(0, 200)]
    public int neighborDistance = 50;

    // provide a max force (this will keep each unit within the flock under control)
    [Range(0, 2)]
    public float maxforce = 0.5f;

    [Range(0, 5)]
    public float maxvelocity = 2.0f;

	// Use this for initialization
	void Start () {

        for (int i=0; i < numUnits; i++)
        {
            Vector3 unitPos = new Vector3(Random.Range(-range.x, range.x),
                Random.Range(-range.y, range.y),
                Random.Range(0, 0));

            units.Add(Instantiate(unitPrefab, this.transform.position + unitPos, Quaternion.identity) as GameObject);
            units[i].GetComponent<Unit>().manager = this.gameObject;
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
