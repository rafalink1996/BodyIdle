using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PolygonUtilities : MonoBehaviour
{
    
    public bool GenerateEdgeCollider;
    public Vector2[] polygonPoints;
    public List<Vector2> PlygonPointColse;
    

   

    // Start is called before the first frame update
    void Awake()
    {
        if(GenerateEdgeCollider == true)
        {
            PolygonCollider2D polygonCollider2D = GetComponent<PolygonCollider2D>();
            if (polygonCollider2D == null)
            {
                polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
            }
            Vector2[] ColliderPoints = polygonCollider2D.points;
            for (int i = 0; i < ColliderPoints.Length; i++)
            {
                PlygonPointColse.Add(ColliderPoints[i]);

            }
            PlygonPointColse.Add(ColliderPoints[0]);

            EdgeCollider2D edgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
            edgeCollider2D.points = PlygonPointColse.ToArray();
            polygonPoints = edgeCollider2D.points;
            Destroy(polygonCollider2D);
            
        }
        

    }



    












}
