using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    public Transform player;
    public bool isFlipped = false;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletPos;
    [SerializeField] GameObject splash;
    [SerializeField] GameObject electrolizedEffect;
    
    float fireRate = 4f;
    public float offset = 0.01f;

    bool takingDamage = false;
    public Slider healthBar;
    public int health;
    public int damage = 2;

    public void switchTakingDamage()
    {
        takingDamage = !takingDamage;
    }
    private void Update()
    {
        healthBar.value = health;
    }
    public void Shoot()
    {
        Instantiate(splash, transform.position, Quaternion.identity);
        Instantiate(bullet, bulletPos.transform.position, Quaternion.identity);
    }


    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && takingDamage == false)
        {
            health -= damage;
            Instantiate(electrolizedEffect, transform.position, Quaternion.identity);

        }
    }

}
