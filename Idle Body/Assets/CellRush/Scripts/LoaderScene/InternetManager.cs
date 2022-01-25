using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class InternetManager : MonoBehaviour
{
    [SerializeField] GameObject NoInternetConnectionScreen;
    bool InternetChequed = false;
    float TimeTilnewCheck = 7;

    public void CheckConection()
    {
        StartCoroutine(CheckInternetConection());
    }

    private void Update()
    {
        if (!InternetChequed)
        {
            Debug.Log("Waiting for first check");
            TimeTilnewCheck -= Time.deltaTime;
            if(TimeTilnewCheck <= 0)
            {
                TimeTilnewCheck = 30;
                StartCoroutine(CheckInternetConection());
            }
        }
    }

    IEnumerator CheckInternetConection()
    {
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
        if(request.error != null)
        {
            Error();
        }
        else
        {
            Success();
        }
    }

    void Error()
    {
        InternetChequed = true;
        if (!NoInternetConnectionScreen.activeSelf)
        {
            NoInternetConnectionScreen.transform.localScale = Vector3.zero;
            NoInternetConnectionScreen.SetActive(true);
            LeanTween.scale(NoInternetConnectionScreen, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
        }
        Debug.Log("Error Conecting");
    }
    void Success()
    {
        InternetChequed = true;
        if (Manager.instance != null)
        {
            Manager.instance.playfabstart();
        }
        Debug.Log("Success Conecting");
    }

    public void Retry()
    {
        
        NoInternetConnectionScreen.SetActive(false);
        StartCoroutine(CheckInternetConection());
    }

    public void OnClickPlayOffline()
    {
        NoInternetConnectionScreen.SetActive(false);
        Manager.instance.gameloader.LoadGameScene();
    }
}
