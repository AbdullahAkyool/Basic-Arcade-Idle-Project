using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelperMovementController : CharacterMovementControllerBase
{
    private NavMeshAgent agent;
    public Animator anim;
    public Transform currentTarget;
    public Transform spawnerMachine;
    public Transform transformerMachine;
    
    public override void Move()
    {
        agent.SetDestination(currentTarget.transform.position);
    }
}
