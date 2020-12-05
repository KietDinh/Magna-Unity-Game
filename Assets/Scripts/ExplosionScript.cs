using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    int damage = 0;

    public void SetDamageValue(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (this.damage > 0) //Apply damage if there is damage to apply
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                player.HitSide(transform.position.x > player.transform.position.x); //If the explosion is the right side
                player.TakeDamage(this.damage); //Apply damage to the player
            }
        }
    }
}
