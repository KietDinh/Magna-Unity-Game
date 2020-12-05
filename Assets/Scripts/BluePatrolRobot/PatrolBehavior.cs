// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PatrolBehavior : MonoBehaviour
// {
//     public float speed;
//     public float rayDist;
//     private bool movingRight;
//     public Transform groundDetect;

//     void Update()
//     {
//         transform.Translate(Vector2.right * speed * Time.deltaTime);
//         RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, 2.0f, LayerMask.GetMask("Ground"));

//         if (groundCheck.collider == false)
//         {
//             if(movingRight)
//             {
//                 transform.eulerAngles = new Vector3(0, -180, 0);
//                 movingRight = false;
//             }
//             else
//             {
//                 transform.eulerAngles = new Vector3(0, 0, 0);
//                 movingRight = true;
//             }
//         }
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : MonoBehaviour
{
    public float speed;
    public float rayDist;
    private bool movingRight = true; // assuming it starts by moving right
    public Transform groundDetect;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundCheck = Physics2D.Raycast(groundDetect.position, Vector2.down, 2.0f, LayerMask.GetMask("Ground"));

        if (groundCheck.collider == false)
        {
            if(movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                // movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                // movingRight = true;
            }
            movingRight = !movingRight;
        }
        Debug.Log(movingRight);
    }


    public bool isMovingRight() { // made a getter for movingRight boolean
        return movingRight;
    }
}
