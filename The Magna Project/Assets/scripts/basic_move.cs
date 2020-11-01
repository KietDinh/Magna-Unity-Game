using System.Collections.Generic;
using UnityEngine;

public class basic_move : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rb;
    public float jumpForce = 20f;
    float mx;

    private void Update()
    {
        mx = Input.GetAxisRaw("Horizontal");
       if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);
        rb.velocity = movement;
    }
    void Jump()
    {
        Vector2 movement = new Vector2(rb.velocity.x, jumpForce);
        rb.velocity = movement;
    }
}
