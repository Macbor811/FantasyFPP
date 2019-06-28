using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : CollisionHandler
{
    // Start is called before the first frame update
    public int damage = 10;

    public bool IsActive
    { set; get; } = false;
    //private bool damageDealt = false;

    protected override void setTagOfObjectCollision()
    {
        tagOfObjectCollision = "Enemy";
    }

    public override void onStartCollsion(GameObject objectCauseCollision)
    {

            var dmg = objectCauseCollision.GetComponent<IPlayerHittable>();
            if (dmg != null)
            {
                dmg.OnHit(this);
                IsActive = false;
            }

    }

    void Start()
    {
       // IsActive = false;
        base.Start();
    }

    
    void Update()
    {
        if (IsActive)
            base.Update();
    }
}
