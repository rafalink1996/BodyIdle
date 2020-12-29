using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellBehaviour : MonoBehaviour
{
    //RigidBody and Speeds
    Rigidbody2D rb2D;
    public float speed;
    public GameObject body;


    // Determine witch cell is it working with
    public Cell CellScriptableObject;

    //IdleCell
    public float initialVelocity;
    Vector3 RandomPosition;
    float radius = 1.6f;
    float IdleSpeed = 0.005f;

    // RedBloodCell
    Transform Target;
    public float MoveSpeed;
    const float Epsilon = 0.4f;
    Vector2 mlookDirection;
    public TextMeshProUGUI PlusOneText;
    float PlusOneTextColorAlpha;

    // whiteBloodCell
    bool isAttacking;
    private Transform BodyCenter;



    


    
    void Start()
    {
        
        Target = GameObject.Find("redBloodCellTarget").GetComponent<Transform>();
        BodyCenter = GameObject.Find("Body").GetComponent<Transform>();
    }





    // ..........................................//
    //-------------- START OF UPDATE --------------//
    // ..........................................//

    void Update()
    {
        // set Sprite
        SpriteRenderer SR = gameObject.GetComponent<SpriteRenderer>();
        SR.sprite = CellScriptableObject.CellSprite;


        // ------- set Behaviour ------- //
        //...............................//
        // ------------- Idle Cell ------------ //

        if (CellScriptableObject.CellID == 0) 
        {
            if (transform.position != RandomPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, RandomPosition, IdleSpeed);
            }
            else
            {
                randomizePos();
            }
        }

        // ---------- Red Blood Cell --------- //

        else if ( CellScriptableObject.CellID == 1) 
        {
            mlookDirection = (Target.position - transform.position).normalized;

            if ((transform.position - Target.position).magnitude > Epsilon)
            {
                transform.Translate(mlookDirection * MoveSpeed * Time.deltaTime);
            }
        }

        // ---------- White Blood Cell --------- //

        else if (CellScriptableObject.CellID == 2) 
        {
            GameObject target = FindClosestEnemy();
            //check if there are eneimes
            if (target != null)
            {
                //check if is inside body
                float DistanceBody = Vector3.Distance(target.transform.position, body.transform.position);
                Debug.Log(DistanceBody);
                if (DistanceBody < 1.8f)
                {
                    float step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

                    //check if is in attack range
                    if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
                    {
                        speed = 0;
                        if (isAttacking == false)
                        {
                            Enemy EnemyCS = target.GetComponent<Enemy>();
                            EnemyCS.TakeDamage(CellScriptableObject.CellID, CellScriptableObject.Damage);
                            NumberPopUp.Create(transform.position, CellScriptableObject.Damage, 2, this.gameObject);
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
                }else
                {
                    if (transform.position != RandomPosition){
                        transform.position = Vector3.MoveTowards(transform.position, RandomPosition, IdleSpeed);
                    }else{
                        randomizePos();
                    }
                }
            }
            else
            {
                if (transform.position != RandomPosition){
                    transform.position = Vector3.MoveTowards(transform.position, RandomPosition, IdleSpeed);
                }else{
                    randomizePos();
                }
            }
        }


        //---------------- palette cell ------------- //

        
    }
    // ..........................................//
    //-------------- END OF UPDATE --------------//
    // ..........................................//

    //------------------------ Idle cell functions ---------------------//

    void randomizePos()
    {
        Vector3 offset = Random.insideUnitCircle * radius;
        RandomPosition = body.transform.position + offset;

    }


    //---------------------- Red blood cell functions -----------------//

    public void showPointsGained()
    {
        //StartCoroutine(RedBloodCellPlusOneOn());
        NumberPopUp.Create(transform.position + new Vector3(0,.2f,0) , 1, 1, this.gameObject);
    }



    //---------------------- white blood cell functions -----------------//

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

    // restart attack (white Blood Cell)
    IEnumerator restartAttack()
    {
        yield return new WaitForSeconds(1);
        isAttacking = false;
    }

    



}
