using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedBloodCell : MonoBehaviour
{

    GameManager gameManager;
    // cell follows cell target
    Transform Target;
    public float[] speed;
    float MoveSpeed;
    const float Epsilon = 0.4f;
    Vector2 mlookDirection;

    //cell adds points
    public TextMeshProUGUI plus1text;
    float alfaValue = 0;



    void Start()
    {
        Target = GameObject.Find("redBloodCellTarget").GetComponent<Transform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        MoveSpeed = Random.Range(speed[0], speed[1]);
        plus1text.color = new Color32(1, 1, 1, 0);
    }


    void Update()
    {
        mlookDirection = (Target.position - transform.position).normalized;

        if ((transform.position - Target.position).magnitude > Epsilon)
        {
            transform.Translate(mlookDirection * MoveSpeed * Time.deltaTime);

        }
    }

    public void showPointsGained()
    {
        StartCoroutine(TextDisplayOn());
    }


     IEnumerator TextDisplayOn()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(0.01f);
            if (plus1text.color.a < 1)
            {
                
                plus1text.color = new Color(1, 1, 1, alfaValue);
                alfaValue += 0.1f;
            }
            else
            {
                yield return new WaitForSeconds(0.3f);
                StartCoroutine(TextDisplayOff());
                break;
            }
        }
        

    }

    IEnumerator TextDisplayOff()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (plus1text.color.a > 0)
            {
               
                plus1text.color = new Color(1, 1, 1, alfaValue);
                alfaValue -= 0.1f;
            }
            else
            {
                break;
            }
        }
        
    }

    

}

