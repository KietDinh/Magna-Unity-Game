using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    
    Transform player;
    GameObject youreDeath;
    
    public float attackRange = 7f;
    public float fireRate = 3f;
    public float speed = 1f;
    public int contactDamageToPlayer = 1;
    bool isFlipped = false;
    bool roomArea = false;
    bool lessThan75 = false;
    bool lessThan50 = false;
    bool lessThan25 = false;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bigBullet;
    [SerializeField] GameObject bulletPos;
    [SerializeField] GameObject splash;
    [SerializeField] GameObject bigSplash;

    [SerializeField] GameObject electrolizedEffect;
    [SerializeField] GameObject smokeEffect;
    [SerializeField] GameObject bossExplodeEffect;
    [SerializeField] GameObject Block;
    private GameObject instantiatedElectrolizeObject;
    private GameObject Smoke;
    Animator animator;
    bool takingDamage = false;
    public Slider healthBar;
    public int health = 100;
    public int bulletDamageToBoss = 5;
    public int grenadeDamageToBoss = 15;
    private Material matWhite;
    private Material matDefault;
    SpriteRenderer sr;

    [SerializeField] GameObject BossMusic;
    [SerializeField] GameObject DefaultMusic;
    [SerializeField] GameObject WinMusic;
    GameObject instanceBossMusic;
    GameObject instanceWinMusic;

    public float fightStartInvisibleTime = 4;
    float redTime = 3;
    bool invisible = true;
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        youreDeath = transform.GetChild(0).gameObject;

        animator = GetComponent<Animator>();
        healthBar.gameObject.SetActive(false);
        healthBar.value = health;
        healthBar.maxValue = health;

        sr = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;
    }
    private void Update()
    {
        if(player)
        {
            if (player.transform.position.x > youreDeath.transform.position.x && roomArea == false)
            {
                roomArea = true;
                Instantiate(Block);
                animator.Play("Boss Walking");
                healthBar.gameObject.SetActive(true);
                instanceBossMusic = Instantiate(BossMusic);
                Destroy(DefaultMusic);
            }
            
            healthBar.value = health;
        }
        else
        {
            animator.Play("Idle");
        }

        if (fightStartInvisibleTime > 0)
        {
            sr.color = Color.red;
            fightStartInvisibleTime -= Time.deltaTime;
            invisible = true;
        }
        else if (sr.color == Color.red && redTime > 0)
        {
            invisible = true;
            redTime -= Time.deltaTime;
        }
        else {
            invisible = false;
            sr.color = Color.white;
            redTime = 3;
        }
    }
    public void Shoot()
    {
        Instantiate(splash, bulletPos.transform.position, Quaternion.identity);
        Instantiate(bullet, bulletPos.transform.position, Quaternion.identity);
    }
    public void ShootBig()
    {
        Instantiate(bigSplash, bulletPos.transform.position, Quaternion.identity);
        Instantiate(bigBullet, bulletPos.transform.position, Quaternion.identity);
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

    void ResetMaterial() { sr.material = matDefault; }
    void SwitchWalking() { animator.Play("Shoot"); }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.CompareTag("Bullet") && !instantiatedElectrolizeObject && !invisible))
        {
            takeDamge(bulletDamageToBoss);
        }

        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Hit");
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            player.HitSide(transform.position.x > player.transform.position.x); //determine which side the player hit on
            player.TakeDamage(this.contactDamageToPlayer);
        }

        if (col.CompareTag("Grenade") && !instantiatedElectrolizeObject && !invisible)
        {
            takeDamge(grenadeDamageToBoss);
            var grenade = col.GetComponent<GrenadeScript>();
            grenade.explode();
        }
    }
    void takeDamge(int dmg)
    {
        animator.Play("Idle"); // switch to idle to stop the shooting

        // --- --- boss takes damage effects --- ---
        instantiatedElectrolizeObject = (GameObject)Instantiate(electrolizedEffect, transform.position, Quaternion.identity);
        sr.material = matWhite;
        // --- --- --- --- --- --- --- --- --- ---

        health -= dmg;
        if (health <= 0)
        {
            animator.Play("Idle"); // switch to idle to stop the shooting
            Destroy(gameObject); // destroy boss object
            Destroy(Smoke); // destroy any smoke effects
            Destroy(instantiatedElectrolizeObject); // destroy any electrolize happening
            Instantiate(bossExplodeEffect, transform.position, Quaternion.identity); // generate particle explosion effects

            Destroy(instanceBossMusic);
            instanceWinMusic = Instantiate(WinMusic);
        }
        //else if (health <= 50 && !Smoke)
        //{
        //    Smoke = (GameObject)Instantiate(smokeEffect, transform.position, Quaternion.identity);
        //}
        else // for the white sprite effect, switch back to normal sprite
        {
            Invoke("ResetMaterial", .1f);
            Invoke("SwitchWalking", .5f);
        }
        if (health <= 75 && !lessThan75)
        {
            sr.color = Color.red;
            animator.Play("ShootBig");
            lessThan75 = true;
        }
        else if (health <= 50 && !lessThan50)
        {
            sr.color = Color.red;
            animator.Play("ShootBig");
            lessThan50 = true;
        }
        else if (health <= 25 && !lessThan25)
        {
            sr.color = Color.red;
            animator.Play("ShootBig");
            lessThan25 = true;
        }
    }
}
