using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CutoutMaskUI))]
public class CutOutMaskUtils : MonoBehaviour
{
   [SerializeField] bool SetMaskAnchorsToCenter;
    private void Start()
    {
        StartCoroutine(WaitToRedraw());
    }
    IEnumerator WaitToRedraw()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<CutoutMaskUI>().RecalculateMasking();
        if (SetMaskAnchorsToCenter)
        {
            Vector2 halfPoint = new Vector2(0.5f, 0.5f);
            var rect = GetComponent<RectTransform>();
            rect.anchorMin = halfPoint;
            rect.anchorMax = halfPoint;
       
            //rect.sizeDelta = new Vector2(Screen.width, Screen.height);
            rect.sizeDelta = new Vector2(1080, GetHeight());

        }
    }
     
    float GetHeight()
    {
        bool Portrait;
        float ratio;
        if(Screen.height > Screen.width) // Portrait
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
        return Height;
    }
}
