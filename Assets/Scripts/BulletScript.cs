using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    SpriteRenderer sprite;

    float destroyTime; //Count down, need to terminate after so long or out of the scene

    public int damage = 1;

    [SerializeField] float bulletSpeed;
    [SerializeField] Vector2 bulletDirection;
    [SerializeField] float destroyDelay;
    [SerializeField] private AudioClip bulletTouchSFX;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if(destroyTime < 0) //if time run out on the bullet
        {
            Destroy(gameObject);
        }
    }

    public void SetBulletSpeed(float speed)
    {
        this.bulletSpeed = speed;
    }

    public void SetBulletDirection(Vector2 direction)
    {
        this.bulletDirection = direction;
    }

    public void SetDamageValue(int damage)
    {
        this.damage = damage;
    }

    public void SetDestroyDelay(float delay)
    {
        this.destroyDelay = delay;
    }

    public void Shoot()
    {
        sprite.flipX = (bulletDirection.x < 0); //if bullet shooting to the left
        rb2d.velocity = bulletDirection * bulletSpeed;
        destroyTime = destroyDelay;
    }

    private void OnTriggerEnter2D(Collider2D collision) //when bullet collide with other box collider
    {
        if (collision.gameObject.CompareTag("Enemy")) //if it collide with enemy, will react with enemy, destroy itself
        {
            AudioSource.PlayClipAtPoint(bulletTouchSFX, transform.position);
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if(enemy != null)
            {
                enemy.TakeDamage(this.damage); //taking damage from the bullet
            }
            Destroy(gameObject, 0.01f); //destroy after the delay
        }
        
    }
}
