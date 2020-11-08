using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletScript : MonoBehaviour
{
    float movespeed = 6f;

    Rigidbody2D rb2d;

    Transform target;
    public GameObject explosion;
    Vector2 moveDirection;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;

        moveDirection = (target.transform.position - transform.position).normalized * movespeed * 100;
        rb2d.velocity = new Vector2(moveDirection.x, 0);
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Left Side")
        {
            Debug.Log("Hit!");
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
