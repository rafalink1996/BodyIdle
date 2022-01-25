using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSwiperHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler
{
    [Header("BOOLS")]
    public bool interactable = true;
    public bool Expand = false;
    public bool Color = true;


    [Header("PARAMS")]
    public float requierdHoldTime = 1;
    public float perecentThreshold = 0.2f;
    [SerializeField]
    ColorBlock colorBlock = new ColorBlock
    {
        normalColor = new Color(1, 1, 1, 1),
        highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1),
        pressedColor = new Color(0.7f, 0.7f, 0.7f, 1),
        selectedColor = new Color(1, 1, 1, 1),
        disabledColor = new Color(1, 1, 1, 0.5f),
    };

    [Header("PRIVATE")]
    private bool pointerDown;
    private float pointerDownTimer;
    Image image;

    public UnityEvent OnLongClick;
    public UnityEvent OnShortClick;
    [Header("Test")]
    [SerializeField] float percentageTest;
    PageSwiper pageSwiper;
    [SerializeField] private Image fillImage;
    Color imageStartColor;


    private void Start()
    {
        image = GetComponent<Image>();
        pageSwiper = FindObjectOfType<PageSwiper>();
        if (pageSwiper == null)
        {
            pageSwiper = FindObjectOfType<PageSwiper>();
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        imageStartColor = image.color;
        image.color = imageStartColor + colorBlock.pressedColor;

        if (interactable)
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }

            pointerDown = true;
        }
        if (Expand)
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        //throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("Poiner Up");
        if (interactable)
        {
            if (pageSwiper == null)
            {
                CheckTime();
            }
            else
            {
                if (!pageSwiper.Dragging)
                {
                    CheckTime();
                }
            }

        }
        transform.localScale = Vector3.one;

        Reset();
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
                    if (pageSwiper == null)
                    {
                        OnLongClick.Invoke();
                    }
                    else
                    {
                        if (!pageSwiper.Dragging)
                        {
                            OnLongClick.Invoke();

                        }
                    }

                    Reset();
                }
            }
            if (fillImage != null)
            {
                fillImage.fillAmount = pointerDownTimer / requierdHoldTime;
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
            image.color = imageStartColor;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (pageSwiper != null)
        {
            pageSwiper.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (pageSwiper != null)
        {
            pageSwiper.OnEndDrag(eventData);
        }
    }
}
