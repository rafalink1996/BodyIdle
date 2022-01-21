using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrganInfo : MonoBehaviour
{
    [Header("References")]
    OrganManager OM;
    GameManager GM;
    CanvasGroup myCanvasGroup;
    [SerializeField] RectTransform Holder;

    [Space(10)]
    [Header("OrganSprites")]
    [SerializeField] Sprite[] organSprites;


    [Header("Organ Info")]
    [SerializeField] TextMeshProUGUI OrganName;
    [SerializeField] TextMeshProUGUI OrganDisplayNumber;
    [SerializeField] Image OrganImage;

    [Space(10)]
    [Header("Cell Info")]
    [Header("Red")]
    [SerializeField] TextMeshProUGUI RedCount;
    [SerializeField] TextMeshProUGUI RedEnergy;
    [SerializeField] TextMeshProUGUI RedPercentage;
    [Header("White")]
    [SerializeField] TextMeshProUGUI WhiteCount;
    [Header("Helper")]
    [SerializeField] TextMeshProUGUI HelperCount;

    [Space(10)]
    [Header("Sell Info")]
    [SerializeField] TextMeshProUGUI SellEnergy;
    [SerializeField] TextMeshProUGUI SellComplexity;

    [Header("Current Ogran Info")]
    int myOrganType;
    int myOrganNumber;

    private void Awake()
    {
        GetReferences();
    }

    public void SetInfo(int organType, int OrganNumber)
    {
        //Debug.Log("organ Type = " + organType);
        GetReferences();
        myOrganType = organType;
        myOrganNumber = OrganNumber;

        OrganImage.sprite = organSprites[organType];
        OrganName.text = OM.organTypes[organType].Name;
        OrganDisplayNumber.text = "#" + (OrganNumber + 1);

        int RedCells = countCells(organType, OrganNumber, 0);
        int WhiteCells = countCells(organType, OrganNumber, 1);
        int HelperCells = countCells(organType, OrganNumber, 2);

        RedCount.text = RedCells.ToString();
        WhiteCount.text = WhiteCells.ToString();
        HelperCount.text = HelperCells.ToString();

        double Energy = CalculateRedProduction(organType, RedCells);
        RedEnergy.text = AbbreviationUtility.AbbreviateNumber(Energy);
        float Percentage = calculateRedPercentage(Energy);
        RedPercentage.text = Percentage.ToString("F1") + "%";
    }

    void GetReferences()
    {
        if (GM == null)
        {
            GM = GameManager.gameManager;
        }
        if (GM != null)
        {
            if (OM == null)
            {
                OM = GM.organManager;
            }
        }
        if (myCanvasGroup == null)
        {
            myCanvasGroup = GetComponent<CanvasGroup>();
        }
    }


    int countCells(int organType, int organNumber, int cellType)
    {
        int CellCount = 0;
        if (OM.organTypes[organType].organs.Count > organNumber)
        {
            for (int i = 0; i < OM.organTypes[organType].organs[organNumber].CellTypes[cellType].cellSizes.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        CellCount += OM.organTypes[organType].organs[organNumber].CellTypes[cellType].cellSizes[i].CellsInfos.Count;
                        break;
                    case 1:
                        CellCount += OM.organTypes[organType].organs[organNumber].CellTypes[cellType].cellSizes[i].CellsInfos.Count * 10;
                        break;
                    case 2:
                        CellCount += OM.organTypes[organType].organs[organNumber].CellTypes[cellType].cellSizes[i].CellsInfos.Count * 100;
                        break;
                }
            }
        }
        return CellCount;
    }

    double CalculateRedProduction(int organType, int redCells)
    {
        double PointsPerSecond = redCells * OM.organTypes[organType].pointsMultiplier;
        return PointsPerSecond;
    }

    float calculateRedPercentage(double EnergyPerSecond)
    {
        float percentage = 0;
        if (GM.pointsManager.pointsPerSecond != 0)
        {
            percentage = (float)(EnergyPerSecond / GM.pointsManager.pointsPerSecond);
        }
        percentage = percentage * 100;
        return percentage;
    }

    public void SellClick()
    {

    }
    public void Close()
    {
        //LeanTween.cancel(Holder.gameObject);
        LeanTween.scale(Holder, Vector2.zero, 1).setEase(LeanTweenType.easeInOutExpo);
        //LeanTween.cancel(this.gameObject);
        LTDescr l = LeanTween.alphaCanvas(myCanvasGroup, 0, 1).setDelay(0.7f);
        l.setOnComplete(DisableCanvas);
        void DisableCanvas()
        {
            myCanvasGroup.interactable = false;
            myCanvasGroup.blocksRaycasts = false;
        }
    }

    public void Show()
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.alphaCanvas(myCanvasGroup, 1, 0);
        myCanvasGroup.interactable = true;
        myCanvasGroup.blocksRaycasts = true;
        LeanTween.cancel(Holder.gameObject);
        LeanTween.scale(Holder, Vector2.zero, 0);
        LeanTween.scale(Holder, Vector2.one, 1).setEase(LeanTweenType.easeInOutExpo).setDelay(0.7f);
    }
}
