using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private bool pointerDown;
    private float pointerDownTimer;
    public float requierdHoldTime = 1;
    public bool interactable = true;
    [SerializeField]
    ColorBlock colorBlock = new ColorBlock
    {
        normalColor = new Color(1, 1, 1, 1),
        highlightedColor = new Color(0.7f, 0.7f, 0.7f, 1),
        pressedColor = new Color(0.5f, 0.5f, 0.5f, 1),
        selectedColor = new Color(1, 1, 1, 1),
        disabledColor = new Color(1, 1, 1, 0.5f),
    };
    Image image;

    public UnityEvent OnLongClick;
    public UnityEvent OnShortClick;


    [SerializeField] private Image fillImage;


    private void Start()
    {
        image = GetComponent<Image>();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (interactable)
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }

            pointerDown = true;
        }

        //throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (interactable)
        {
            CheckTime();
            Reset();
        }
        //throw new System.NotImplementedException();
    }
    void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requierdHoldTime)
            {
                if (OnLongClick != null)
                {
                    OnLongClick.Invoke();
                    Reset();
                }
            }
            if (fillImage != null)
            {
                fillImage.fillAmount = pointerDownTimer / requierdHoldTime;
            }
            image.color = colorBlock.pressedColor;
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
        Reset();
    }

    void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
        if (fillImage != null)
        {
            fillImage.fillAmount = pointerDownTimer / requierdHoldTime;
        }
        if (image != null)
        {
            image.color = colorBlock.normalColor;
        }
    }
}
