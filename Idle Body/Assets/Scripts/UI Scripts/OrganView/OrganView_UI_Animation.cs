using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganView_UI_Animation : MonoBehaviour
{
    [Header("Normal UI")]
    [SerializeField] GameObject StickyImage;
    [SerializeField] GameObject bottom_UI;
    [SerializeField] GameObject ArrowTabIndicator;
    public bool UIHidden = false;
    [SerializeField] float TweenTime = 2f;
    [SerializeField] GameObject UIObject;
    [SerializeField] GameObject UIBuyObject;
    OrganView_Manager organView_Manager;


    private void Start()
    {
        LeanTween.moveLocal(UIBuyObject, new Vector3(0, -900, 0), 0);
        organView_Manager = GameManager.gameManager.OrganViewUI;
    }
    public void UiTabToggle()
    {
        if (!UIHidden)
        {
            
            LeanTween.cancel(bottom_UI);
            LeanTween.moveLocal(bottom_UI, new Vector3(0, -400, 0), TweenTime / 1.5f).setEase(LeanTweenType.easeOutElastic);
            UIHidden = true;

            LeanTween.cancel(StickyImage);
            LeanTween.scaleY(StickyImage, .3f, TweenTime / 1.5f).setEase(LeanTweenType.easeOutElastic);


            LeanTween.cancel(ArrowTabIndicator);
            LeanTween.rotate(ArrowTabIndicator, new Vector3(0, 0, 180), TweenTime / 4);
            organView_Manager.toggleButtonsInteractive(false);
            organView_Manager.PositionOrganIndicator(1);


        }
        else
        {
            LeanTween.cancel(bottom_UI);

            LeanTween.moveLocal(bottom_UI, new Vector3(0, 0, 0), TweenTime / 1.5f).setEase(LeanTweenType.easeOutElastic);
            UIHidden = false;
            LeanTween.cancel(StickyImage);
            LeanTween.scaleY(StickyImage, 1f, TweenTime / 1.5f).setEase(LeanTweenType.easeOutElastic);

            LeanTween.cancel(ArrowTabIndicator);
            LeanTween.rotate(ArrowTabIndicator, new Vector3(0, 0, 0), TweenTime / 4);
            organView_Manager.toggleButtonsInteractive(true);
            organView_Manager.PositionOrganIndicator(0);
        }
    }

    public void GoToBuyOrganUI()
    {
        //Debug.Log("Time to buy new organ");
        LeanTween.cancel(UIObject);
        LeanTween.moveLocal(UIObject, new Vector3(0, -900, 0), 0.5f).setEase(LeanTweenType.easeInExpo);

        LeanTween.cancel(UIBuyObject);
        LeanTween.moveLocal(UIBuyObject, new Vector3(0, 0, 0), 0.5f).setEase(LeanTweenType.easeInExpo);  
    }
    public void GoToNormalOrganUI()
    {
        //Debug.Log("back to normal Organ");
        LeanTween.cancel(UIBuyObject);
        LeanTween.moveLocal(UIBuyObject, new Vector3(0, -900, 0), 0.5f).setEase(LeanTweenType.easeInExpo);

        LeanTween.cancel(UIObject);
        LeanTween.moveLocal(UIObject, new Vector3(0, 0, 0), 0.5f).setEase(LeanTweenType.easeInExpo);
    }
}
