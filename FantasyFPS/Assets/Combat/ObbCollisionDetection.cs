
using UnityEngine;

public class Obb
{
    public readonly Vector3[] Vertices;
    public readonly Vector3 Right;
    public readonly Vector3 Up;
    public readonly Vector3 Forward;

    public Obb(BoxCollider collider)
    {
        var rotation = collider.transform.rotation;

        Vertices = new Vector3[8];
        Vertices[0] = collider.transform.TransformPoint(collider.center + new Vector3(-collider.size.x, -collider.size.y, -collider.size.z) * 0.5f);
        Vertices[1] = collider.transform.TransformPoint(collider.center + new Vector3(collider.size.x, -collider.size.y, -collider.size.z) * 0.5f);
        Vertices[2] = collider.transform.TransformPoint(collider.center + new Vector3(collider.size.x, -collider.size.y, collider.size.z) * 0.5f);
        Vertices[3] = collider.transform.TransformPoint(collider.center + new Vector3(-collider.size.x, -collider.size.y, collider.size.z) * 0.5f);
        Vertices[4] = collider.transform.TransformPoint(collider.center + new Vector3(-collider.size.x, collider.size.y, -collider.size.z) * 0.5f);
        Vertices[5] = collider.transform.TransformPoint(collider.center + new Vector3(collider.size.x, collider.size.y, -collider.size.z) * 0.5f);
        Vertices[6] = collider.transform.TransformPoint(collider.center + new Vector3(collider.size.x, collider.size.y, collider.size.z) * 0.5f);
        Vertices[7] = collider.transform.TransformPoint(collider.center + new Vector3(-collider.size.x, collider.size.y, collider.size.z) * 0.5f);

        Right = rotation * Vector3.right;
        Up = rotation * Vector3.up;
        Forward = rotation * Vector3.forward;
    }
}

public class ObbCollisionDetection : MonoBehaviour
{
   

    public static Obb ToObb(BoxCollider collider)
    {
        return new Obb(collider);
    }

    

    public static bool Intersects(BoxCollider lhs, BoxCollider rhs)
    {
        var a = new Obb(lhs);
        var b = new Obb(rhs);

        if (Separated(a.Vertices, b.Vertices, a.Right))
            return false;
        if (Separated(a.Vertices, b.Vertices, a.Up))
            return false;
        if (Separated(a.Vertices, b.Vertices, a.Forward))
            return false;

        if (Separated(a.Vertices, b.Vertices, b.Right))
            return false;
        if (Separated(a.Vertices, b.Vertices, b.Up))
            return false;
        if (Separated(a.Vertices, b.Vertices, b.Forward))
            return false;

        if (Separated(a.Vertices, b.Vertices, Vector3.Cross(a.Right, b.Right)))
            return false;
        if (Separated(a.Vertices, b.Vertices, Vector3.Cross(a.Right, b.Up)))
            return false;
        if (Separated(a.Vertices, b.Vertices, Vector3.Cross(a.Right, b.Forward)))
            return false;

        if (Separated(a.Vertices, b.Vertices, Vector3.Cross(a.Up, b.Right)))
            return false;
        if (Separated(a.Vertices, b.Vertices, Vector3.Cross(a.Up, b.Up)))
            return false;
        if (Separated(a.Vertices, b.Vertices, Vector3.Cross(a.Up, b.Forward)))
            return false;

        if (Separated(a.Vertices, b.Vertices, Vector3.Cross(a.Forward, b.Right)))
            return false;
        if (Separated(a.Vertices, b.Vertices, Vector3.Cross(a.Forward, b.Up)))
            return false;
        if (Separated(a.Vertices, b.Vertices, Vector3.Cross(a.Forward, b.Forward)))
            return false;

        return true;
    }

    static bool Separated(Vector3[] vertsA, Vector3[] vertsB, Vector3 axis)
    {
        // przypadek iloczynu wektorowego = {0,0,0}
        if (axis == Vector3.zero)
            return false;

        var aMin = float.MaxValue;
        var aMax = float.MinValue;
        var bMin = float.MaxValue;
        var bMax = float.MinValue;

        for (var i = 0; i < 8; i++)
        {
            var aDist = Vector3.Dot(vertsA[i], axis);
            aMin = aDist < aMin ? aDist : aMin;
            aMax = aDist > aMax ? aDist : aMax;
            var bDist = Vector3.Dot(vertsB[i], axis);
            bMin = bDist < bMin ? bDist : bMin;
            bMax = bDist > bMax ? bDist : bMax;
        }

        // test przecięcia
        var longSpan = Mathf.Max(aMax, bMax) - Mathf.Min(aMin, bMin);
        var sumSpan = aMax - aMin + bMax - bMin;
        return longSpan >= sumSpan; // > aby traktowac dotykanie jak kolizje
    }
}

abstract public class CollisionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider thisCollider;
    GameObject latestCollisionObject = null;
    protected string tagOfObjectCollision;

    protected void Start()
    {
        setTagOfObjectCollision();
        thisCollider = this.GetComponent<BoxCollider>();
        if (!thisCollider)
        {
            throw new MissingComponentException("Custom collision handler doesn't contain box collider component");
        }
    }

    // update is called once per frame
    bool exitCollsion = false;
    bool hasAlreadyStarted = false;
    protected void Update()
    {
        GameObject obj = getCollsionObject(this.gameObject, tagOfObjectCollision);
        if (!hasAlreadyStarted && obj)
        {
            onStartCollsion(obj);
            hasAlreadyStarted = true;
        }
        if (obj)
        {
            handleCollision(obj);
            latestCollisionObject = obj;
            exitCollsion = true;
        }
        else
        {
            if (exitCollsion)
            {
                exitCollsion = false;
                onExitCollision(latestCollisionObject);
                hasAlreadyStarted = false;
            }
        }
    }

    GameObject getCollsionObject(GameObject thisObj, string tag)
    {
        //Obb obb = new Obb(thisObj.GetComponent<BoxCollider>());
        var objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            if (obj != thisObj && ObbCollisionDetection.Intersects(thisCollider, obj.GetComponent<BoxCollider>()))
            {
                return obj;
            }
        }
        return null;
    }

    //TODO
    //set a proper tag
    //has to be implemented!
    protected abstract void setTagOfObjectCollision();
    //{
    //    tagOfObjectCollision = "Obbs";
    //}


    //TODO
    //override if you want to
    public virtual void onStartCollsion(GameObject objectCauseCollision)
    {
       // Debug.Log("Poczatek kolizji z obiektem:" + objectCauseCollision.ToString());
    }


    //TODO
    //override if you want to
    public virtual void handleCollision(GameObject objectCauseCollision)
    {

    }

    //TODO
    //override if you want to
    public virtual void onExitCollision(GameObject objectCauseCollision)
    {
       // Debug.Log("Wyjscie z kolizji z obiektem:" + objectCauseCollision.ToString());
    }


}