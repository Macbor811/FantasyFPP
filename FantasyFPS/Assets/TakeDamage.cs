using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public GameObject blood;
    public float HP=100;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(HP<=0)
        {
            //death
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        DealDamage dmg = collision.gameObject.GetComponent<DealDamage>();
        if (dmg!=null)
        {
            HP -= dmg.damage;

            Vector3 direction = transform.position - collision.transform.position;
            direction.Normalize();

            Instantiate(blood, transform.position, Quaternion.LookRotation(direction));

            Debug.Log(HP);
        }
    }
}
