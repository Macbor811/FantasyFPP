using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : CollisionHandler
{
    // Start is called before the first frame update
    public int damage = 10;
    public string collisionTag;
    public GameObject blood;
        
    public bool IsActive
    { set; get; } = false;
    //private bool damageDealt = false;

    protected override void setTagOfObjectCollision()
    {
        tagOfObjectCollision = collisionTag;
    }

    //public override void onStartCollsion(GameObject objectCauseCollision)
    //{
    //        var dmg = objectCauseCollision.GetComponent<IPlayerHittable>();
    //        if (dmg != null)
    //        {
    //            dmg.OnHit(this);
    //            IsActive = false;
    //        }
    //}

    public override void handleCollision(GameObject objectCauseCollision)
    {
        var dmg = objectCauseCollision.GetComponent<IHittable>();
        if (dmg != null)
        {
            dmg.OnHit(this);
            IsActive = false;
            Vector3 direction = transform.position - objectCauseCollision.transform.position;
            direction.Normalize();

            Instantiate(blood, objectCauseCollision.transform.position, Quaternion.LookRotation(direction));
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
