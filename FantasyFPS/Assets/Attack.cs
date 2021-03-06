﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour, IHittable
{

    private Animator weaponAnimator;
    public GameObject MagicProjectile;
    public GameObject weaponHolder;
    public GameObject gameOverScreen;

    private DealDamage damage;
    bool damageDealt = false;
    public int healthPoints = 100;
    public int manaPoints = 100;
    public Text hpText;
    float manaRegen = 0.0f;
    float hpRegen = 0.0f;
    //private Obb obb;

    private int chosenWeapon = 1;
    // Start is called before the first frame update
    void Start()
    {
        weaponAnimator = weaponHolder.GetComponent<Animator>();
        damage = weaponHolder.GetComponentInChildren<DealDamage>();
        hpText.text = "HP: " + healthPoints + "    Mana: " + manaPoints;
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
        this.manaRegen += Time.deltaTime;
        this.hpRegen += Time.deltaTime;
        if(this.hpRegen > 5.0f) {
            if(healthPoints < 100) {
                healthPoints++;
            }
            this.hpRegen = 0.0f;
        }
        if( this.manaRegen > 1.0f) {
            if(this.manaPoints < 100) {
                 this.manaPoints++;
                 hpText.text = "HP: " + healthPoints + "    Mana: " + manaPoints;
            }
             this.manaRegen = 0.0f;
        }
        //if (weaponAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        //{
        //    damage.IsActive = true;
        //}
        //else
        //{
        //    damage.IsActive = false;
        //}

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
        damage.IsActive = true;

    }
    private void Attack2()
    {
        //Debug.Log("Instantied with: " + transform.rotation + " " + transform.position);
        if(this.manaPoints > 20) {
            this.manaPoints -= 20;
            hpText.text = "HP: " + healthPoints + "    Mana: " + manaPoints;
            Instantiate(MagicProjectile, transform.position, transform.rotation);
        }
    }
    private void Attack3()
    {
        Debug.Log("Attack3");
    }

    public void OnHit(DealDamage damage)
    {
        if(Input.GetMouseButton(1)) {
            healthPoints -= (damage.damage - Random.Range(0, damage.damage));
        } else {
            healthPoints -= damage.damage;
        }
          
        hpText.text = "HP: " + healthPoints + "    Mana: " + manaPoints;
        if (healthPoints <= 0)
        {
            gameOverScreen.SetActive(true);
        }
    }
}
