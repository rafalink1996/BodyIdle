using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITopLeanTween : MonoBehaviour
{
    [SerializeField] GameObject TransitionImage;
    RectTransform TransitionImageRectTransform;
    [SerializeField] float TweenTimeTransition;

    void Start()
    {
        TransitionImageRectTransform = TransitionImage.GetComponent<RectTransform>();
        TransitionImage.SetActive(true);
        Invoke("TransitionOut", 0.5f);
    }


    // Transition
    public void TransitionIn()
    {
        EnableTransition();
        LeanTween.cancel(TransitionImage);
        LeanTween.scaleY(TransitionImage, 1, TweenTimeTransition);
        LeanTween.moveLocalY(TransitionImage, 0, TweenTimeTransition);
        
    }
    public void TransitionOut()
    {
        LeanTween.cancel(TransitionImage);
       // LeanTween.scaleY(TransitionImage, 0, TweenTimeTransition).setEase(LeanTweenType.easeInExpo);
        LeanTween.moveLocalY(TransitionImage, -3413, TweenTimeTransition).setEase(LeanTweenType.easeInExpo);


        Invoke("EnableTransition", TweenTimeTransition);
    
    }

    void EnableTransition()
    {
        if (!TransitionImage.activeSelf)
        {
            TransitionImage.SetActive(true);
        }
        else
        {
            TransitionImage.SetActive(false);
        }
    }


}
