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
    int numUnits = 500;

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
    [Range(0, 500)]
    public float maxforce = 500f;

    [Range(0, 1000)]
    public float maxvelocity = 1000.0f;

    // Array of all trees in the game
    GameObject[] trees;

    // Use this for initialization
    void Start() {

        for (int i = 0; i < numUnits; i++)
        {
            Vector3 unitPos = new Vector3(Random.Range(-range.x, range.x),
                Random.Range(-range.y, range.y),
                Random.Range(0, 0));

            units.Add(Instantiate(unitPrefab, this.transform.position + unitPos, Quaternion.identity) as GameObject);
            units[i].GetComponent<Unit>().manager = this.gameObject;
        }

        // Populate trees array with all trees currently instantiated. 
        // May need to change this code if we decide to dynamically add trees to the game during runtime
        trees = GameObject.FindGameObjectsWithTag("tree");

    }

    /// <summary>
    /// Make Trees array available through a property
    /// </summary>
    public GameObject[] GetTrees { get { return trees; } }
	
	// Update is called once per frame
	void Update () {
		
	}
}
