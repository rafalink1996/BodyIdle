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
    [SerializeField] Sprite CombineSprite = null;
    [SerializeField] private OrganManager.OrganType.OrganInfo.cellsType.CellSizes.CellInfo myInfo;

    [Header("Movement")]
    [SerializeField] Vector2 target = Vector2.zero;
    [SerializeField] Vector2 currentPos = Vector2.zero;

    public Vector2 combineTarget;
    [HideInInspector] public CellMerger myCellMerger;
    [HideInInspector] public CellSpawner myCellSpawner;
    SpriteRenderer mySpriteRender;

    protected Coroutine moveCoroutine;
    protected Coroutine moveAgain;
    protected Coroutine UpdateHealthCoroutine;

    protected bool combining;
    [SerializeField] protected float speed;

    [Header("Components")]
    [SerializeField] ParticleSystem myParticleSystem;
    [SerializeField] Rigidbody2D myRigidbody2D;
    [SerializeField] Collider2D myCollider2D;
    float randomTime;



    [Header("Testing")]
    public bool TakeDamage;



    void Start()
    {
        myCellMerger = FindObjectOfType<CellMerger>();
        myCellSpawner = FindObjectOfType<CellSpawner>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        myCollider2D = GetComponent<Collider2D>();

    }
    public void CellStartAnim()
    {
        Vector3 size = transform.localScale;

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        transform.localScale = new Vector3(0, 0, 1);
        LTDescr f = LeanTween.scale(gameObject, size, 0.5f).setEase(LeanTweenType.easeInExpo);
        myParticleSystem = GetComponent<ParticleSystem>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        if (myParticleSystem != null)
        {
            myParticleSystem.Play();
        }
        f.setOnComplete(StartToMove);

    }
    public void CellStart(int cellID, CellSize size, OrganManager.OrganType.OrganInfo.cellsType.CellSizes.CellInfo info)
    {
        myInfo = info;
        cellSize = size;
        ID = cellID;

        Vector2 randomPosition = Vector2.zero;
        switch (CellType)
        {
            case 0: // Red Cells
                randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.7f, 0.87f));
                break;
            case 1:
            case 2:
                randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.1f, 0.68f));
                break;
            default:
                randomPosition = Vector2.zero;
                break;

        }
        target = Camera.main.ViewportToWorldPoint(randomPosition);


        UpdateHealthCoroutine = StartCoroutine(UpdateHealth());
        randomTime = Random.Range(0.4f, 1.5f);


    }
    public void StartToMove()
    {
        moveCoroutine = StartCoroutine(Move());
    }

    private void Update()
    {
        currentPos = transform.position;
        if (TakeDamage == true)
        {
            myInfo.health -= 5;
            TakeDamage = false;
        }
    }

    IEnumerator UpdateHealth()
    {
        while (myInfo.alive)
        {
            if (myInfo.health <= 0)
            {
                myInfo.alive = false;
                myInfo.timer = 2 * (int)cellSize + 1;
                StartCoroutine(DeathTimer());
            }
            yield return new WaitForSeconds(0.3f);
        }
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
        StartCoroutine(UpdateHealth());
    }

    IEnumerator MoveAgain()
    {
        if (!combining)
        {
            Vector2 randomPosition = Vector2.zero;
            switch (CellType)
            {
                case 0: // Red Cells
                    randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.7f, 0.87f));
                    break;
                case 1:
                case 2:
                    randomPosition = new Vector2(Random.Range(0.2f, 0.8f), Random.Range(0.1f, 0.68f));
                    break;
                default:
                    randomPosition =  Vector2.zero;
                    break;

            }
            yield return new WaitForSeconds(randomTime);   
            target = Camera.main.ViewportToWorldPoint(randomPosition);
            moveCoroutine = StartCoroutine(Move(true));
            randomTime = Random.Range(2f, 4f);
        }
    }
    IEnumerator Move(bool hasTarget = false)
    {
        if (hasTarget)
        {
            Vector3 RandomRotation = new Vector3(0, 0, Random.Range(-10, 10));
            while (currentPos != target)
            {
                float factor = Vector2.Distance(transform.position, target);
                Mathf.Clamp(factor, 0.1f, speed);
                transform.Rotate(RandomRotation * speed * 0.5f * factor);
                transform.position = Vector2.MoveTowards(transform.position, target, factor/* factor*/ * Time.deltaTime);
                yield return null;
            }
            GetNewTarget();
            yield return null;
        }
        else
        {
            float movingTime = Random.Range(2, 3);
            Vector2 force = new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed)).normalized;
            Vector2 cameraPos = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
            switch (CellType)
            {
                case 0:
                    if(currentPos.y < cameraPos.y)
                    {
                        force = new Vector2(Random.Range(-speed, speed), Random.Range(0, speed)).normalized;
                    }
                    else
                    {
                        force = new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed)).normalized;
                    }                
                    break;
                case 1:
                case 2:
                    if (currentPos.y < cameraPos.y)
                    {
                        force = new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed)).normalized;
                    }
                    else
                    {
                        force = new Vector2(Random.Range(-speed, speed), Random.Range(-speed, 0)).normalized;
                    }
                    break;
                default:
                    force = new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed)).normalized;
                    break;

            }
            

            while (movingTime > 0)
            {
                yield return new WaitForEndOfFrame();
                movingTime -= Time.deltaTime;
                float factor = Mathf.Sin((movingTime / 2) * Mathf.PI);
                myRigidbody2D.velocity = factor * force;
                yield return null;
            }
          
            myRigidbody2D.velocity = Vector2.zero;
            yield return new WaitForSeconds(randomTime);
            moveCoroutine = StartCoroutine(Move());
        }
    }

    IEnumerator MoveToCombine()
    {
        myRigidbody2D.velocity = Vector2.zero;
        while (!(Vector2.Distance(currentPos, combineTarget) < 0.3f))
        {
            transform.position = Vector2.MoveTowards(transform.position, combineTarget, speed * Time.deltaTime);
            yield return null;
        }
        mySpriteRender.color = new Color(0, 0, 0, 0);
        Debug.Log("Arrived to destination");
        myCellMerger.CellArrived();
        gameObject.SetActive(false);
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

            LeanTween.scale(gameObject, transform.localScale - transform.localScale / 2, 0.5f);
            yield return new WaitForSeconds(0.1f);

            if (CombineSprite != null && mySpriteRender != null)
            {
                mySpriteRender.sprite = CombineSprite;
                Debug.Log("Combine sprite changed");
            }
            else
            {
                Debug.Log("Combine is null");
            }

            yield return new WaitForSeconds(0.3f); //0.3f

            target = combineTarget;
            speed += 3;
            moveCoroutine = StartCoroutine(MoveToCombine());
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.TryGetComponent(out Pathogen_Base pathogen_Base);
        if (pathogen_Base != null)
        {
            PathogenHit(pathogen_Base);
        }
    }

    protected virtual void PathogenHit(Pathogen_Base pathogen_Base)
    {
    }

    public void ToggleCollider()
    {
        if (myCollider2D.isTrigger)
        {
            myCollider2D.isTrigger = false;
        }
        else
        {
            myCollider2D.isTrigger = true;
        }
    }
}


