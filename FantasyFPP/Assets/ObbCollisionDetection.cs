
using UnityEngine;

public class Obb
{
    public readonly Vector3[] vertices;
    public readonly Vector3 right;
    public readonly Vector3 up;
    public readonly Vector3 forward;

    public Obb(BoxCollider collider)
    {
        var rotation = collider.transform.rotation;
        vertices = new Vector3[8];
        vertices[0] = collider.transform.TransformPoint(collider.center + new Vector3(-collider.size.x, -collider.size.y, -collider.size.z) * 0.5f);
        vertices[1] = collider.transform.TransformPoint(collider.center + new Vector3(collider.size.x, -collider.size.y, -collider.size.z) * 0.5f);
        vertices[2] = collider.transform.TransformPoint(collider.center + new Vector3(collider.size.x, -collider.size.y, collider.size.z) * 0.5f);

        vertices[3] = collider.transform.TransformPoint(collider.center + new Vector3(-collider.size.x, -collider.size.y, collider.size.z) * 0.5f);
        vertices[4] = collider.transform.TransformPoint(collider.center + new Vector3(-collider.size.x, collider.size.y, -collider.size.z) * 0.5f);
        vertices[5] = collider.transform.TransformPoint(collider.center + new Vector3(collider.size.x, collider.size.y, -collider.size.z) * 0.5f);
        vertices[6] = collider.transform.TransformPoint(collider.center + new Vector3(collider.size.x, collider.size.y, collider.size.z) * 0.5f);
        vertices[7] = collider.transform.TransformPoint(collider.center + new Vector3(-collider.size.x, collider.size.y, collider.size.z) * 0.5f);

        forward = rotation * Vector3.forward;
        right = rotation * Vector3.right;
        up = rotation * Vector3.up;
    }
}

public class ObbCollisionDetection : MonoBehaviour
{


    public static Obb ToObb(BoxCollider collider)
    {
        return new Obb(collider);
    }



    public static bool Intersects(Obb a, Obb b)
    {
        if (Separated(a.vertices, b.vertices, a.forward))
            return false;
        if (Separated(a.vertices, b.vertices, a.up))
            return false;
        if (Separated(a.vertices, b.vertices, a.right))
            return false;

        if (Separated(a.vertices, b.vertices, b.forward))
            return false;
        if (Separated(a.vertices, b.vertices, b.up))
            return false;
        if (Separated(a.vertices, b.vertices, b.right))
            return false;

        if (Separated(a.vertices, b.vertices, Vector3.Cross(a.forward, b.forward)))
            return false;
        if (Separated(a.vertices, b.vertices, Vector3.Cross(a.forward, b.up)))
            return false;
        if (Separated(a.vertices, b.vertices, Vector3.Cross(a.forward, b.right)))
            return false;

        if (Separated(a.vertices, b.vertices, Vector3.Cross(a.right, b.forward)))
            return false;
        if (Separated(a.vertices, b.vertices, Vector3.Cross(a.right, b.up)))
            return false;
        if (Separated(a.vertices, b.vertices, Vector3.Cross(a.right, b.right)))
            return false;

        if (Separated(a.vertices, b.vertices, Vector3.Cross(a.up, b.forward)))
            return false;
        if (Separated(a.vertices, b.vertices, Vector3.Cross(a.up, b.up)))
            return false;
        if (Separated(a.vertices, b.vertices, Vector3.Cross(a.up, b.right)))
            return false;
        return true;
    }

    static bool Separated(Vector3[] vertsA, Vector3[] vertsB, Vector3 axis)
    {
        //if is a zero vector
        if (axis == Vector3.zero)
            return false;

        var aMin = float.MaxValue;
        var bMin = float.MaxValue;
        var aMax = float.MinValue;
        var bMax = float.MinValue;

        // Define two intervals, a and b. Calculate their min and max values
        for (var i = 0; i < 8; i++)
        {
            var bDistance = Vector3.Dot(vertsB[i], axis);
            var aDistance = Vector3.Dot(vertsA[i], axis);

            if (bDistance < bMin)
                bMin = bDistance;
            else if (bDistance > bMax)
                bMax = bDistance;
            if (aDistance < aMin)
                aMin = aDistance;
            else if (aDistance > aMax)
                aMax = aDistance;
        }

        // One-dimensional intersection test between a and b
        var sumSpan = aMax - aMin + bMax - bMin;
        var longSpan = Mathf.Max(aMax, bMax) - Mathf.Min(aMin, bMin);
        return longSpan >= sumSpan ? true : false; // > to treat touching as intersection
    }
}

abstract public class HandleCollsion : MonoBehaviour
{
    // Start is called before the first frame update
    Obb obb;
    GameObject latestCollisionObject = null;
    protected string tagOfObjectCollision ;

    void Start()
    {
        setTagOfObjectCollision();
        obb = new Obb(this.GetComponent<BoxCollider>());
    }

    // update is called once per frame
    bool exitCollsion = false;
    bool hasAlreadyStarted = false;
    void Update()
    {
        GameObject obj = getCollsionObject(this.gameObject, tagOfObjectCollision);
        if(!hasAlreadyStarted && obj)
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
        Obb obb = new Obb(thisObj.GetComponent<BoxCollider>());
        var objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            if (ObbCollisionDetection.Intersects(obb, new Obb(obj.GetComponent<BoxCollider>())))
            {
                return obj;
            }
        }
        return null;
    }

    //TODO
    //set a proper tag
    //has to be implemented!
    protected virtual void setTagOfObjectCollision()
    {
        tagOfObjectCollision = "Obbs";
    }


    //TODO
    //override if you want to
    public virtual void onStartCollsion(GameObject objectCauseCollision)
    {
        Debug.Log("Poczatek kolizji z obiektem:" + objectCauseCollision.ToString());
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
        Debug.Log("Wyjscie z kolizji z obiektem:" + objectCauseCollision.ToString());
    }


}
