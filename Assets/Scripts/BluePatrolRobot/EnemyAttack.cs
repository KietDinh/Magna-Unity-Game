using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PatrolBehavior component needed
[RequireComponent(typeof(PatrolBehavior))]
[RequireComponent(typeof(Animator))]
public class EnemyAttack : MonoBehaviour
{
    [Header("Base Info")]
    public GameObject target; // the object being targeted, aka the player/ must have PlayerController component
    public LayerMask targetLayer; // the layer of the player, for raycasting (keep it 0:Default)
    public Sprite projectileSprite; // Sprite for enemy arm, projectile

    [Header("Modifiers")]
    public float sightDist = 5f; // How far can the enemy see the target?
    public int damageAmt = 1; // how much damage will the attack take from the player
    public float atkCooldown = 0f; // time (in seconds) until the next attack, (0 seconds == rapid fire attacks while in sight)
    public float projectileSpeed = 3f; // speed of the arm projectile

    private Vector2 sightDir = Vector2.right; // Where is the enemy looking (default looking right)
    private RaycastHit2D targetSeen; // can the target be seen?
    private PatrolBehavior patrolBehavior; // the PatrolBehavior component of this enemy
    private Animator animator; // the Animator component of this enemy
    private float atkReadyTime; // the next time an attack is ready
    private GameObject projObject; // arm sprite as a gameobject

    void Awake()
    {
        patrolBehavior = GetComponent<PatrolBehavior>();
        animator = GetComponent<Animator>();
        atkReadyTime = Time.time;

        // inti setup for projectile
        projObject = new GameObject(name +" projectile");
        projObject.transform.position = transform.position;
        projObject.AddComponent<SpriteRenderer>();
        projObject.GetComponent<SpriteRenderer>().sprite = projectileSprite;
        projObject.SetActive(false);
    }

    void Update()
    {
        targetSeen = Physics2D.Raycast(transform.position, // enemy position
                     new Vector2(Mathf.Abs(sightDir.x) * (patrolBehavior.isMovingRight() ? 1 : -1), sightDir.y), // direction enemy attacks, based on movingRight in PatrolBehavior script
                     sightDist, targetLayer); // sight distance + layer of target
    }

    void FixedUpdate() {
        if (targetSeen && targetSeen.rigidbody.gameObject.CompareTag(target.tag) && atkReadyTime <= Time.time) { // is targetSeen not null AND has the same tag as Target AND attackcooldown done
            // Debug.Log(name + " attacks " + target.name);

            // Attack player
            animator.SetBool("attacking", true);
           
            // Projectile shot animation, setup
            projObject.transform.position = transform.position;
            projObject.transform.rotation = transform.rotation;
            projObject.SetActive(true);

            // Delay the next ATK, when will the next attack happen
            atkReadyTime = atkCooldown + Time.time;
        }

        if (animator.GetBool("attacking")) {
            // Shoots arm
            projObject.transform.position = Vector2.MoveTowards(projObject.transform.position, target.transform.position, projectileSpeed);
            
            if(new Vector2Int((int)projObject.transform.position.x, (int)projObject.transform.position.y) // checking if projectile hit target
                == new Vector2Int((int)target.transform.position.x, (int)target.transform.position.y)) {
                // Attack player, (calculation)
                animator.SetBool("attacking", false);
                target.GetComponent<PlayerController>().TakeDamage(Mathf.Abs(damageAmt));
                projObject.SetActive(false);
            }
        }
    }
}
