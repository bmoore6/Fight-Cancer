using System.Collections;
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

    void Start()
    {
        // Audio source initialization
        audioS = GetComponent<AudioSource>();
    }

    // Methods for calling the sounds
    #region Sound Methods

    // T-Cell firing noises
    public void TCellShoot()
    {

    }

    // OPTIONAL T-Cell walking sounds
    public void TCellSteps()
    {

    }

    // Bullet despawn (hit) noises
    public void CytotoxinSplash()
    {

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
        
    }
    #endregion
}
