using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerController : MonoBehaviour
{
    Animator animator;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;
    SpriteRenderer sprite; //For explosion


    float keyHorizontal;
    bool keyJump;
    bool keyMelee;
    bool keyShoot;

    //For dashing
    bool canDash;
    private int dashDirection;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    [SerializeField] ParticleSystem dashParticle;
    [SerializeField] ParticleSystem dust;
    [SerializeField] GameObject DefaultMusic;


    bool isGrounded; //bool initialize as False
    bool isShooting;
    bool isTakingDamage;
    bool isInvincible;
    bool isFacingRight;

    bool hitSideRight; //determine for hit direction animation

    float shootTime;
    bool keyShootRelease;

    public int currentHealth;
    public int maxHealth = 10;
    public int explosionDamage = 0; //explosion damage to the enemies

    GameObject explodeEffect;

    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float jumpSpeed = 3.7f;

    //For Grenade
    //public ProjectileBehavior ProjectilePrefab;
    bool keyGrenade;
    private bool canThrow = true;
    public GrenadeScript LaunchableProjectPrefab;
    [SerializeField] Transform thrownGrenadePosition;

    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5;
    [SerializeField] Transform bulletShootPosition;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject explodeEffectPrefab;

    //SFX
    [SerializeField] private AudioSource getHitSFX;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip throwGrenadeSFX;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); //get a handle to the animator
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        //Sprite defaults to facing right
        isFacingRight = true;

        currentHealth = maxHealth;
        dashTime = startDashTime;
        canThrow = true;

    }


    private void FixedUpdate() //Check to see if we are on the ground or not
    {
        isGrounded = false;
        Color raycastColor;
        RaycastHit2D raycastHit;
        float raycastDistance = 0.05f;
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        //Ground check
        Vector3 box_origin = box2d.bounds.center;
        box_origin.y = box2d.bounds.min.y + (box2d.bounds.extents.y / 4f);
        Vector3 box_size = box2d.bounds.size;
        box_size.y = box2d.bounds.size.y / 4f;
        raycastHit = Physics2D.BoxCast(box_origin, box_size, 0f, Vector2.down, raycastDistance, layerMask);
        //player box coliding with ground layer
        if (raycastHit.collider != null)
        {
            isGrounded = true;

        }
        //draw debug lines
        raycastColor = (isGrounded) ? Color.green : Color.red;
        Debug.DrawRay(box_origin + new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
        Debug.DrawRay(box_origin - new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
        Debug.DrawRay(box_origin - new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);

    }

    // Update is called once per frame
    void Update()
    {
        if (isTakingDamage)
        {
            animator.Play("Player_Hit");
            return;
        }

        PlayerDirectionInput();
        PlayerJumpInput();
        PlayerDashInput();
        PlayerShootInput();
        PlayGrenadeInput();
        PlayerMovement();

    }

    void PlayerDirectionInput()
    {
        //Get keyboard input
        keyHorizontal = Input.GetAxisRaw("Horizontal"); //defind left and right movement
    }

    void PlayerJumpInput()
    {
        keyJump = Input.GetKeyDown(KeyCode.Space); //true/false
    }

    void PlayGrenadeInput()
    {
        keyGrenade = Input.GetKeyDown(KeyCode.G);

        if (keyGrenade && canThrow == true)
        {
            AudioSource.PlayClipAtPoint(throwGrenadeSFX, transform.position);
            canThrow = false;
            Instantiate(LaunchableProjectPrefab, thrownGrenadePosition.position, transform.rotation);
            StartCoroutine(CoolDown());

        }
    }

    //For Grenade Cooldown
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(4.5f); //4 second
        canThrow = true;
    }


    void PlayerDashInput()
    {
        if (dashDirection == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                if (keyHorizontal < 0)
                {
                    dashDirection = 1;
                }
                else if (keyHorizontal > 0)
                {
                    dashDirection = 2;
                }
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                dashDirection = 0;
                dashTime = startDashTime;
                rb2d.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;

                if (dashDirection == 1)
                {
                    rb2d.velocity = Vector2.left * dashSpeed;
                }
                else if (dashDirection == 2)
                {
                    rb2d.velocity = Vector2.right * dashSpeed;
                }
                
            }
        }
    }

    void PlayerShootInput()
    {
        float shootTimeLength = 0;
        float keyShootReleaseTimeLength = 0;

        keyShoot = Input.GetKey(KeyCode.C);//C key for shooting

        if (keyShoot && keyShootRelease) //Keep Press the key
        {
            isShooting = true;
            keyShootRelease = false;
            shootTime = Time.time;
            //Shoot Bullet
            //Invoke("ShootBullet()", 0.1f);
            ShootBullet();
        }
        if (!keyShoot && !keyShootRelease) //key is up
        {
            keyShootReleaseTimeLength = Time.time - shootTime; //Capture how long the key is down for
            keyShootRelease = true;
        }
        if (isShooting)
        {
            shootTimeLength = Time.time - shootTime;
            if (shootTimeLength > 0.25f || keyShootReleaseTimeLength >= 0.15f) //How long the shooting is for and how long the key is press for
            {
                isShooting = false;
            }
        }


    }

    void PlayerMovement()
    {
        if (keyHorizontal < 0) //Going left
        {
            if (isFacingRight)
            {
                Flip();
            }

            if (isGrounded)
            {
                if (isShooting)
                {
                    CreateDust();
                    animator.Play("Player_RunShoot");
                }
                else
                {
                    CreateDust();
                    animator.Play("Player_Run");
                }

            }

            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
        }
        else if (keyHorizontal > 0) //Going right
        {
            if (!isFacingRight)
            {
                Flip();
            }

            if (isGrounded)
            {
                if (isShooting)
                {
                    CreateDust();
                    animator.Play("Player_RunShoot");
                }
                else
                {
                    CreateDust();
                    animator.Play("Player_Run");
                }
            }
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
        }
        else //No moving at all
        {
            if (isGrounded)
            {
                if (isShooting)
                {
                    animator.Play("Player_Shoot");
                }
                else
                {
                    animator.Play("Player_Idle");
                }
            }
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }


        if (keyJump && isGrounded) //Only jumping if we grounded
        {
            if (isShooting)
            {
                animator.Play("Player_JumpShoot");
            }
            else
            {
                animator.Play("Player_Jump");
            }

            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            canDash = true;
        }

        if (!isGrounded)
        {
            if (isShooting)
            {
                animator.Play("Player_JumpShoot");
            }
            else
            {
                animator.Play("Player_Jump");
            }

            if (canDash)
            {

                if ((Input.GetKeyDown(KeyCode.LeftShift) && keyHorizontal != 0) || (Input.GetKeyDown(KeyCode.RightShift) && keyHorizontal != 0))
                {
                    createDashEffect();
                    PlayerDashInput();
                    rb2d.velocity = new Vector2(rb2d.velocity.x, dashSpeed);
                    canDash = false;
                }
            }


        }
    }

    void Flip()
    {
        //CreateDust();
        isFacingRight = !isFacingRight; //Toggle
        transform.Rotate(0f, 180f, 0f); //Rotate the character
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletShootPosition.position, Quaternion.identity);
        bullet.name = bulletPrefab.name;
        bullet.GetComponent<BulletScript>().SetDamageValue(bulletDamage);
        bullet.GetComponent<BulletScript>().SetBulletSpeed(bulletSpeed);
        bullet.GetComponent<BulletScript>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        bullet.GetComponent<BulletScript>().Shoot();
    }

 

    public void HitSide(bool rightSide)
    {
        hitSideRight = rightSide;
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible) //Only take damage if the enemy is not invincible
        {
            getHitSFX.Play(); //SFX
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth); //Current health should never go below 0 and above max health
            UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
            if (currentHealth <= 0) //If enemy health <= 0, enemy is defeat
            {
                Defeat();
            }
            else
            {
                StartDamageAnimation();
            }
        }

    }

    void StartDamageAnimation()
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            isInvincible = true; //Showing damage animation without taking extra hit
            float hitForceX = 0.25f; //X and y direction force when taking damage
            float hitForceY = 1.5f;
            if (hitSideRight) hitForceX = -hitForceX; //hit on the right side, we want to go left and vice versa
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);

        }
    }

    void StartDefeatAnimation() //For player
    {
        explodeEffect = Instantiate(explodeEffectPrefab);
        explodeEffect.name = explodeEffectPrefab.name;
        explodeEffect.transform.position = sprite.bounds.center; // Explode from the center
        explodeEffect.GetComponent<ExplosionScript>().SetDamageValue(this.explosionDamage);
        Destroy(explodeEffect, 2f);
    }

    void StopDefeatAnimation()//In case if we want to stop manually
    {
        Destroy(explodeEffect);
    }

    void StopDamageAnimation() //Called at the end of the animation, stop taking damage
    {
        isTakingDamage = false;
        isInvincible = false; //no longer invincible
        animator.Play("Player_Hit", -1, 0f); //Reset animation to have no loop
    }

    void CreateDust()
    {
        dust.Play();
    }

    void createDashEffect()
    {
        dashParticle.Play();
    }

    void Defeat()
    {
        AudioSource.PlayClipAtPoint(deathSFX, transform.position);
        StartDefeatAnimation();
        //Time.timeScale = 1f;
        //LoseMenu.SetActive(true); --------- FROM KIET
        Destroy(gameObject); //remove character from the screen
        Destroy(DefaultMusic);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }

}
