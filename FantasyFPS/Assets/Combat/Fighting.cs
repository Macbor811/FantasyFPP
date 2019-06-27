using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighting : MonoBehaviour
{
    Animation anim;
    bool stabStarted = false;
   // bool stabCompleted = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !anim.isPlaying)
        {
            anim.Play();
            stabStarted = true;
        }
        else if (!anim.isPlaying && stabStarted)
        {
            anim.clip.SampleAnimation(this.gameObject, 0);
            stabStarted = false;
        }
        

    }
}
