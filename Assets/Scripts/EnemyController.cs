using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]


public class EnemyController : MonoBehaviour
{

    Animator animator;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;
    SpriteRenderer sprite;

    bool isInvincible; //Take damage or not on the enemy or main character

    GameObject explodeEffect;

    public int currentHealth;
    public int maxHealth = 1; //1 bullet to kill them
    public int contactDamage = 1;
    public int explosionDamage = 0; //Determine how much the explosion affect on the player

    //SFX
    //public AudioSource bombExplodeSFX;
    [SerializeField] private AudioClip deathSFX;

    [SerializeField] GameObject explodeEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

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

    void StartDefeatAnimation()
    {
        explodeEffect = Instantiate(explodeEffectPrefab);
        explodeEffect.name = explodeEffectPrefab.name;
        explodeEffect.transform.position = sprite.bounds.center; //Explode from the center
        explodeEffect.GetComponent<ExplosionScript>().SetDamageValue(this.explosionDamage);
        Destroy(explodeEffect, 2f);
    }

    void StopDefeatAnimation()//In case if we want to stop manually
    {
        Destroy(explodeEffect);
    }

    void Defeat()
    {
        AudioSource.PlayClipAtPoint(deathSFX, transform.position);
        StartDefeatAnimation();
        Destroy(gameObject); //Enemy will remove itself from the scene
    }

    private void OnTriggerStay2D(Collider2D collision) //called as long as the box touch each other
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.HitSide(transform.position.x > player.transform.position.x); //determine which side the player hit on
            player.TakeDamage(this.contactDamage);

        }
    }

}
