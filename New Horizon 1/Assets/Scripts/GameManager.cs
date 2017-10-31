using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    GameObject healthBar;
    [SerializeField]
    float minHealth=.2f;
    GameObject[] trees;
    float percentHealth;

	// Use this for initialization
	void Start() {
        percentHealth = 0;
        trees = GameObject.FindGameObjectsWithTag("tree");
	}
	
	// Update is called once per frame
	void Update () {
        //update health bar
        percentHealth = 0;
        foreach (GameObject tree in trees)
        {
            percentHealth += tree.GetComponent<TreeScript>().Health;
        }
        percentHealth =percentHealth/trees.GetLength(0);
        healthBar.GetComponent<Image>().fillAmount = percentHealth;

        //loss condition
        if (percentHealth < minHealth)
        {

        }
	}
    public static void win()
    {
        if (GameObject.FindGameObjectsWithTag("Pig").GetLength(0) <= 0)
        {
            SceneManager.LoadScene("WinScn");
        }
    }
    public void lose()
    {
        SceneManager.LoadScene("LoseScn");
    }
}
