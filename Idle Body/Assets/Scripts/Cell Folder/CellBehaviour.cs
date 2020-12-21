using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    //RigidBody and Speeds
    Rigidbody2D rb2D;
    public float speed;
   

    // Determine witch cell is it working with
    public Cell CellScriptableObject;

    //IdleCell
    public float initialVelocity;
    // RedBloodCell
    Transform Target;
    public float MoveSpeed;
    const float Epsilon = 0.4f;
    Vector2 mlookDirection;




    // Start is called before the first frame update
    void Start()
    {

        Target = GameObject.Find("redBloodCellTarget").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // set Sprite
        SpriteRenderer SR = gameObject.GetComponent<SpriteRenderer>();
        SR.sprite = CellScriptableObject.CellSprite;


        // set Behaviour
        if (CellScriptableObject.CellID == 0)
        {

        }
        else if ( CellScriptableObject.CellID == 1)
        {
            mlookDirection = (Target.position - transform.position).normalized;

            if ((transform.position - Target.position).magnitude > Epsilon)
            {
                transform.Translate(mlookDirection * MoveSpeed * Time.deltaTime);
            }
        }
        
        
        
    }

    



}
