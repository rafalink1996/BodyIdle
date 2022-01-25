using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCell_M : MonoBehaviour
{
    [SerializeField]
    List<GameObject> smallCells;
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
        smallCells = new List<GameObject>();
        target = new Vector2(Random.Range(-8.5f, 8.5f), Random.Range(-5f, 5f));
        if (!spawn)
        {
            gameObject.tag = "Untagged";
            GetComponent<Animator>().SetTrigger("Light");
            GetComponent<HitPoints>().canDie = false;
        }
        gameObject.name = "RedBlood_M";
    }

    // Update is called once per frame
    void Update()
    {
        if (smallCells.Count == 10 && !spawn)
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
        if (other.name == "RedBlood_S" && !spawn)
        {
            smallCells.Add(other.gameObject);
        }

    }
    IEnumerator SpawnSelf()
    {
        spawn = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().SetTrigger("Spawn");
        GetComponent<HitPoints>().canDie = true;
        foreach (GameObject cell_S in smallCells)
        {
            Destroy(cell_S);
        }
        move = true;
        FindObjectOfType<OrganCellSpawner>().canBuyRedCell = true;
        FindObjectOfType<OrganCellSpawner>().combine_M = false;
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
