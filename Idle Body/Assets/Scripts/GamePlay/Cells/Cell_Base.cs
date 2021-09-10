using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Base : MonoBehaviour
{
    public enum CellSize { Small, Medium, Big };
    [Header("Identification")]
    public CellSize cellSize;
    public int ID;
    public int CellType;
    [SerializeField] private OrganManager.OrganInfo.cellsType.CellSizes.CellInfo myInfo;

    [Header("Movement")]
    [SerializeField] Vector2 target = Vector2.zero;
    [SerializeField] Vector2 currentPos = Vector2.zero;

    public Vector2 combineTarget;
    [HideInInspector] public CellMerger myCellMerger;
    [HideInInspector] public CellSpawner myCellSpawner;

    protected Coroutine moveCoroutine;
    protected Coroutine moveAgain;
    protected Coroutine UpdateHealthCoroutine;

    protected bool combining;
    [SerializeField] protected float speed;
    

    void Start() 
    {
        myCellMerger = FindObjectOfType<CellMerger>();
        myCellSpawner = FindObjectOfType<CellSpawner>();
    }

   public void CellStart(int cellID, CellSize size, OrganManager.OrganInfo.cellsType.CellSizes.CellInfo info)
    {
        myInfo = info;
        cellSize = size;
        ID = cellID;

        Vector2 randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
        target = Camera.main.ViewportToWorldPoint(randomPosition);

        moveCoroutine = StartCoroutine(Move());
        UpdateHealthCoroutine = StartCoroutine(UpdateHealth());
    }

    private void Update()
    {
        currentPos = transform.position;
    }

    IEnumerator UpdateHealth()
    {
        while (myInfo.alive)
        {
            if (myInfo.health <= 0)
            {
                myInfo.alive = false;
                myInfo.timer = 2 * (int)cellSize;
                StartCoroutine(DeathTimer());
            }
            yield return new WaitForSeconds(0.3f);
        }
        currentPos = transform.position;
        myInfo.health += Time.deltaTime;
        
    }

    IEnumerator DeathTimer()
    {
        while (!myInfo.alive)
        {
            myInfo.timer -= Time.deltaTime;
            if (myInfo.timer <= 0)
            {
                myInfo.health = myInfo.maxHealth;
                myInfo.timer = 0;
                myInfo.alive = true;
            }
            yield return null;
        }
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
                GetNewTarget();
                yield return null;
                break;

            }
        }
    }

    public virtual void GetNewTarget()
    {
        moveAgain = StartCoroutine(MoveAgain());
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.TryGetComponent(out Pathogen_Base pathogen_Base);
        if(pathogen_Base != null)
        {
            PathogenHit(pathogen_Base);
        }
    }

    protected virtual void PathogenHit(Pathogen_Base pathogen_Base)
    {
        
    }
}


