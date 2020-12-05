using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletScript : MonoBehaviour
{
    public float movespeed = 6f;

    Rigidbody2D rb2d;

    Transform target;
    public GameObject explosion;
    Vector2 moveDirection;
    public int contactDamage = 1;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectsWithTag("Boss")[0].transform;

        moveDirection = (target.transform.position - transform.position).normalized * movespeed * -1;
        rb2d.velocity = new Vector2(moveDirection.x, 0);
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Left Side")
        {
            //Debug.Log("Hit!");
            Destroy(gameObject);
        }
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Hit");
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            player.HitSide(transform.position.x > player.transform.position.x); //determine which side the player hit on
            player.TakeDamage(this.contactDamage);

        }
    }

    void OnDestroy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
