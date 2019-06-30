using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour, IHittable
{
    // Start is called before the first frame update
    public float lookRadius = 10f;
    public float attackRange = 4f;
    Animator animator;
    Transform target;
    NavMeshAgent agent;
    public GameObject[] patrolPoints;
    int currentPoint = 0;
    bool isChase = false;
    bool isDying = false;


    enum Mode { CHASE, ATTACK, PATROL};

    private Mode mode = Mode.PATROL;

    private DealDamage damage;

    public int healthPoints = 100;


    void Start()
    {
        //base.Start();
        target = GameObject.FindGameObjectWithTag("hero").transform;
        Debug.Log("hero: " + target.position);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        damage = GetComponentInChildren<DealDamage>();
        Debug.Log(transform.position);
        agent.stoppingDistance = attackRange;
    }

    // Update is called once per frame
    void Update()
    {
      if (!isDying)
        {
            handleAI();
            //handlePatrol();
            //handleCombat();
        }
        //base.Update();

    }


    bool hasAttacked = false;

    void handleCombat()
    {    
        if (!hasAttacked)
        {
            animator.SetTrigger("attack");
            hasAttacked = true;
        }
    
    }

    void handlePatrol()
    {
        if (mode == Mode.PATROL && patrolPoints.Length > 0)
        {
            Debug.Log(patrolPoints[currentPoint].transform.position);
            Debug.Log(transform.position);
            Debug.Log("length: " + patrolPoints.Length);
            if (transform.position.x == patrolPoints[currentPoint].transform.position.x &&
                transform.position.z == patrolPoints[currentPoint].transform.position.z)
            {
                if (currentPoint + 1 >= patrolPoints.Length)
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
                agent.SetDestination(patrolPoints[currentPoint].transform.position);
            }
        }
    }

    //bool checkAttackCondition()
    //{
    //    float distance = Vector3.Distance(target.position, transform.position);
    //    return distance <= lookRadius;
    //}

    void handleAI()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            if (distance < attackRange)
            {
                Debug.Log("Attack mode");
                mode = Mode.ATTACK;
                agent.isStopped = true;
                agent.ResetPath();
                handleCombat();
            }
            else
            {
                Debug.Log("Chase mode");
                mode = Mode.CHASE;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    agent.isStopped = false;
                    agent.SetDestination(target.position);
                }    
            }         
        }
        else
        {
            Debug.Log("Patrol mode");
            mode = Mode.PATROL;
            handlePatrol();
        }
        animator.SetBool("isChase", mode == Mode.CHASE);
    }

    void onDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void OnHit(DealDamage damage)
    {    
        healthPoints -= damage.damage;
        Debug.Log("HP left: " + healthPoints);
        if (healthPoints <= 0 && !isDying)
        {
            animator.SetTrigger("death");
            agent.isStopped = true;
            isDying = true;
        }
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }

    public void AttackStart()
    {
        damage.IsActive = true;
    }
    public void AttackEnd()
    {
        damage.IsActive = false;
    }

    public void AttackAnimationEnd()
    {
        hasAttacked = false;
    }
}
