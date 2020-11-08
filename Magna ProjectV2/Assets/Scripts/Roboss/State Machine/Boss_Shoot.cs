using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Shoot : StateMachineBehaviour
{
    Transform player;
    Boss boss;
    float nextFire;
    public float attackRange = 3f;
    Rigidbody2D rb2d;
    float fireRate = 3f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        rb2d = animator.GetComponent<Rigidbody2D>();
        nextFire = Time.time;
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        if (Vector2.Distance(player.position, rb2d.position) > attackRange)
        {
            animator.SetTrigger("Walk");
        }
        else
        {
            if (Time.time > nextFire)
            {
                boss.Shoot();
                nextFire = Time.time + fireRate;
            }
            
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
