using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TailContoller : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 5.0f;
    private Transform currentWaypoint; // keep track of current waypoint
    private int currentIndex; // current position in the waypoint array
    bool wait = false;
    bool forwardDirection = true;
    public float delayTime = 2f;
    private GameObject player;
    private bool xAxis = false, yAxis = false, zAxis = false;

  
    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = patrolPoints[0]; // set initial waypoint
        currentIndex = 0; // set initial index
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (wait == false)
        {

            Vector3 vector = handleMove();
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
            if ((player.transform.position.x < this.transform.position.x + 1) && (player.transform.position.x > this.transform.position.x - 1)
                   && (player.transform.position.z < this.transform.position.z + 1) && (player.transform.position.z > this.transform.position.z - 1))
            {
                handlePlayermove(vector);
                Debug.Log("bee");
            }
        }
    }
    Vector3 handleMove()
    {
      

        var deltaPosition = currentWaypoint.transform.position - transform.position;
        var calcVector = deltaPosition.normalized * speed * Time.deltaTime;
        transform.Translate(calcVector);
        return calcVector;
        // this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(deltaPosition), 4 * Time.deltaTime);
    }

    void handlePlayermove(Vector3 vector)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.Translate(vector,Space.World);
        player.GetComponent<CharacterController>().enabled = true;
    }
    void handlePlayerMove()
    {
        setAxis();
        if (xAxis)
        {
            var tmp = player.transform.position;
            tmp.x+= 5f * speed;
            player.transform.position = tmp;
        }
        if (yAxis)
        {
            var tmp = player.transform.position;
            tmp.y += 5f * speed;
            player.transform.position = tmp;
        }
        if (zAxis)
        {
            var tmp = player.transform.position;
            tmp.z += 5f*speed;
            player.transform.position = tmp;
        }
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
    void setAxis()
    {
        if (forwardDirection)
        {
            if ((patrolPoints[currentIndex].position.x - patrolPoints[currentIndex + 1].position.x) != 0)
                xAxis = true;
            else xAxis = false;
            if ((patrolPoints[currentIndex].position.y - patrolPoints[currentIndex + 1].position.y) != 0)
                yAxis = true;
            else yAxis = false;
            if ((patrolPoints[currentIndex].position.z - patrolPoints[currentIndex + 1].position.z) != 0)
                zAxis = true;
            else zAxis = false;

        }
    }
}
