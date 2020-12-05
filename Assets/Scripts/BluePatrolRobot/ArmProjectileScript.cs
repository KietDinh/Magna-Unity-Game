using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmProjectileScript : MonoBehaviour
{
    float moveSpeed = 7f;
    public int contactDamage = 1;
    Rigidbody2D rb2d;

    PlayerController target;
    Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindObjectOfType<PlayerController>();
        if (target)
        {
            moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
        }
        
        rb2d.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

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
