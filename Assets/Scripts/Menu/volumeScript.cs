using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volumeScript : MonoBehaviour
{

    private float musicVolume = 1f;

    void Update()
    {
        AudioListener.volume = musicVolume;
    }

    // Method that is called by slider game object
    // This method takes vol value passed by slider
    // and sets it as musicValue
    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
}
