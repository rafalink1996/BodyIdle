using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBotLeanTween : MonoBehaviour
{
    [SerializeField] float TweenTime = 2f;
    private bool UIHidden = false;

    // Change Cell Type UI
    private int CurrentCellType = 0;
    private int previousCellType = 0;
    public GameObject CellTypeBar;
    RectTransform CellTypeBarTransform;
    public GameObject CellSlotsMask;
    public GameObject BuyCellButton;
    public GameObject StickyImage;
    public GameObject ArrowTabIndicator;
    GridLayoutGroup gridLayoutGroup;

    BottomUiManager MyBottomUiManager;
    [SerializeField] AnimationCurve CustomElastic;

    private void Start()
    {
        CellTypeBarTransform = CellTypeBar.GetComponent<RectTransform>();
        MyBottomUiManager = GetComponent<BottomUiManager>();
        gridLayoutGroup = CellTypeBar.GetComponent<GridLayoutGroup>();
    }
    public void CustomStart()
    {
        ChangeSelectedCellType(1);    
    }
    public void UiTabToggle()
    {
        if (!UIHidden)
        {
            
            LeanTween.cancel(gameObject);
            LeanTween.moveLocal(gameObject, new Vector3(0, -400, 0), TweenTime/1.5f).setEase(LeanTweenType.easeOutElastic);
            UIHidden = true;

            LeanTween.cancel(StickyImage);
            LeanTween.scaleY(StickyImage, .3f, TweenTime / 1.5f).setEase(LeanTweenType.easeOutElastic);

            //LeanTween.scaleY(ArrowTabIndicator, -1, 0);
            LeanTween.cancel(ArrowTabIndicator);
            LeanTween.rotate(ArrowTabIndicator, new Vector3(0, 0, 180), TweenTime / 4);

        }
        else
        {
            LeanTween.cancel(gameObject);
            //LeanTween.isTweening(gameObject);
            LeanTween.moveLocal(gameObject, new Vector3(0, 0, 0), TweenTime/1.5f).setEase(LeanTweenType.easeOutElastic);
            UIHidden = false;
            LeanTween.cancel(StickyImage);
            LeanTween.scaleY(StickyImage, 1f, TweenTime / 1.5f).setEase(LeanTweenType.easeOutElastic);
            //LeanTween.scaleY(ArrowTabIndicator, 1, 0);
            LeanTween.cancel(ArrowTabIndicator);
            LeanTween.rotate(ArrowTabIndicator, new Vector3(0, 0, 0), TweenTime / 4);


        }
    }

    public void ChangeSelectedCellType(int CellType)
    {
        
        previousCellType = CurrentCellType;
        if (CurrentCellType == CellType)
        {
            return;
        }
        ScrollRect CellSlotsScrollRect = CellSlotsMask.GetComponent<ScrollRect>();
        CellSlotsScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        CellSlotsScrollRect.enabled = false;
     
        
        LeanTween.cancel(CellTypeBar);

        float CellSize = ((gridLayoutGroup.cellSize.x * Mathf.Abs(MyBottomUiManager.CheckCellTotal(previousCellType))) + 120);
        float CellTypePos = CellSize + ((1420 - CellSize) / 2);
        //print(CellTypePos + " :CellTypePos" + " - CellType " + previousCellType);

        LeanTween.moveLocal(CellTypeBar, new Vector3(-CellTypePos, 0,0), TweenTime/4).setEase(LeanTweenType.easeInQuad);
     

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

        Invoke("ChangeCells", 1f);
    }


    void ChangeCells()
    {
        MyBottomUiManager.ChangeCellType(CurrentCellType);
    }

    public void FinishChangeCellType()
    {

        float CellSize = ((gridLayoutGroup.cellSize.x * Mathf.Abs(MyBottomUiManager.CheckCellTotal(CurrentCellType))) + 120);
        float CellTypePos = CellSize + ((1420 - CellSize) / 2);
        //print(CellTypePos + " :CellTypePos 2" + " - CellType " + previousCellType);
        CellTypeBar.transform.localPosition = new Vector3(-CellTypePos, 0, 0);
        LeanTween.cancel(CellTypeBar);
        float MovePosition = 65 * (MyBottomUiManager.CheckCellTotal(CurrentCellType) - 10);
        //print(MovePosition + " :MovePosition");

        LeanTween.moveLocal(CellTypeBar, new Vector3(MovePosition, 0, 0), TweenTime / 8); //65*(CellTypeBar.transform.childCount-10)
        //Invoke("RenewScrollRect", TweenTime/8);

        LeanTween.cancel(BuyCellButton);
        LeanTween.scale(BuyCellButton, new Vector3(1, 1, 1), TweenTime / 4).setEase(LeanTweenType.easeInExpo);
        BuyCellButton.GetComponent<Button>().interactable = true;
        ScrollRect CellSlotsScrollRect = CellSlotsMask.GetComponent<ScrollRect>();
        CellSlotsScrollRect.movementType = ScrollRect.MovementType.Elastic;
        CellSlotsScrollRect.enabled = true;

    }

    public void BuyCellTween()
    {
        LeanTween.cancel(BuyCellButton);
        LeanTween.scale(BuyCellButton, new Vector3(1.2f, 1.2f, 1.2f), TweenTime / 4).setEase(LeanTweenType.easeOutExpo);
        LeanTween.scale(BuyCellButton, new Vector3(1, 1, 1), TweenTime / 4).setEase(LeanTweenType.easeInExpo).setDelay(TweenTime / 4);
    }






   
}
