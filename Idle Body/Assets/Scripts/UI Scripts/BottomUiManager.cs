using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUiManager : MonoBehaviour
{
    public GameObject UiCellCountcontainer;
    [SerializeField] GameObject SlotPrefab;
    [SerializeField] Image BuyButton, BuyButtonCost, BuyButtonCellImage;
    


    [SerializeField] CellsSO RedBloodCell, WhiteBloodCell, HelperCell;
   

    [SerializeField] int RedBloodCell1;
    [SerializeField] int RedBloodCell10;
    [SerializeField] int RedBloodCell100;

    [SerializeField] int whiteBloodCell1;
    [SerializeField] int whiteBloodCell10;
    [SerializeField] int whiteBloodCell100;

    [SerializeField] int HelperCell1;
    [SerializeField] int HelperCell10;
    [SerializeField] int HelperCell100;


    public void ChangeCellType(int CellType)
    {
        for (int i = 0; i < UiCellCountcontainer.transform.childCount; i++)
        {
           Destroy(UiCellCountcontainer.transform.GetChild(i).gameObject);
        }

       if (CellType == 1)
        {
            for (int i = 0; i < RedBloodCell1; i++)
            {
               GameObject RedCell = Instantiate(SlotPrefab, UiCellCountcontainer.transform.position, Quaternion.identity) as GameObject;
                //RedCell.transform.parent = UiCellCountcontainer.transform;
                RedCell.transform.SetParent(UiCellCountcontainer.transform);
                Image RedCellBGColor = RedCell.GetComponent<Image>();
                RedCellBGColor.color = RedBloodCell.MyColor;

                GameObject RedCellImageObject = RedCell.transform.GetChild(1).gameObject;
                Image RedCellImage = RedCellImageObject.GetComponent<Image>();
                RedCellImage.sprite = RedBloodCell.Cellx1;
            }

            for (int i = 0; i < RedBloodCell10; i++)
            {
                GameObject RedCell = Instantiate(SlotPrefab, UiCellCountcontainer.transform.position, Quaternion.identity) as GameObject;
                // RedCell.transform.parent = UiCellCountcontainer.transform;
                RedCell.transform.SetParent(UiCellCountcontainer.transform);
                Image RedCellBGColor = RedCell.GetComponent<Image>();
                RedCellBGColor.color = RedBloodCell.MyColor;

                GameObject RedCellImageObject = RedCell.transform.GetChild(1).gameObject;
                Image RedCellImage = RedCellImageObject.GetComponent<Image>();
                RedCellImage.sprite = RedBloodCell.Cellx10;
            }

            for (int i = 0; i < RedBloodCell100; i++)
            {
                GameObject RedCell = Instantiate(SlotPrefab, UiCellCountcontainer.transform.position, Quaternion.identity) as GameObject;
                //RedCell.transform.parent = UiCellCountcontainer.transform;
                RedCell.transform.SetParent(UiCellCountcontainer.transform);
                Image RedCellBGColor = RedCell.GetComponent<Image>();
                RedCellBGColor.color = RedBloodCell.MyColor;

                GameObject RedCellImageObject = RedCell.transform.GetChild(1).gameObject;
                Image RedCellImage = RedCellImageObject.GetComponent<Image>();
                RedCellImage.sprite = RedBloodCell.Cellx100;
            }

            BuyButton.color = RedBloodCell.MyColor;
            BuyButtonCost.color = RedBloodCell.MyColor;
            BuyButtonCellImage.sprite = RedBloodCell.Cellx1;

        }

        if (CellType == 2)
        {
            for (int i = 0; i < whiteBloodCell1; i++)
            {
                GameObject WhiteCell = Instantiate(SlotPrefab, UiCellCountcontainer.transform.position, Quaternion.identity) as GameObject;
                // WhiteCell.transform.parent = UiCellCountcontainer.transform;
                WhiteCell.transform.SetParent(UiCellCountcontainer.transform);
                Image WhiteCellBGColor = WhiteCell.GetComponent<Image>();
                WhiteCellBGColor.color = WhiteBloodCell.MyColor;

                GameObject WhiteCellImageObject = WhiteCell.transform.GetChild(1).gameObject;
                Image WhiteCellImage = WhiteCellImageObject.GetComponent<Image>();
                WhiteCellImage.sprite = WhiteBloodCell.Cellx1;
            }

            for (int i = 0; i < whiteBloodCell10; i++)
            {
                GameObject WhiteCell = Instantiate(SlotPrefab, UiCellCountcontainer.transform.position, Quaternion.identity) as GameObject;
                //WhiteCell.transform.parent = UiCellCountcontainer.transform;
                WhiteCell.transform.SetParent(UiCellCountcontainer.transform);
                Image WhiteCellBGColor = WhiteCell.GetComponent<Image>();
                WhiteCellBGColor.color = WhiteBloodCell.MyColor;

                GameObject WhiteCellImageObject = WhiteCell.transform.GetChild(1).gameObject;
                Image WhiteCellImage = WhiteCellImageObject.GetComponent<Image>();
                WhiteCellImage.sprite = WhiteBloodCell.Cellx10;

            }

            for (int i = 0; i < whiteBloodCell100; i++)
            {
                GameObject WhiteCell = Instantiate(SlotPrefab, UiCellCountcontainer.transform.position, Quaternion.identity) as GameObject;
                //WhiteCell.transform.parent = UiCellCountcontainer.transform;
                WhiteCell.transform.SetParent(UiCellCountcontainer.transform);
                Image WhiteCellBGColor = WhiteCell.GetComponent<Image>();
                WhiteCellBGColor.color = WhiteBloodCell.MyColor;

                GameObject WhiteCellImageObject = WhiteCell.transform.GetChild(1).gameObject;
                Image WhiteCellImage = WhiteCellImageObject.GetComponent<Image>();
                WhiteCellImage.sprite = WhiteBloodCell.Cellx100;


            }

            BuyButton.color = WhiteBloodCell.MyColor;
            BuyButtonCost.color = WhiteBloodCell.MyColor;
            BuyButtonCellImage.sprite = WhiteBloodCell.Cellx1;
        }

        if (CellType == 3)
        {
            for (int i = 0; i < HelperCell1; i++)
            {
                GameObject HelperCellO = Instantiate(SlotPrefab, UiCellCountcontainer.transform.position, Quaternion.identity) as GameObject;
                //HelperCellO.transform.parent = UiCellCountcontainer.transform;
                HelperCellO.transform.SetParent(UiCellCountcontainer.transform);
                Image HelperCellBGColor = HelperCellO.GetComponent<Image>();
                HelperCellBGColor.color = HelperCell.MyColor;

                GameObject HelperCellImageObject = HelperCellO.transform.GetChild(1).gameObject;
                Image HelperCellImage = HelperCellImageObject.GetComponent<Image>();
                HelperCellImage.sprite = HelperCell.Cellx1;
            }

            for (int i = 0; i < HelperCell10; i++)
            {
                GameObject HelperCellO = Instantiate(SlotPrefab, UiCellCountcontainer.transform.position, Quaternion.identity) as GameObject;
                //HelperCellO.transform.parent = UiCellCountcontainer.transform;
                HelperCellO.transform.SetParent(UiCellCountcontainer.transform);
                Image HelperCellBGColor = HelperCellO.GetComponent<Image>();
                HelperCellBGColor.color = HelperCell.MyColor;

                GameObject HelperCellImageObject = HelperCellO.transform.GetChild(1).gameObject;
                Image HelperCellImage = HelperCellImageObject.GetComponent<Image>();
                HelperCellImage.sprite = HelperCell.Cellx1;

            }

            for (int i = 0; i < HelperCell100; i++)
            {
                GameObject HelperCellO = Instantiate(SlotPrefab, UiCellCountcontainer.transform.position, Quaternion.identity) as GameObject;
                //HelperCellO.transform.parent = UiCellCountcontainer.transform;
                HelperCellO.transform.SetParent(UiCellCountcontainer.transform);
                Image HelperCellBGColor = HelperCellO.GetComponent<Image>();
                HelperCellBGColor.color = HelperCell.MyColor;

                GameObject HelperCellImageObject = HelperCellO.transform.GetChild(1).gameObject;
                Image HelperCellImage = HelperCellImageObject.GetComponent<Image>();
                HelperCellImage.sprite = HelperCell.Cellx1;
            }

            BuyButton.color = HelperCell.MyColor;
            BuyButtonCost.color = HelperCell.MyColor;
            BuyButtonCellImage.sprite = HelperCell.Cellx1;
        }

    }
}
