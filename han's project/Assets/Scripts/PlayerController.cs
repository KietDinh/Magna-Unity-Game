using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;

    

    float keyHorizontal;
    bool keyJump;
    bool keyMelee;
    bool keyShoot;

    bool isGrounded; //bool initialize as False
    bool isShooting;
    bool isTakingDamage;
    bool isInvincible;
    bool isMelee;
    bool isFacingRight;

    bool hitSideRight; //determine for hit direction animation

    float shootTime;
    bool keyShootRelease;

    public int currentHealth;
    public int maxHealth = 10;

    [SerializeField] float moveSpeed = 1.5f;
    [SerializeField] float jumpSpeed = 3.7f;

    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5;
    [SerializeField] Transform bulletShootPosition;
    [SerializeField] GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); //get a handle to the animator
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        //Sprite defaults to facing right
        isFacingRight = true;

        currentHealth = maxHealth;

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
        raycastHit = Physics2D.BoxCast(box_origin, box_size, 0f, Vector2.down,raycastDistance, layerMask);
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
        PlayerShootInput();
        PlayerMovement();
        
    }

    void PlayerDirectionInput()
    {
        //Get keyboard input
        keyHorizontal = Input.GetAxisRaw("Horizontal"); //defind left and right movement
        //keyMelee = Input.GetKey(KeyCode.C); //C key for melee
        
    }

    void PlayerJumpInput()
    {
        keyJump = Input.GetKeyDown(KeyCode.Space); //true/false
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
        if(!keyShoot && !keyShootRelease) //key is up
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
                    animator.Play("Player_Shoot");
                }
                else
                {
                    animator.Play("Player_Walk");
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
                    animator.Play("Player_Shoot");
                }
                else
                {
                    animator.Play("Player_Walk");
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
            animator.Play("Player_Jump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        if (!isGrounded)
        {
            animator.Play("Player_Jump");
        }
    }

    void Flip()
    {
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
            float hitForceX = 0.50f; //X and y direction force when taking damage
            float hitForceY = 1.5f;
            if (hitSideRight) hitForceX = -hitForceX; //hit on the right side, we want to go left and vice versa
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(new Vector2(hitForceX, hitForceY), ForceMode2D.Impulse);

        }
    }

    void StopDamageAnimation() //Called at the end of the animation, stop taking damage
    {
        isTakingDamage = false;
        isInvincible = false; //no longer invincible
        animator.Play("Player_Hit", -1, 0f); //Reset animation to have no loop
        
    }

    void Defeat()
    {
        Destroy(gameObject); //remove character from the screen
    }

}
