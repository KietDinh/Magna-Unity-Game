using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public int contactDamage = 1;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision) //called as long as the box touch each other
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player Hit");
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.HitSide(transform.position.x > player.transform.position.x); //determine which side the player hit on
            player.TakeDamage(this.contactDamage);

        }
    }
}
