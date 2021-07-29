using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCell_L : MonoBehaviour
{
    [SerializeField]
    List<GameObject> medCells;
    public bool spawn = true;
    public bool move = true;
    public Vector2 target;
    public Vector2 currentPos;
    public Vector2 combineTarget;
    public float speed;
    public bool combine;
    public bool combining;
    // Start is called before the first frame update
    void Start()
    {
        medCells = new List<GameObject>();
        target = new Vector2(Random.Range(-8.5f, 8.5f), Random.Range(-5f, 5f));
        if (!spawn)
        {
            gameObject.tag = "Untagged";
            GetComponent<Animator>().SetTrigger("Light");
            GetComponent<HitPoints>().canDie = false;
        }
        gameObject.name = "RedBlood_L";
    }

    // Update is called once per frame
    void Update()
    {
        if (medCells.Count == 10 && !spawn)
        {
            StartCoroutine(SpawnSelf());
        }
        currentPos = transform.position;
        if (move)
        {
            //transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        if (currentPos == target)
        {
            StartCoroutine(MoveAgain());
        }
        if (combine)
        {
            StartCoroutine(Combine());
        }
      
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "RedBlood_M")
        {
            medCells.Add(other.gameObject);
        }

    }
    IEnumerator SpawnSelf()
    {
        spawn = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().SetTrigger("Spawn");
        GetComponent<HitPoints>().canDie = true;

        foreach (GameObject cell_M in medCells)
        {
            Destroy(cell_M);
        }
        move = true;
        //gameObject.tag = "RedCells10";
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
