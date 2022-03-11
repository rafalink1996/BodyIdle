using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionAnimation : MonoBehaviour
{
    RectTransform rect;
    Image image;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    private void Start()
    {
        TransitionIn();
        //rect.sizeDelta = new Vector2(Screen.width, Screen.height);
        rect.sizeDelta = Vector2.zero;
    }

    public IEnumerator TransitionIn()
    {
        image.raycastTarget = true;
        bool Completed = false;
        LTDescr L = LeanTween.size(rect, Vector2.zero, 0.3f).setEase(LeanTweenType.easeOutExpo).setOnComplete(end => { Completed = true; });
        while (!Completed)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);

    }

    public IEnumerator TransitionOut()
    {
        //Vector2 outVector = new Vector2(Screen.width * 3.2f, Screen.height * 3.2f);
        float H = GetHeight() * 1.15f;
        Vector2 outVector = new Vector2(H, H);
        bool Completed = false;
        LTDescr L = LeanTween.size(rect, outVector, 0.3f).setEase(LeanTweenType.easeInExpo).setOnComplete(end => { Completed = true; });
        while (!Completed)
        {
            yield return null;
        }
        image.raycastTarget = false;
    }

    float GetHeight()
    {
        bool Portrait;
        float ratio;
        if (Screen.height > Screen.width) // Portrait
        {
            Portrait = true;
            ratio = (float)Screen.height / Screen.width;
        }
        else // Landscape
        {
            Portrait = false;
            ratio = (float)Screen.width / Screen.height;
        }

        float Height;

        if (Portrait)
        {
            Height = 1080 * ratio;
        }
        else
        {
            Height = 1920 * ratio;
        }
        //Debug.Log("Height: " + Height + " Portrait: " + Portrait + " Ratio: " + ratio);
        return Height;
    }
}
