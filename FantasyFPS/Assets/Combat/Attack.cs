using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    bool stabStarted = false;
    public Animation stabAnimation;
    public GameObject MagicProjectile;
    private int choosenWeapon = 1;
    // Start is called before the first frame update
    void Start()
    {
        stabAnimation = GetComponent<Animation>();
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
        // Debug.Log("Attack1");
        if (Input.GetMouseButtonDown(0) && !stabAnimation.isPlaying)
        {
            stabAnimation.Play();
            stabStarted = true;
        }
        else if (!stabAnimation.isPlaying && stabStarted)
        {
            stabAnimation.clip.SampleAnimation(this.gameObject, 0);
            stabStarted = false;
        }

    }
    private void Attack2()
    {
        Debug.Log("Instantied with: " + transform.rotation + " " + transform.position);
        Instantiate(MagicProjectile, transform.position, transform.rotation);
    }
    private void Attack3()
    {
        Debug.Log("Attack3");
    }
}
