using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellView_UI_Animations : MonoBehaviour
{
    [SerializeField] float TweenTime = 2f;
    private bool UIHidden = false;

    // Change Cell Type UI
   
    public GameObject CellTypeBar;
    RectTransform CellTypeBarTransform;
    public GameObject CellSlotsMask;
    public GameObject BuyCellButton;
    public GameObject StickyImage;
    public GameObject ArrowTabIndicator;
    GridLayoutGroup gridLayoutGroup;

    CellView_UI_Manager MyCellViewUiManager;
    [SerializeField] AnimationCurve CustomElastic;

    private void Start()
    {
        CellTypeBarTransform = CellTypeBar.GetComponent<RectTransform>();
        MyCellViewUiManager = GetComponent<CellView_UI_Manager>();
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

            
            LeanTween.cancel(ArrowTabIndicator);
            LeanTween.rotate(ArrowTabIndicator, new Vector3(0, 0, 180), TweenTime / 4);

        }
        else
        {
            LeanTween.cancel(gameObject);
           
            LeanTween.moveLocal(gameObject, new Vector3(0, 0, 0), TweenTime/1.5f).setEase(LeanTweenType.easeOutElastic);
            UIHidden = false;
            LeanTween.cancel(StickyImage);
            LeanTween.scaleY(StickyImage, 1f, TweenTime / 1.5f).setEase(LeanTweenType.easeOutElastic);
            
            LeanTween.cancel(ArrowTabIndicator);
            LeanTween.rotate(ArrowTabIndicator, new Vector3(0, 0, 0), TweenTime / 4);


        }
    }

    public void ChangeSelectedCellType(int CellType)
    {

        MyCellViewUiManager.previousCellType = MyCellViewUiManager.CurrentCellType;
        if (MyCellViewUiManager.CurrentCellType == CellType)
        {
            return;
        }
        ScrollRect CellSlotsScrollRect = CellSlotsMask.GetComponent<ScrollRect>();
        CellSlotsScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        CellSlotsScrollRect.enabled = false;
     
        
        LeanTween.cancel(CellTypeBar);

        float CellSize = ((gridLayoutGroup.cellSize.x * Mathf.Abs(MyCellViewUiManager.CheckCellSlotTotal(MyCellViewUiManager.previousCellType))) + 120);
        float CellTypePos = CellSize + ((1420 - CellSize) / 2);
        //print(CellTypePos + " :CellTypePos" + " - CellType " + previousCellType);

        LTDescr d = LeanTween.moveLocal(CellTypeBar, new Vector3(-CellTypePos, 0,0), TweenTime/4).setEase(LeanTweenType.easeInQuad);
     

        BuyCellButton.GetComponent<Button>().interactable = false;
        LeanTween.scale(BuyCellButton, new Vector3(0, 0, 0), TweenTime/4).setEase(LeanTweenType.easeInExpo);
       
        if (CellType == 1)
        {
            MyCellViewUiManager.CurrentCellType = 1;
        }
        else if (CellType == 2)
        {
            MyCellViewUiManager.CurrentCellType = 2;
        }
        else if (CellType == 3)
        {
            MyCellViewUiManager.CurrentCellType = 3;
        }

        d.setOnComplete(ChangeCells);

       // Invoke("ChangeCells", 1f);
    }


    void ChangeCells()
    {
        MyCellViewUiManager.ChangeCellType(MyCellViewUiManager.CurrentCellType);
    }

    public void FinishChangeCellType()
    {

        float CellSize = ((gridLayoutGroup.cellSize.x * Mathf.Abs(MyCellViewUiManager.CheckCellSlotTotal(MyCellViewUiManager.CurrentCellType))) + 120);
        float CellTypePos = CellSize + ((1420 - CellSize) / 2);
        //print(CellTypePos + " :CellTypePos 2" + " - CellType " + previousCellType);
        CellTypeBar.transform.localPosition = new Vector3(-CellTypePos, 0, 0);
        LeanTween.cancel(CellTypeBar);
        float MovePosition = 65 * (MyCellViewUiManager.CheckCellSlotTotal(MyCellViewUiManager.CurrentCellType) - 10);
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
