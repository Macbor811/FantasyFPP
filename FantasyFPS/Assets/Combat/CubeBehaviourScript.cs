using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviourScript : MonoBehaviour
{
    public Collider coll;

    void Start()
    {
        coll = GetComponent<Collider>();
        coll.isTrigger = true;
        coll.attachedRigidbody.useGravity = true;
    }

    // Disables gravity on all rigidbodies entering this collider.
    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
            other.attachedRigidbody.useGravity = false;
    }
}
