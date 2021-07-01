using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PieceAnimations : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponent<Animator>();
        agent=GetComponent<NavMeshAgent>();
    }
    void Update()
    {
       if(agent!=null)
            //Enables the walking animation based of the magnitude of this object
            SetWalking(agent.velocity.magnitude > 0.5f);
       else
            //If this object has no nav mesh agent this means he has to be idle
            SetWalking(false);
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }
    public void TriggerDeath()
    {
        animator.SetTrigger("Death");
    }
    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }
}
