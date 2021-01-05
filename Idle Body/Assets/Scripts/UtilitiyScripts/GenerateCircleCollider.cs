using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generates a circle from a edge collider with a specified radius
public class GenerateCircleCollider : MonoBehaviour
{
   public int NumEdges;
   public float Radius;

    public Vector2[] polygonPoints;

    // Use this for initialization
    void Start()
    {
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        Vector2[] points = new Vector2[NumEdges + 1];

        for (int i = 0; i < NumEdges; i++)
        {
            float angle = 2 * Mathf.PI * i / NumEdges;
            float x = Radius * Mathf.Cos(angle);
            float y = Radius * Mathf.Sin(angle);

            points[i] = new Vector2(x, y);
        }
        points[NumEdges] = points[0];
        polygonPoints = points;
        edgeCollider.points = points;
    }
 
}
