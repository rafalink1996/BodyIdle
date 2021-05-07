using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCell_M : MonoBehaviour
{
    [SerializeField]
    List<GameObject> smallCells;
    public bool spawn = true;
    public Vector2 target;
    public Vector2 currentPos;
    public Vector2 combineTarget;
    public bool combine;
    // Start is called before the first frame update
    void Start()
    {
        smallCells = new List<GameObject>();
        target = new Vector2(Random.Range(-8.5f, 8.5f), Random.Range(-5f, 5f));
        if (!spawn)
        {
            gameObject.tag = "Untagged";
            GetComponent<Animator>().SetTrigger("Light");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (smallCells.Count == 10 && !spawn)
        {
            StartCoroutine(SpawnSelf());
        }
        currentPos = transform.position;
        if (gameObject.tag == "RedCells10")
        {
            transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime);
        }
        if (currentPos == target)
        {
            target = new Vector2(Random.Range(-8.5f, 8.5f), Random.Range(-5f, 5f));
        }
        if (combine)
        {
            StartCoroutine(Combine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RedCells1")
        {
            smallCells.Add(other.gameObject);
        }

    }
    IEnumerator SpawnSelf()
    {
        spawn = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().SetTrigger("Spawn");
        foreach (GameObject cell_S in smallCells)
        {
            Destroy(cell_S);
        }
        gameObject.tag = "RedCells10";
    }
    IEnumerator Combine()
    {
        yield return new WaitForSeconds(0.4f);
        GetComponent<Animator>().SetTrigger("Light");
        yield return new WaitForSeconds(0.3f);
        target = combineTarget;
    }
}
