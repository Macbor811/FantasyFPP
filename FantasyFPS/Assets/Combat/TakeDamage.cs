using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int HP=100;
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


    public void takeDamage(int damage)
    {
        Debug.Log("Taken damage: " + damage);
        HP -= damage;
    }
}
