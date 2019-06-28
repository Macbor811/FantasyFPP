using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TailContoller : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float airplaneSpeed = 20.0f;
    private Transform currentWaypoint; // keep track of current waypoint
    private int currentIndex; // current position in the waypoint array
    bool wait = false;
    bool forwardDirection = true;
    public float delayTime = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = patrolPoints[0]; // set initial waypoint
        currentIndex = 0; // set initial index
    }

    // Update is called once per frame
    void Update()
    {
        if (wait == false)
        {

            handleMove();
            if (Vector3.Distance(currentWaypoint.transform.position, transform.position) <= 0.2f)
            {
                if(forwardDirection)
                {
                    if (currentIndex == patrolPoints.Length - 1)
                    {
                        delay(delayTime);
                        forwardDirection = false;
                    }
                    else
                    {
                        currentIndex++;
                    }
                }
                else
                {
                    if (currentIndex == 0)
                    {
                        delay(delayTime);
                        forwardDirection = true;
                    }
                    else
                    {
                        currentIndex--;
                    }
                }
                currentWaypoint = patrolPoints[currentIndex];
            }
        }
    }
    void handleMove()
    {
        Vector3 deltaPosition = currentWaypoint.transform.position - transform.position;
        Vector3 calcVector = deltaPosition.normalized * airplaneSpeed * Time.deltaTime;
        transform.Translate(calcVector);
        // this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(deltaPosition), 4 * Time.deltaTime);
    }
    void delay(float seconds)
    {
        wait = true;
        Invoke("setActive", seconds);
    }
    public void setActive()
    {
        wait = false;
    }

}
