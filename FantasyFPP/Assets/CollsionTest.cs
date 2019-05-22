using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//przykład użycia CollisionTest
public class CollsionTest : HandleCollsion
{
    protected override void setTagOfObjectCollision()
    {
        this.tagOfObjectCollision = "Obbs";
    }

    
    public override void handleCollision(GameObject objectCauseCollision)
    {
        objectCauseCollision.GetComponent<Renderer>().material.color = Color.red;
    }

    public override void onExitCollision(GameObject objectCauseCollision)
    {
        objectCauseCollision.GetComponent<Renderer>().material.color = Color.yellow;
    }
}
