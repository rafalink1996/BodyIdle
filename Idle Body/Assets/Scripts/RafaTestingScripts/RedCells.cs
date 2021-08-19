using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCells : MonoBehaviour
{
    public Vector2 target;
    public Vector2 currentPos;
    public Vector2 combineTarget;
    public float speed;
    public bool combining;
    Coroutine moveCoroutine;
    Coroutine moveAgain;
    [HideInInspector]
    public CellMerger myCellMerger;
    // Start is called before the first frame update
    void Start()
    {
        myCellMerger = FindObjectOfType<CellMerger>();
        Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
        target = Camera.main.ViewportToWorldPoint(randomPosition);
        //target = new Vector2(Random.Range(-8.5f, 8.5f), Random.Range(-5f, 5f));
        moveCoroutine = StartCoroutine(Move());

    }
    IEnumerator Move()
    {
        while (true)
        {
            
                if (currentPos != target)
                {
                    transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    yield return null;
                }
                else
                {

                    moveAgain = StartCoroutine(MoveAgain());
                    yield return null;
                    break;

                }
            
            
        }

        /*if (currentPos != target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            
        }
        else
        {
            if (move)
            {
                StartCoroutine(MoveAgain());
            }
        }*/
    }
    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        /*Vector2 currentPos = transform.position;
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
        }*/
    }
    IEnumerator MoveAgain()
    {
        if (!combining)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
            target = Camera.main.ViewportToWorldPoint(randomPosition);
            moveCoroutine = StartCoroutine(Move());
        }
        

    }
    public void StartMerge()
    {
        StartCoroutine(Combine());
    }
    IEnumerator Combine()
    {
        if (!combining)
        {
            combining = true;
            if (moveAgain != null)
            {
                StopCoroutine(moveAgain);
            }
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            yield return new WaitForSeconds(0.4f);
            GetComponent<Animator>().SetTrigger("Light");

            yield return new WaitForSeconds(0.3f);

            target = combineTarget;
            speed += 5;
            moveCoroutine = StartCoroutine(Move());
  
        }
    }
}
