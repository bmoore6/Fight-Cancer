﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // audio source declaration
    public AudioSource audioS;

    // T-Cell sound declaration
    public AudioClip[] tCellShoot = new AudioClip[6];
    public AudioClip[] tCellSteps = new AudioClip[1];
    public AudioClip[] cytotoxinSplash = new AudioClip[1];

    // Cancer pig sound declaration
    public AudioClip[] cancerPigHit = new AudioClip[1];
    public AudioClip[] cancerPigAmbient = new AudioClip[1];

    // Menu sound declaration
    public AudioClip[] menuSounds = new AudioClip[1];

    // Background track declaration
    public AudioClip[] backgroundTrack = new AudioClip[1];

    // Singleton
    //public static SoundManager SM = null;

    void Start()
    {
        // Audio source initialization
        audioS = GetComponent<AudioSource>();

        // Singleton check
        //if (SM == null)
        //    SM = this;
        //else if (SM != null)
        //    Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    // Methods for calling the sounds
    #region Sound Methods

    // T-Cell firing noises
    public void TCellShoot()
    {
        audioS.PlayOneShot(tCellShoot[Random.Range(0, tCellShoot.Length)]);
    }

    // OPTIONAL T-Cell walking sounds
    public void TCellSteps()
    {

    }

    // Bullet despawn (hit) noises
    public void CytotoxinSplash()
    {
        audioS.PlayOneShot(cytotoxinSplash[Random.Range(0, cytotoxinSplash.Length)]);
    }

    // Cancer Pigs hitting the aveola
    public void CancerPigHit()
    {

    }

    // Ambient sounds for Cancer Pig movement
    public void CancerPigAmbient()
    {

    }

    // Menu beep sounds for navigation feedback
    public void MenuSounds(int soundNumber)
    {

    }

    // Background music method
    public void BackgroundTrack()
    {
        audioS.PlayOneShot(backgroundTrack[0]);
    }
    #endregion
}
