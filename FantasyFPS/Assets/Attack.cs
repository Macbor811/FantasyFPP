using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    private int choosenWeapon = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Button1"))
            choosenWeapon = 1;
        if (Input.GetButtonDown("Button2"))
            choosenWeapon = 2;
        if (Input.GetButtonDown("Button3"))
            choosenWeapon = 3;
        if (Input.GetMouseButtonDown(0))
            PlayerAttack();
    }
    private void PlayerAttack()
    {
        switch (choosenWeapon)
        {
            case 1:
                Attack1();
                break;
            case 2:
                Attack2();
                break;
            case 3:
                Attack3();
                break;
        }
    }
    private void Attack1()
    {
        Debug.Log("Attack1");
    }
    private void Attack2()
    {
        Debug.Log("Attack2");
    }
    private void Attack3()
    {
        Debug.Log("Attack3");
    }
}
