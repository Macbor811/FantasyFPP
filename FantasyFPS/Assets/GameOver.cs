using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameOver : MonoBehaviour
{
public GameObject blood;
public GameObject gameOver;
public GameObject FirstPersonControler;
public float HP = 100;
// Start is called before the first frame update
void Start()
{

}

// Update is called once per frame
void Update()
{
    if (HP <= 0)
    {
            //death
            gameOver.SetActive(true);
            GetComponent<FirstPersonController>().enabled = false;
    }
}
private void OnCollisionEnter(Collision collision)
{
    DealDamage dmg = collision.gameObject.GetComponent<DealDamage>();
    if (dmg != null)
    {
        HP -= dmg.damage;

        Vector3 direction = transform.position - collision.transform.position;
        direction.Normalize();

        Instantiate(blood, transform.position, Quaternion.LookRotation(direction));
    }
}
}
