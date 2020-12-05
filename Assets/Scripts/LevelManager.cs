using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float waitToRespawn;
    public PlayerController thePlayer;
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
    }

    
    void Update()
    {
        
    }
    public void Respawn()
    {
        thePlayer.gameObject.SetActive(false);
    }
}
