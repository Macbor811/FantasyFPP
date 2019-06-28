using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour
{
    // Start is called before the first frame update
    public float lookRadius = 10f;
    public float attackRange = 2f;
    Animator animator;
    Transform target;
    NavMeshAgent agent;
    public GameObject[] partolPoints;
    int currentPoint = 0;
    bool isChase = false;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("hero").transform;
        Debug.Log("hero: " + target.position);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Debug.Log(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        handleAI();
        handlePatrol();
        handleCombat();
        
    }

    void attack()
    {
        animator.SetTrigger("attack");
        hasAttacked = true;
    }

    bool hasAttacked = false;
    void handleCombat()
    {
        float distance = agent.remainingDistance;
        if (isChase &&   agent.remainingDistance < attackRange && !hasAttacked)
        {
            Debug.Log("Attacking");
            attack();
        }
        else if (hasAttacked && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            hasAttacked = false;
        }
    }

    void handlePatrol()
    {
        if (!isChase)
        {
            Debug.Log(partolPoints[currentPoint].transform.position);
            Debug.Log(transform.position);
            Debug.Log("length: " + partolPoints.Length);
            if (transform.position.x == partolPoints[currentPoint].transform.position.x &&
                transform.position.z == partolPoints[currentPoint].transform.position.z)
            {
                if (currentPoint + 1 >= partolPoints.Length)
                {
                    currentPoint = 0;
                }
                else
                {
                    currentPoint++;
                }
            }
            else
            {
                agent.SetDestination(partolPoints[currentPoint].transform.position);
            }
        }
    }

    void handleAI()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            isChase = true;
            animator.SetBool("isChase", true);
        }
        else
        {
            isChase = false;
            animator.SetBool("isChase", false);
        }
    }

    void onDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
