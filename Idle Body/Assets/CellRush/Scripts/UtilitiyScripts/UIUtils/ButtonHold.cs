using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using System;

//[RequireComponent(typeof(Button))]
public class ButtonHold : Button
{
    [Header("BOOLS")]
    public bool Expand = false;
    public bool useColor = true;
    public bool useFillImage = false;


    [Header("PARAMS")]
    public float requierdHoldTime = 1;
    public float perecentThreshold = 0.2f;
    public float expandAmount = 1.2f;

    [Header("REFEREMNCES")]
    public Image fillImage;


    [Header("PRIVATE")]
    private bool pointerDown;
    private float pointerDownTimer;

    [Serializable]
    public class HoldButtonEvent : UnityEvent { }
    [Header ("EVENTS")]
    public HoldButtonEvent OnLongClick;
    public HoldButtonEvent OnShortClick;

    protected override void Awake()
    {
        base.Awake();
    }


    void Update()
    {
        if (interactable)
        {
            if (pointerDown)
            {
                pointerDownTimer += Time.deltaTime;
                if (useFillImage) FillImage();
                if (pointerDownTimer >= requierdHoldTime)
                {
                    if (OnLongClick != null)
                    {
                        OnLongClick.Invoke();
                        ResetParams();
                    }
                }
            }
        }
    }
    void CheckTime()
    {
        if (pointerDown)
        {
            if (pointerDownTimer < requierdHoldTime)
            {
                if (OnShortClick != null)
                {
                    OnShortClick.Invoke();
                }

            }
        }
        ResetParams();
    }

    void ResetParams()
    {
        pointerDown = false;
        pointerDownTimer = 0;

    }

    void FillImage()
    {
        if (fillImage == null) return;
        float Percentage = (pointerDownTimer) / requierdHoldTime;
        fillImage.fillAmount = Percentage;
    }
    void ResetImage()
    {
        if (fillImage == null) return;
        fillImage.fillAmount = 0;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        pointerDown = true;
        if (Expand)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        CheckTime();
        transform.localScale = Vector3.one;
        ResetParams();
        ResetImage();
        
    }
}
