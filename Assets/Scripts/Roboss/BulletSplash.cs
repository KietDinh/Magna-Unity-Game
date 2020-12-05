using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSplash : MonoBehaviour
{
    // Start is called before the first frame update
    Transform player;
    bool isFlipped = false;


    void Start()
    {
        Destroy(gameObject, 0.2f);
        player = GameObject.FindGameObjectsWithTag("Boss")[0].transform;
    }
    void Update()
    {
        if(gameObject)
            LookAtPlayer();
    }
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        try
        {
            if (transform.position.x < player.position.x && isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
            else if (transform.position.x > player.position.x && !isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }
        }
        catch { Debug.Log("Player Destroyed!"); }
    }   
}
