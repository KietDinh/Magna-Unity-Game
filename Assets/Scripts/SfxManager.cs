using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public AudioSource shootingSound;
    public AudioSource jumpSound;
    public AudioSource dashSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            shootingSound.Play();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            dashSound.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpSound.Play();
        }

    }
}
