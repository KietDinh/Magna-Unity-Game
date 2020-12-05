using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Boss_Walk : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rb2d;
    Boss boss;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        try
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        }
        catch
        {
            Debug.Log("Player destroyed");
            player = null;
        }
        rb2d = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, rb2d.position.y);
        Vector2 newPos =  Vector2.MoveTowards(rb2d.position, target, boss.speed * Time.fixedDeltaTime);
        rb2d.MovePosition(newPos);


        if (Vector2.Distance(player.position, rb2d.position) <= boss.attackRange)
        {
            //Debug.Log("in range");
            animator.SetTrigger("Shoot");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Shoot");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
