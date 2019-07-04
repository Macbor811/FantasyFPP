using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private double TimePassed=0;
    public double TimeBetweenShoots = 10;
    public GameObject Emiter;
    public GameObject ArrowPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name== "FPSController")
        {
            TimePassed += Time.deltaTime;
            if (TimePassed >= TimeBetweenShoots)
            {
                Shoot();
                TimePassed = 0;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        TimePassed = 0;
    }
    private void Shoot()
    {
        Instantiate(ArrowPrefab, Emiter.transform.position,Emiter.transform.rotation);
    }
}
