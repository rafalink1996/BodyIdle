using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CR_OrganismView_Anim : MonoBehaviour
{

    [SerializeField] RectTransform BuyUI;
    [SerializeField] Vector2 BuyUiStartingPos;
    private void Start()
    {
        StartCoroutine(WaitForEndFarme());
    }
    IEnumerator WaitForEndFarme()
    {
        yield return new WaitForEndOfFrame();
        BuyUiStartingPos = BuyUI.localPosition;
    }

    public void HideUI()
    {
        LeanTween.cancel(BuyUI.gameObject);
        LeanTween.moveLocalY(BuyUI.gameObject, BuyUiStartingPos.y , 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete =>{

        });
    }

    public void ShowUI()
    {
        LeanTween.cancel(BuyUI.gameObject);
        LeanTween.moveLocalY(BuyUI.gameObject, BuyUiStartingPos.y + BuyUI.rect.height, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(complete => {

        });
    }

    float GetCanvasHeight()
    {
        float S_Width = Screen.width;
        float S_Height = Screen.height;

        float Ratio = 0;
        if (S_Height < S_Width)
        {
            Ratio = S_Width / S_Height;
        }
        else
        {
            Ratio = S_Height / S_Width;
        }
        return Ratio * 1080;

    }
}
