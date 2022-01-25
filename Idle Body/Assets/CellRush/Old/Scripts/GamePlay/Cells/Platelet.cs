using System.Collections;
using UnityEngine;

public class Platelet : MonoBehaviour
{

    Rigidbody2D RB;
    Collider2D myCollider;
    SpriteRenderer mySpriteRender;
    ParticleSystem myParticleSystem;

    [SerializeField] float speed = 2;
    [SerializeField] bool Move;
    [SerializeField] bool Stop;



    [SerializeField] Sprite[] mySprites;
    Coroutine WaitCoroutine;

    public bool MergeReady;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myParticleSystem = GetComponent<ParticleSystem>();
    }
    public void CustomStart(int size, bool sound, bool anim)
    {
        if (RB == null)
        {
            RB = GetComponent<Rigidbody2D>();
        }
        if (myCollider == null)
        {
            myCollider = GetComponent<Collider2D>();
        }
        if (mySpriteRender == null)
        {
            mySpriteRender = GetComponent<SpriteRenderer>();
        }
        if (mySprites.Length != 0)
        {
            mySpriteRender.sprite = mySprites[0];
        }
        if (myParticleSystem == null)
        {
            myParticleSystem = GetComponent<ParticleSystem>();
        }
        mySpriteRender.enabled = true;
        if (sound)
        {
            AudioManager.Instance.Play("UI_pop");
        }
        if (anim)
        {
            SpawnEfefct(size);

        }
        toggleCollider(false);
        WaitCoroutine = StartCoroutine(MoveWaitTime(0.5f));
    }

    void SpawnEfefct(int mySize)
    {
        Vector3 sizeScale = new Vector3(0.025f, 0.025f, 0.025f);
        switch (mySize)
        {
            case 0:
                sizeScale = new Vector3(0.025f, 0.025f, 0.025f);
                break;
            case 1:
                sizeScale = new Vector3(0.05f, 0.05f, 0.05f);
                break;
            case 2:
                sizeScale = new Vector3(0.06f, 0.06f, 0.06f);
                break;

        }
        transform.localScale = Vector3.zero;
        LTDescr l = LeanTween.scale(gameObject, sizeScale, 0.5f).setEase(LeanTweenType.easeInOutExpo);

        myParticleSystem.Play();

    }

    private void Update()
    {
        if (Move)
        {
            PlateletMove();
            Move = false;
        }
        if (Stop)
        {
            StopCoroutine(WaitCoroutine);
            Stop = false;
        }
    }
    private void PlateletMove()
    {
        Vector3 force = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        if (RB == null)
        {

        }
        RB.AddForce(force.normalized * speed * RB.mass);
        if (WaitCoroutine != null)
        {
            StopCoroutine(WaitCoroutine);
            WaitCoroutine = null;
        }
        WaitCoroutine = StartCoroutine(MoveWaitTime(Random.Range(8f, 10f)));
    }
    IEnumerator MoveWaitTime(float Time)
    {
        yield return new WaitForSeconds(Time);
        PlateletMove();
    }


    #region Merging
    public void StartMerge(Transform MergeTransform, PlatletManager manager, bool small)
    {
        MergeReady = false;
        ChangeToMergeSprite();
        toggleCollider(true);
        StartCoroutine(GoToMerger(MergeTransform, manager, small));

    }

    void ChangeToMergeSprite()
    {
        if (mySpriteRender != null)
        {
            if (mySprites.Length > 1)
            {
                mySpriteRender.sprite = mySprites[1];
            }
            else
            {
                Debug.LogWarning("Error with lenght of sprites");
            }
        }
    }

    IEnumerator GoToMerger(Transform merger, PlatletManager manager, bool small)
    {
        Vector2 currentPos = transform.position;
        RB.velocity = Vector3.zero;
        while (!(Vector2.Distance(currentPos, merger.position) < 0.3f))
        {
            transform.position = Vector2.MoveTowards(transform.position, merger.position, 2 * Time.deltaTime);
            currentPos = transform.position;
            yield return null;
        }
        merger.localScale = new Vector2(merger.localScale.x * 1.1f, merger.localScale.y * 1.1f); ;
        mySpriteRender.enabled = false;
        Debug.Log("Arrived");
        AudioManager.Instance.Play("MergeBubble");
        MergeReady = true;
        toggleCollider(false);
        manager.CheckIfMergeisDone(small, merger.gameObject);
    }



    #endregion Merging

    void toggleCollider(bool trigger)
    {
        if (trigger)
        {
            myCollider.isTrigger = true;
        }
        else
        {
            myCollider.isTrigger = false;
        }
    }
    public void Despawn()
    {
        StopCoroutine(WaitCoroutine);
        gameObject.SetActive(false);
    }

    // Moverse.
    // Detectar si hay una herida.

}
