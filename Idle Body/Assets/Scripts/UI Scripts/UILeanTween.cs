﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeanTween : MonoBehaviour
{
    [SerializeField] float TweenTime = 2f;
    private bool UIHidden = false;

    // Change Cell Type UI
    private int CurrentCellType = 1;
    public GameObject CellTypeBar;
    public GameObject CellSlotsMask;
    public GameObject BuyCellButton;

   public void UiTabToggle()
    {
        if (!UIHidden)
        {
            LeanTween.cancel(gameObject);
            LeanTween.moveLocal(gameObject, new Vector3(0, -750, 0), TweenTime/1.5f).setEase(LeanTweenType.easeOutElastic);
            UIHidden = true;   
        }
        else
        {
            LeanTween.cancel(gameObject);
            //LeanTween.isTweening(gameObject);
            LeanTween.moveLocal(gameObject, new Vector3(0, -570, 0), TweenTime/1.5f).setEase(LeanTweenType.easeOutElastic);
            UIHidden = false;    
        }
    }

    public void ChangeSelectedCellType(int CellType)
    {
        if (CurrentCellType == CellType)
        {
            return;
        }
        ScrollRect CellSlotsScrollRect = CellSlotsMask.GetComponent<ScrollRect>();
        CellSlotsScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        CellSlotsScrollRect.enabled = false;
     

        LeanTween.cancel(CellTypeBar);
        LeanTween.moveLocal(CellTypeBar, new Vector3(-800,0,0), TweenTime/4).setEase(LeanTweenType.easeInQuad);

        BuyCellButton.GetComponent<Button>().interactable = false;
        LeanTween.scale(BuyCellButton, new Vector3(0, 0, 0), TweenTime/4).setEase(LeanTweenType.easeInExpo);

        if (CellType == 1)
        {
            CurrentCellType = 1;
        }
        else if (CellType == 2)
        {
            CurrentCellType = 2;
        }
        else if (CellType == 3)
        {
            CurrentCellType = 3;
        }

        Invoke("FinishChangeCellType", 1f);
    }

    void FinishChangeCellType()
    {
        LeanTween.cancel(CellTypeBar);
        LeanTween.moveLocal(CellTypeBar, new Vector3(10, 0, 0), TweenTime / 8);
        ScrollRect CellSlotsScrollRect = CellSlotsMask.GetComponent<ScrollRect>();
        CellSlotsScrollRect.movementType = ScrollRect.MovementType.Elastic;
        CellSlotsScrollRect.enabled = true;

        LeanTween.cancel(BuyCellButton);
        LeanTween.scale(BuyCellButton, new Vector3(1, 1, 1), TweenTime / 4).setEase(LeanTweenType.easeInExpo);
        BuyCellButton.GetComponent<Button>().interactable = true;

    }

    public void BuyCellTween()
    {
        LeanTween.cancel(BuyCellButton);
        LeanTween.scale(BuyCellButton, new Vector3(1.2f, 1.2f, 1.2f), TweenTime / 4).setEase(LeanTweenType.easeOutExpo);
        LeanTween.scale(BuyCellButton, new Vector3(1, 1, 1), TweenTime / 4).setEase(LeanTweenType.easeInExpo).setDelay(TweenTime / 4);
    }
}
