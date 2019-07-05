using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlood : MonoBehaviour
{
    public GameObject blood;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        DealDamage dmg = collision.gameObject.GetComponent<DealDamage>();
        if (dmg != null)
        {
            Vector3 direction = transform.position - collision.transform.position;
            direction.Normalize();

            Instantiate(blood, transform.position, Quaternion.LookRotation(direction));
        }
    }
}
