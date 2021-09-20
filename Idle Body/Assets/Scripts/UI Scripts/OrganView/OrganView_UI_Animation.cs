using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganView_UI_Animation : MonoBehaviour
{

    [SerializeField] GameObject StickyImage;
    [SerializeField] GameObject bottom_UI;
    [SerializeField] GameObject ArrowTabIndicator;
    private bool UIHidden = false;
    [SerializeField] float TweenTime = 2f;

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


        }
    }
}
