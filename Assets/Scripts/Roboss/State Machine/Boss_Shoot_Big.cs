using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Shoot_Big : StateMachineBehaviour
{
    Transform player;
    Boss boss;
    float nextFire;
    Rigidbody2D rb2d;
    bool shooted = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        rb2d = animator.GetComponent<Rigidbody2D>();
        nextFire = Time.time;
        try
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        }
        catch
        {
            Debug.Log("Bug Here - reference Boss Shoot Big.cs - line 24");
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        if(!shooted)
        {
            boss.ShootBig();
        }
    }
}
