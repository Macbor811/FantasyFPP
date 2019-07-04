using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShackingAnimation : MonoBehaviour
{
  private Animator animator;

  void Start() {
    animator = GetComponent<Animator>();
  }
  
  void Update() {
    if(Input.GetKeyDown(KeyCode.W)) {
        animator.SetBool("Walk", true);
    } 
    if(Input.GetKeyUp(KeyCode.W)) {
        animator.SetBool("Walk", false);
    }

    if(Input.GetKeyDown(KeyCode.LeftShift)) {
        animator.SetBool("FastWalk", true);
    } 
    if(Input.GetKeyUp(KeyCode.LeftShift)) {
        animator.SetBool("FastWalk", false);
    }

    if(Input.GetMouseButtonDown(0)) {
        animator.SetBool("Attack", true);
    }
    if(Input.GetMouseButtonUp(0)) {
        animator.SetBool("Attack", false);
    }
  }
}
