using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    bool isInvincible; //Take damage or not on the enemy or main character

    public int currentHealth;
    public int maxHealth = 1; //1 bullet to kill them
    public int contactDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible) //Only take damage if the enemy is not invincible
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth); //Current health should never go below 0 and above max health
            if(currentHealth <= 0) //If enemy health <= 0, enemy is defeat
            {
                Defeat();
            }
        }
    }

    void Defeat()
    {
        Destroy(gameObject); //Enemy will remove itself from the scene
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
