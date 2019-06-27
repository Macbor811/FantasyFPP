using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator anim;
    //CapsuleCollider cc;


    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
            anim.SetTrigger("StabAttack");

    }



    void OnTriggerEnter(Collider c)
    {

    }
}