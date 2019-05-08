using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTestBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var vertices = GetColliderVertexPositions(this.gameObject);
        foreach( Vector3 point in vertices)
        {
            Debug.Log(point.x + ", " + point.y + ", " + point.z);
        }

    }

    // Update is called once per frame
    void Update()
    {
        var objects = GameObject.FindGameObjectsWithTag("Obbs");
        var collider = this.GetComponent<BoxCollider>();
        Obb obb = ObbCollisionDetection.ToObb(collider);

        //gameObject.GetComponent<Renderer>().material.color = Color.blue;

        foreach (GameObject obj in objects)
        {
            if (obj != this)
            {
                if (ObbCollisionDetection.Intersects(obb, ObbCollisionDetection.ToObb(obj.GetComponent<BoxCollider>())))
                {
                    gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                else
                {   // rend.material.shader = Shader.Find("_Color");
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
            }
           
          
        }

    }

    public Vector3[] GetColliderVertexPositions(GameObject obj)
    {
        BoxCollider b = obj.GetComponent<BoxCollider>(); //retrieves the Box Collider of the GameObject called obj
        Vector3[] vertices = new Vector3[8];
        vertices[0] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f);
        vertices[1] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f);
        vertices[2] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f);
        vertices[3] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f);
        vertices[4] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, -b.size.z) * 0.5f);
        vertices[5] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, -b.size.z) * 0.5f);
        vertices[6] = obj.transform.TransformPoint(b.center + new Vector3(b.size.x, b.size.y, b.size.z) * 0.5f);
        vertices[7] = obj.transform.TransformPoint(b.center + new Vector3(-b.size.x, b.size.y, b.size.z) * 0.5f);
       

        return vertices;
    }
}
