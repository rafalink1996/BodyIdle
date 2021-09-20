using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TopUI_Manager : MonoBehaviour
{
    [SerializeField] RectTransform TransitionObject;
    [SerializeField] RectTransform TransitionImageMask;
    Image transitionImage;
    [SerializeField] float TweenTimeTransition;
    [SerializeField] TextMeshProUGUI PointsText, PointsPerSecondText;
    [SerializeField] GameManager myGameManager;
    [SerializeField] NewPointsManager pointsManager;

    enum LoadingScreenActions { ShowCellViewUI, ShowOrganViewUI, ShowOrganismViewUI }
    LoadingScreenActions NextAction;
    public bool CanTransition;

    [Header ("Game Views")]
    [SerializeField] GameObject CellViewUI;
    [SerializeField] GameObject OrganViewUI;
    [SerializeField] GameObject OrganismViewUI;

    public void customStart()
    {
        myGameManager = GameManager.gameManager;
        if (myGameManager != null)
            pointsManager = myGameManager.pointsManager;

        transitionImage = TransitionObject.GetComponent<Image>();
        SetTransitionImage();
        TransitionOut();
    }

    void SetTransitionImage()
    {
        TransitionObject.anchorMax = new Vector2(0.5f, 0.5f);
        TransitionObject.anchorMin = new Vector2(0.5f, 0.5f);
        TransitionObject.anchoredPosition = new Vector2(0, 0);
        float screenHeight = (Screen.height * 1920) / Screen.width;
        TransitionObject.sizeDelta = new Vector2(1920, screenHeight);
    }

    private void Update()
    {
        PointsText.text = AbbreviationUtility.AbbreviateNumber(pointsManager.totalPoints);
        PointsPerSecondText.text = AbbreviationUtility.AbbreviateNumber(pointsManager.PointsPerSecond()) + " /s";

        //Testing
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TransitionOut();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TransitionIn();
        }
    }
    void TransitionIn()
    {
        ToggleRaycast();
        LeanTween.cancel(TransitionImageMask);
        float screenHeight = (Screen.height * 1920) / Screen.width;
        LTDescr d = LeanTween.size(TransitionImageMask, new Vector2(-1920, 0), 1).setEase(LeanTweenType.easeInOutExpo);
        d.setOnComplete(LoadingScreenAction);
    }
    public void TransitionOut()
    {
        LeanTween.cancel(TransitionImageMask);
        LTDescr d = LeanTween.size(TransitionImageMask, new Vector2(2640, Screen.height), 1).setEase(LeanTweenType.easeInOutExpo);
        d.setOnComplete(ToggleRaycast);
    }

    public void ChangeView(int view)
    {
        switch (view)
        {
            case 0: //cell View
                NextAction = LoadingScreenActions.ShowCellViewUI;
                TransitionIn();
                break;
            case 1: //Organ View
                NextAction = LoadingScreenActions.ShowOrganViewUI;
                TransitionIn();
                break;
            case 2: //Organism View
                NextAction = LoadingScreenActions.ShowOrganismViewUI;
                TransitionIn();
                break;
        }
    }

    void LoadingScreenAction()
    {

        myGameManager.organManager.cellSpawner.myCellMerger.DestroyMergerReference();
        switch (NextAction)
        {
            case LoadingScreenActions.ShowCellViewUI:
                myGameManager.changeView(0);
                TransitionOut();
                break;
            case LoadingScreenActions.ShowOrganViewUI:
                myGameManager.changeView(1);
                TransitionOut();
                break;
            case LoadingScreenActions.ShowOrganismViewUI:
                myGameManager.changeView(2);
                TransitionOut();
                break;
        }
    }

    void ToggleRaycast()
    {
        if (transitionImage.raycastTarget == false) 
        {
            transitionImage.raycastTarget = true;
        }
        else
        {
            transitionImage.raycastTarget = false;
        }

    }
}
