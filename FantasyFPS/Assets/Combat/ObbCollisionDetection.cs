
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
        // Handles the cross product = {0,0,0} case
        if (axis == Vector3.zero)
            return false;

        var aMin = float.MaxValue;
        var aMax = float.MinValue;
        var bMin = float.MaxValue;
        var bMax = float.MinValue;

        // Define two intervals, a and b. Calculate their min and max values
        for (var i = 0; i < 8; i++)
        {
            var aDist = Vector3.Dot(vertsA[i], axis);
            aMin = aDist < aMin ? aDist : aMin;
            aMax = aDist > aMax ? aDist : aMax;
            var bDist = Vector3.Dot(vertsB[i], axis);
            bMin = bDist < bMin ? bDist : bMin;
            bMax = bDist > bMax ? bDist : bMax;
        }

        // One-dimensional intersection test between a and b
        var longSpan = Mathf.Max(aMax, bMax) - Mathf.Min(aMin, bMin);
        var sumSpan = aMax - aMin + bMax - bMin;
        return longSpan >= sumSpan; // > to treat touching as intersection
    }
}
