using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 postion;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            postion = rigidbody.transform.position; 
            postion.y += 1;
            rigidbody.transform.position += postion;
        }
    }
}
