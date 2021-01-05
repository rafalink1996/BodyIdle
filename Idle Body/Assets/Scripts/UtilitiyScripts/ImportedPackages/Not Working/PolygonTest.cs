using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonTest : MonoBehaviour
{
  
    public Vector2[] vertices2D;
    
  
    // Start is called before the first frame update
    void Start()
    {
        GameObject body = GameObject.Find("Body");
        PolygonUtilities polyUtils = body.GetComponent<PolygonUtilities>();
        Vector2[] bodyPlyPoints = polyUtils.polygonPoints;
        for (int i = 0; i < bodyPlyPoints.Length; i++)
        {
            Vector2 Point = bodyPlyPoints[i];
            Point.x *= 10;
            Point.y *= 10;

            Point.x = Mathf.FloorToInt(Point.x);
            Point.y = Mathf.FloorToInt(Point.y);

            bodyPlyPoints[i] = Point;
        }
        //System.Array.Reverse(bodyPlyPoints);
        vertices2D = bodyPlyPoints;
         

        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        // Set up game object with mesh;
        gameObject.AddComponent(typeof(MeshRenderer));
        MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        filter.mesh = msh;
    }

  
}
