using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public float rayDist;
    public bool movingRight;
    public Transform groundDetect;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, 2.0f, LayerMask.GetMask("Ground"));
        if (groundCheck.collider == false)
        {
            if (movingRight)
            {
                transform.localEulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }
}
