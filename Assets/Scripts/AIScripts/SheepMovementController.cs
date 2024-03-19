using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepMovementController : CharacterMovementControllerBase
{
    private NavMeshAgent agent;
    public Animator anim;
    public Transform currentTarget;

    public List<Transform> patrolPoints;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(FindRandomTargetCo());
    }

    private void Update()
    {
        Move();
    }

    private Transform FindRandomTarget()
    {
        // Vector3 randomPoint = Vector3.zero;
        // bool foundPoint = false;
        //
        // var randomX = Random.Range(-15, 15);
        // var randomZ = Random.Range(-15, 15);
        //
        // randomPoint = new Vector3(randomX, 0, randomZ);
        // NavMeshHit hit;
        //
        // if (NavMesh.SamplePosition(randomPoint, out hit, 100, NavMesh.AllAreas))
        // {
        //     randomPoint = hit.position;
        //     foundPoint = true;
        // }
        //
        // if (foundPoint)
        // {
        //     return randomPoint;
        // }
        // else
        // {
        //     return Vector3.zero;
        // }
        
        // var randomX = Random.Range(-15, 15);
        // var randomZ = Random.Range(-15, 15);
        //
        // var randomPoint = new Vector3(randomX, 0, randomZ);
        // return randomPoint;
        
        int randomIndex = Random.Range(0, patrolPoints.Count);
        return patrolPoints[randomIndex];
    }

    IEnumerator FindRandomTargetCo()
    {
        while (true)
        {
            currentTarget = FindRandomTarget();
            yield return new WaitForSeconds(10f);
        }
    }

    public override void Move()
    {
        if (!agent) return;

        var distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance >= 2)
        {
            agent.isStopped = false;
            agent.speed = 2f;
            agent.SetDestination(currentTarget.position);
            anim.SetBool("isWalk", true);
            anim.SetBool("isIdle", false);
        }
        else
        {
            agent.isStopped = true;
            agent.speed = 0f;
            anim.SetBool("isWalk", false);
            anim.SetBool("isIdle", true);
        }
    }
}