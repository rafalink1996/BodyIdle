using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    GameManager gameManager;


    private Vector3 panelLocation;
    private Vector3 StartingPos;
    public float perecentThreshold = 0.2f;
    public float easing = 0.5f;
    bool ScreenSpace = true;
    public List<GameObject> Holders;
    [SerializeField] bool CanChange = true;
    public bool Locked = false;
    


    void Start()
    {
        gameManager = GameManager.gameManager;
        panelLocation = transform.position;
        StartingPos = transform.position;
        transform.position = StartingPos;
        //Debug.Log(panelLocation);
    }
    public void OnDrag(PointerEventData data)
    {
        if (CanChange && !Locked)
        {
            if (ScreenSpace)
            {
                float difference = Camera.main.ScreenToWorldPoint(data.pressPosition).x - Camera.main.ScreenToWorldPoint(data.position).x;
                transform.position = panelLocation - new Vector3(difference, 0, 0);
            }
            else
            {
                float difference = data.pressPosition.x - data.position.x;
                transform.position = panelLocation - new Vector3(difference, 0, 0);
            }
        }

    }
    public void OnEndDrag(PointerEventData data)
    {
        if (CanChange && !Locked)
        {
            float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
            if (Mathf.Abs(percentage) >= perecentThreshold)
            {
                Vector3 newLocation = panelLocation;
                //Debug.Log("Start = " + newLocation);
                bool Left = false;
                if (ScreenSpace)
                {
                    float x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + (Screen.width / 2), 0, 0)).x;
                    if (percentage > 0)
                    {
                        Left = false;
                        newLocation += new Vector3(-x, 0, 0);
                    }
                    else if (percentage < 0)
                    {
                        Left = true;
                        newLocation += new Vector3(x, 0, 0);
                    }
                }
                else
                {
                    if (percentage > 0)
                    {
                        Left = false;
                        newLocation += new Vector3(-(Screen.width * 2), 0, 0);
                    }
                    else if (percentage < 0)
                    {
                        Left = true;
                        newLocation += new Vector3(Screen.width * 2, 0, 0);
                    }
                }
                //Debug.Log("End = " + newLocation);
                if (Left)
                {
                    StartCoroutine(SmoothMove(transform.position, newLocation, easing, -1));
                }
                else
                {
                    StartCoroutine(SmoothMove(transform.position, newLocation, easing, 1));
                }
                panelLocation = newLocation;
            }
            else
            {
                StartCoroutine(SmoothMove(transform.position, panelLocation, easing, 0));
            }
        }
    }

    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds, int movement)
    {
        float t = 0f;
        CanChange = false;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1f, t));
            yield return null;
        }

        switch (movement)
        {
            default: //Didn't Move
                break;
            case 1: // Move Right 
                transform.position = StartingPos;
                panelLocation = StartingPos;
                Holders[0].transform.SetAsLastSibling();
                Holders.Clear();
                for (int i = 0; i < transform.childCount; i++)
                {
                    Holders.Add(transform.GetChild(i).gameObject);
                }
                SwipeRight();
                break;
            case -1: // Move Left
                transform.position = StartingPos;
                panelLocation = StartingPos;
                Holders[2].transform.SetAsFirstSibling();
                Holders.Clear();
                for (int i = 0; i < transform.childCount; i++)
                {
                    Holders.Add(transform.GetChild(i).gameObject);
                }
                SwipeLeft();
                break;
        }
        CanChange = true;
    }

    void SwipeRight()
    {
        gameManager.OrganViewUI.UpdateOrganViews(false);
    }

    void SwipeLeft()
    {
        //int UnlockedOrgans = 1;
        //for (int i = 0; i < gameManager.organManager.organTypes.Length; i++)
        //{
        //    if (gameManager.organManager.organTypes[i].unlocked)
        //    {
        //        UnlockedOrgans++;
        //    }
        //}
        //gameManager.organManager.activeOranType -= 1;
        //if (gameManager.organManager.activeOranType < 0)
        //{
        //    gameManager.organManager.activeOranType = UnlockedOrgans;
        //}
        gameManager.OrganViewUI.UpdateOrganViews(true);
    }


}
