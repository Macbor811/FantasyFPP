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

  
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0; // set initial index
        currentWaypoint = patrolPoints[currentIndex]; // set initial waypoint
        player = GameObject.FindGameObjectWithTag("hero");
    }

    // Update is called once per frame
    void Update()
    {
        if (wait == false)
        {
            Vector3 vector = handleMove();
            if (Vector3.Distance(currentWaypoint.position, transform.position) <= 0.2f)
            {
                if (forwardDirection)
                {
                    if (currentIndex == patrolPoints.Length - 1)
                    {
                        forwardDirection = false;
                        currentIndex--;
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
                        forwardDirection = true;
                        currentIndex++;
                    }
                    else
                    {
                        currentIndex--;
                    }
                }
                currentWaypoint = patrolPoints[currentIndex];
                delay(delayTime);
            }
              if ((player.transform.position.x < this.transform.position.x + 2) && (player.transform.position.x > this.transform.position.x - 2) && 
                   (player.transform.position.z < this.transform.position.z + 2) && (player.transform.position.z > this.transform.position.z - 2)
                   && (player.transform.position.y >= this.transform.position.y && player.transform.position.y < this.transform.position.y + 3 ))
            { 
                Debug.Log("TRANSPORT GRACZA");
                handlePlayermove(vector);
                
            }
            
                Debug.Log("gracza: x " + player.transform.position.x);
                Debug.Log("kafelka: x " + transform.position.x);
                Debug.Log("gracza: z " + player.transform.position.z);
                Debug.Log("kafelka: z " + transform.position.z);

                
            
        }
    }
    Vector3 handleMove()
    {
        var deltaPosition = currentWaypoint.position - transform.position;
        var calcVector = deltaPosition.normalized * speed * Time.deltaTime;
        transform.Translate(calcVector, Space.World);
        return calcVector;
        // this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(deltaPosition), 4 * Time.deltaTime);
    }

    void handlePlayermove(Vector3 vector)
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.Translate(vector,Space.World);
        player.GetComponent<CharacterController>().enabled = true;
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
