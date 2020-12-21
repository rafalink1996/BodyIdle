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

    // whiteBloodCell
    bool isAttacking;

    


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
        if (CellScriptableObject.CellID == 0) // idle Cell Behaviour
        {

        }
        else if ( CellScriptableObject.CellID == 1) // RedBloodCell Behaviour
        {
            mlookDirection = (Target.position - transform.position).normalized;

            if ((transform.position - Target.position).magnitude > Epsilon)
            {
                transform.Translate(mlookDirection * MoveSpeed * Time.deltaTime);
            }
        }
        else if (CellScriptableObject.CellID == 2) // WhiteBloodCell Behaviour
        {
            GameObject target = FindClosestEnemy();
            if (target != null)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

                if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
                {
                    speed = 0;
                    if (isAttacking == false)
                    {
                        Enemy EnemyCS = target.GetComponent<Enemy>();
                        EnemyCS.TakeDamage(CellScriptableObject.CellID, 1);
                        StartCoroutine(restartAttack());
                        isAttacking = true;
                    }    
                }
                else
                {
                    if (speed < 1)
                    {
                        speed += 0.01f;
                    }
                    
                }
            } 
        }
    } // end Of Update



    // get closest enemy (for WhiteBloodCell)
    GameObject FindClosestEnemy()
    {
        float distanceToClostestEnemy = Mathf.Infinity;
        GameObject ClosestEnemy = null;
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClostestEnemy)
            {
                distanceToClostestEnemy = distanceToEnemy;
                ClosestEnemy = currentEnemy;
            }
        }

        return ClosestEnemy;
    }

    IEnumerator restartAttack()
    {
        yield return new WaitForSeconds(1);
        isAttacking = false;
    }

    



}
