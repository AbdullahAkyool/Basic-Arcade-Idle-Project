using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HelperMovementController : CharacterMovementControllerBase
{
    private NavMeshAgent agent;
    public Animator anim;
    public Transform currentTarget;
    public Transform spawnerMachine;
    public Transform transformerMachine;
    
    private CharacterStackSystem stackSystem;

    public bool canMove;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stackSystem = GetComponent<CharacterStackSystem>();
        currentTarget = spawnerMachine;
    }

    private void Update()
    {
        ChangeTarget();
        Move();
    }

    private void ChangeTarget()
    {
        if (stackSystem.stackedObjects.Count == stackSystem.characterStackLimit)
        {
            currentTarget = transformerMachine;
        }

        if (stackSystem.stackedObjects.Count == 0)
        {
            currentTarget = spawnerMachine;
        }
    }

    public override void Move()
    {
        if(!agent || !currentTarget) return;
        
        var distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance >= 2)
        {
            agent.isStopped = false;
            agent.speed = 2f;
            agent.SetDestination(currentTarget.position);
            anim.SetBool("isWalk",true);
            anim.SetBool("isIdle",false);
        }
        else
        {
            agent.isStopped = true;
            agent.speed = 0f;
            anim.SetBool("isWalk",false);
            anim.SetBool("isIdle",true);
        }
    }
}
