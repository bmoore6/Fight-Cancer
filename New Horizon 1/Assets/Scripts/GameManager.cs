using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField]
    GameObject healthBar;
    GameObject[] trees;
    float percentHealth;

	// Use this for initialization
	void Start() {
        percentHealth = 0;
        trees = GameObject.FindGameObjectsWithTag("tree");
	}
	
	// Update is called once per frame
	void Update () {
        percentHealth = 0;
        foreach (GameObject tree in trees)
        {
            percentHealth += tree.GetComponent<TreeScript>().Health;
        }
        percentHealth =percentHealth/trees.GetLength(0);
        healthBar.GetComponent<Image>().fillAmount = percentHealth;
	}
}
