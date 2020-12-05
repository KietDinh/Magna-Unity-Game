using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundAdjustment : MonoBehaviour
{
    // Start is called before the first frame update

    AudioSource theme;

    float musicSlider = 1f;
    void Start()
    {
        theme = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        theme.volume = musicSlider;
    }

    public void setVolume(float vol)
    {
        musicSlider = vol;
    }
}
