﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    public float lookRadius = 10f;
    Transform target;   
    NavMeshAgent agent;
    public GameObject[] partolPoints;
    int currentPoint = 0;
    bool isChase = false;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("Player: " + target.position);
        agent = GetComponent<NavMeshAgent>();
        
        Debug.Log(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        handleAI();
        handlePatrol();
        
    }

    void handlePatrol()
    {
        if (!isChase)
        {
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
        }
        else
        {
            isChase = false;
        }
    }

    void onDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
