using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private Animator weaponAnimator;
    public GameObject MagicProjectile;
    public GameObject weaponHolder;
    bool damageDealt = false;
    //private Obb obb;

    private int chosenWeapon = 1;
    // Start is called before the first frame update
    void Start()
    {
        weaponAnimator = weaponHolder.GetComponent<Animator>();

    }

    void CheckCollisions(DealDamage damageSource)
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            if (ObbCollisionDetection.Intersects(weaponHolder.GetComponentInChildren<BoxCollider>(), enemy.GetComponent<BoxCollider>()))
            {
                var hittable = enemy.GetComponent<TakeDamage>();
                if (hittable != null)
                {
                    damageDealt = true;
                    hittable.takeDamage(damageSource.damage);
                }
                else
                {
                    throw new MissingComponentException("Enemy tagged object doesn't implement IHittable!");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (!damageDealt)
            {
                CheckCollisions(weaponHolder.GetComponentInChildren<DealDamage>());
            }           
        }
        else
        {
            damageDealt = false;
        }

        if (Input.GetButtonDown("Button1"))
            chosenWeapon = 1;
        if (Input.GetButtonDown("Button2"))
            chosenWeapon = 2;
        if (Input.GetButtonDown("Button3"))
            chosenWeapon = 3;
        if (Input.GetMouseButtonDown(0))
            PlayerAttack();
    }
    private void PlayerAttack()
    {
        switch (chosenWeapon)
        {
            case 1:
                {
                    Attack1();
                    break;
                }
            case 2:
                {                   
                    Attack2();
                    break;
                }

            case 3:
                Attack3();
                break;
        }
    }
    private void Attack1()
    {
        // Debug.Log("Attack1");
        weaponAnimator.SetTrigger("StabAttack");

    }
    private void Attack2()
    {
        //Debug.Log("Instantied with: " + transform.rotation + " " + transform.position);
        Instantiate(MagicProjectile, transform.position, transform.rotation);
    }
    private void Attack3()
    {
        Debug.Log("Attack3");
    }
}
