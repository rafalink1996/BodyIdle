using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CellSlot : MonoBehaviour
{
    CellsSO myObject;
    [SerializeField] private OrganManager.OrganType.OrganInfo.cellsType.CellSizes.CellInfo myInfo;
    int myType;
    

    [SerializeField] Image BGImage;
    [SerializeField] Image CellImage;
    [SerializeField] GameObject DeathTimerObject;
    [SerializeField] Image DeathTimerCellImage;
    [SerializeField] TextMeshProUGUI Timer;

    private void Update()
    {
        if(myInfo != null)
        {
            if(myInfo.alive == false)
            {
                DeathTimerObject.SetActive(true);
                UpdateTimer(myInfo.timer);
            }
            else
            {
                DeathTimerObject.SetActive(false);
            }
        }
    }
    public void UpdateSlot(int type, CellsSO cellSO, OrganManager.OrganType.OrganInfo.cellsType.CellSizes.CellInfo Info)
    {
        myType = type;
        myObject = cellSO;
        if(Info != null)
        {
            myInfo = Info;
        }
        else
        {
            Debug.LogWarning("Error: info is null at - cellSlot");
        }
       

        BGImage.color = cellSO.MyColor;
        switch (type)
        {
            case 1:
                CellImage.sprite = cellSO.Cellx1;
                DeathTimerCellImage.sprite = cellSO.Cellx1;
                CellImage.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                break;

            case 2:
                CellImage.sprite = cellSO.Cellx10;
                DeathTimerCellImage.sprite = cellSO.Cellx10;
                CellImage.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                break;

            case 3:
                CellImage.sprite = cellSO.Cellx100;
                DeathTimerCellImage.sprite = cellSO.Cellx100;
                CellImage.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                break;
        }

    }

    public void UpdateTimer(float time)
    {
        Timer.text = time.ToString("F1");
    }

    public int GetCellSlotType()
    {
        return myType;
    }

}
