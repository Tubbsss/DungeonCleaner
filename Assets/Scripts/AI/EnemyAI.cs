using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//===============================================================================================================
// Adapted from script by Dave/GameDevelopment - https://www.youtube.com/watch?v=UjkSFoLxesw
//===============================================================================================================

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer, whatIsWall;

    public bool aiEnabled = false;

    //Patrolling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, wallBlockingSight;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (aiEnabled && !agent)
        {
            agent.enabled = true;
        }

        if (aiEnabled)
        {
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            //wallBlockingSight = Physics.Raycast(transform.position, transform.forward, sightRange, whatIsWall);

            if (!playerInSightRange && !playerInAttackRange)
            {
                Patrolling();
                Debug.Log("Patrolling");
            }
            else if (playerInSightRange && !playerInAttackRange) //&& !wallBlockingSight)
            {
                ChasePlayer();
                Debug.Log("Chasing");
            }
            else if (playerInSightRange && playerInAttackRange) //&& !wallBlockingSight)
            {
                AttackPlayer();
                Debug.Log("Attacking");
            }
            else if (playerInSightRange && !playerInAttackRange) //&& wallBlockingSight)
            {
                Patrolling();
                Debug.Log("Patrolling");
            }
        }

        if (!aiEnabled && agent)
        {
            agent.enabled = false;
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
            //Debug.Log("Searching for walkpoint");
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            //Debug.Log("Going to walkpoint");
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //Debug.Log(distanceToWalkPoint.magnitude);

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 2f)
        {
            walkPointSet = false;
            //Debug.Log("Walkpoint false");
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomY = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            if (!Physics.CheckSphere(walkPoint, 0.5f, whatIsWall))
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(walkPoint, path);
                if (path.status != NavMeshPathStatus.PathPartial)
                {
                    walkPointSet = true;
                }
            }
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code here



            ///

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);
    }
}
