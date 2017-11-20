using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTrackScriptTemp : MonoBehaviour
{

    SoundManager sounds;
    public bool canPlay = true;

	// Use this for initialization
	void Start ()
    {
        sounds = GetComponent<SoundManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (canPlay)
        {
            sounds.BackgroundTrack();
            canPlay = false;
        }
	}
}
