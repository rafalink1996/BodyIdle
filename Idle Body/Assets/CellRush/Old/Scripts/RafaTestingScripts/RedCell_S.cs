using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCell_S : MonoBehaviour
{
    public Vector2 target;
    public Vector2 currentPos;
    public Vector2 combineTarget;
    public Vector2 velocity = Vector2.zero;
    public bool move = true;
    public float speed;
    public float maxSpeed;
    public bool combine;
    public bool combining;
    // Start is called before the first frame update
    void Start()
    {
        target = new Vector2(Random.Range(-8.5f, 8.5f), Random.Range(-5f, 5f));
        gameObject.name = "RedBlood_S";
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        if (move)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        //transform.position = Vector2.SmoothDamp(transform.position, target, ref velocity, speed * Time.deltaTime, maxSpeed * Time.deltaTime);
        if (currentPos == target)
        {
            StartCoroutine(MoveAgain());
        }
        if (combine)
        {
            StartCoroutine(Combine());
        }
    }
    IEnumerator MoveAgain()
    {
        if (!combining)
        {
            move = false;
            target = new Vector2(Random.Range(-8.5f, 8.5f), Random.Range(-5f, 5f));
            yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));
            move = true;
        }
    }
    IEnumerator Combine()
    {
        if (!combining)
        {
            combining = true;
            yield return new WaitForSeconds(0.4f);
            GetComponent<Animator>().SetTrigger("Light");
            yield return new WaitForSeconds(0.3f);
            target = combineTarget;
            speed += 5;
        }
    }
}
