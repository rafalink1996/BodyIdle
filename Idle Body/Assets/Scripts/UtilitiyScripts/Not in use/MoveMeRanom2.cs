using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMeRanom2 : MonoBehaviour
{
    Rigidbody2D rb2D;
    public float speed;

    public float collisionAngleRange;
    public float initialVelocity;


    private Vector2 RandomVector(float min, float max)
    {
        var x = Random.Range(min, max);
        var y = Random.Range(min, max);

        
        
        return new Vector2(x, y);
    }



    // Start is called before the first frame update
    void Start()
    {
        
        rb2D = GetComponent<Rigidbody2D>();

        rb2D.velocity = VectorUtilities.AngleToVector2(135) * initialVelocity;
        Physics2D.IgnoreLayerCollision(0, 9);
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        Bounds bounds = collision.gameObject.GetComponent<EdgeCollider2D>().bounds;
        float left = bounds.min.x;
        float right = bounds.max.x;

        Vector2 contact = collision.contacts[0].point;

        // Interpolate the contact point and determine the angle.
        float interpolation = (contact.x - left) / (right - left);
        float angle = 90 - collisionAngleRange / 2 + collisionAngleRange * (1 - interpolation);

        // Change the direction of the ball without affecting its speed.
        rb2D.velocity = VectorUtilities.AngleToVector2(angle) * rb2D.velocity.magnitude;
        */

    }

}
