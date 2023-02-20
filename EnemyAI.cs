using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask IsGround, IsPlayer;

    public Vector3 walkPt;
    bool walkPtSet;
    public float walkPtRange;

    public float AttackSpd;
    bool Attacked;

    public float sightRng, atkRng;
    public bool playerInSightRng, playerInAtkRng;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRng = Physics.CheckSphere(transform.position, sightRng, IsPlayer);
        playerInAtkRng = Physics.CheckSphere(transform.position, atkRng, IsPlayer);

        if (!playerInSightRng && !playerInAtkRng) Patrol();
        if (playerInSightRng && !playerInAtkRng) Chase();
        if (playerInSightRng && playerInAtkRng) Attack();
    }

    private void Patrol()
    {
        if (!walkPtSet) SearchWalkPt();

        if (walkPtSet)
        {
            agent.SetDestination(walkPt);
        }
        Vector3 distToWalkPt = transform.position - walkPt;
        
        if (distToWalkPt.magnitude < 1f)
        {
            walkPtSet = false;
        }
    }

    private void SearchWalkPt()
    {
        float randomZ = Random.Range(-walkPtRange, walkPtRange);
        float randomX = Random.Range(-walkPtRange, walkPtRange);

        walkPt = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
  
        if (Physics.Raycast(walkPt, -transform.up, 2f, IsGround))
        {
            walkPtSet = true;
        }

    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
    }
}
