using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//===============================================================================================================
// Adapted from script by Dave/GameDevelopment - https://www.youtube.com/watch?v=UjkSFoLxesw
//===============================================================================================================

public class RatAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public LayerMask whatIsGround, whatIsPlayer, whatIsWall;

    public bool aiEnabled = false;

    public bool isDead = false;

    private Pickup pickupScript;

    public AudioSource audioSource;
    public AudioClip ratSqueak;
    public AudioClip ratDeath;

    [SerializeField]
    private float squeakDelay = 5f;
    private float squeakDelayCounter = 0f;

    //Patrolling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    private void Start()
    {
        pickupScript = GetComponent<Pickup>();
    }

    private void Awake()
    {
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
            Patrolling();
            if (!isDead)
            {
                RatSqueak();
            }
        }

        if (!aiEnabled && agent)
        {
            agent.enabled = false;
        }
    }

    private void RatSqueak()
    {
        if (squeakDelayCounter > 0)
        {
            squeakDelayCounter -= Time.deltaTime;
        }
        else if (squeakDelayCounter <= 0)
        {
            squeakDelayCounter = squeakDelay;
            if (audioSource != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.clip = ratSqueak;
                audioSource.Play();
            }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && pickupScript.isPickedUp != true)
        {
            //Destroy(collision.gameObject);
            //collision.gameObject.GetComponent<MeshRenderer>().material = pickupMaterial;
            aiEnabled = false;
            isDead = true;
            if (audioSource != null)
            {
                audioSource.clip = ratDeath;
                audioSource.loop = false;
                audioSource.Play();
            }
            pickupScript.enabled = true;
            pickupScript.isPickupable = true;
        }
    }
}
