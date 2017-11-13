using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void HandleAlertButtonOnCLick()
    {
        Object.Instantiate(Resources.Load("TextBG"));
        Object.Instantiate(Resources.Load("MissionCanvas"));
        
    }

    public void HandleStartButtonOnClick()
    {
        SceneManager.LoadScene("Lungs");
    }

    public void HandleHelpButtonOnClick()
    {
        SceneManager.LoadScene("Help");
    }
}
