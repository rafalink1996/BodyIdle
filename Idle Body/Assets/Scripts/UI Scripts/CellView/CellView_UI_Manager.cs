using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CellView_UI_Manager : MonoBehaviour
{
    public GameObject UiCellCountcontainer;
    [SerializeField] GameObject SlotPrefab;

    [SerializeField] Image BuyButton, BuyButtonCost, BuyButtonCellImage;

    [SerializeField] CellsSO RedBloodCell, WhiteBloodCell, HelperCell;
    [SerializeField] CellView_UI_Animations MyUILeanTween;
    [SerializeField] TextMeshProUGUI CellCost;
    OrganManager myOrganManager;
    public int CurrentCellType = 0;
    public int previousCellType = 0;

    [System.Serializable]
    public class SlotSize
    {
        public string name;
        public List<GameObject> ActiveCellSlots = new List<GameObject>();

    }
    public SlotSize[] SlotSizes = new SlotSize[]
    {
        new SlotSize
        {
            name = "Small Cells Slots",
        },
           new SlotSize
        {
            name = "Medium Cells Slots",
        },
              new SlotSize
        {
            name = "Big Cells Slots",
        },
    };

    [System.Serializable]
    public class pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<pool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    string[] CellTag = new string[]{
    "SmallCellSlot",
    "MediumCellSlot",
    "BigCellSlot"
    };

    private void Awake()
    {
        MyUILeanTween = GetComponent<CellView_UI_Animations>();
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (pool pool in pools)
        {
            Queue<GameObject> ObjectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(UiCellCountcontainer.transform);
                ObjectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.tag, ObjectPool);
            //Debug.Log("Added pool with key " + pool.tag);
        }
    }

    public GameObject SpawnFroomPool(string tag)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("pool With tag" + tag + " deosn't exist");
            return null;
        }
        GameObject ObjectToSpawn = PoolDictionary[tag].Dequeue();
        ObjectToSpawn.SetActive(true);
        PoolDictionary[tag].Enqueue(ObjectToSpawn);
        return ObjectToSpawn;
    }

    public void CustomStart()
    {
        myOrganManager = GameManager.gameManager.organManager;
        //UpdateCellCost();
    }

    #region From input Methods
    public void StartChangeCell(int cellType, bool changeView = false)
    {
        MyUILeanTween.ChangeSelectedCellType(cellType, changeView);
        
    }
    public void StartCellTab()
    {
        MyUILeanTween.UiTabToggle();
    }
    public void StartBuyCell()
    {
        MyUILeanTween.BuyCellTween();
        //ChangeCellType(CurrentCellType, true);
        BuyCellEffect(CurrentCellType);
        UpdateCellCost();
    }

    public void UpdateCellCost()
    {
        if(myOrganManager != null)
        {
            if (myOrganManager.organTypes[myOrganManager.activeOrganType].organs != null)
            {
                if (myOrganManager.organTypes[myOrganManager.activeOrganType].organs.Count != 0)
                {
                    if(CellCost != null)
                    {
                        CellCost.text = AbbreviationUtility.AbbreviateNumber(myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CurrentCellType].currentCellCost);  
                    }
                    else
                    {
                        Debug.Log("Cell Cost is null");
                    }
                   
                }
            }         
        }
        else
        {
            Debug.LogWarning("Organ Manager is null");
        }
       
    }
    #endregion From input Methods


    void BuyCellEffect(int CellType)
    {
        //CellType -= 1;
        int[] CellTotals = new int[3];
        /* ---- et Total Number of Cell Slots ---- */
        for (int i = 0; i < SlotSizes.Length; i++)
        {
            CellTotals[i] = SlotSizes[i].ActiveCellSlots.Count;
        }

        /* ---- Assign CellType ---- */
        CellsSO cellsSO = null;
        switch (CellType)
        {
            case 0:
                cellsSO = RedBloodCell;
                break;
            case 1:
                cellsSO = WhiteBloodCell;
                break;
            case 2:
                cellsSO = HelperCell;
                break;
        }


        for (int a = 0; a < myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes.Count; a++)
        {
            /* ---- Check if cells merged ---- */
            int organNumber = myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes[a].CellsInfos.Count;
            if (organNumber == 0 && CellTotals[a] != organNumber)
            {
                for (int x = 0; x < SlotSizes[a].ActiveCellSlots.Count; x++)
                {
                    SlotSizes[a].ActiveCellSlots[x].SetActive(false);
                }
                SlotSizes[a].ActiveCellSlots.Clear();
            }
        }

        /* ---- Get New Cell Slot ---- */


        for (int a = 0; a < myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes.Count; a++)
        {
            for (int c = SlotSizes[a].ActiveCellSlots.Count; c < myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes[a].CellsInfos.Count; c++)
            {
                GameObject Cell = SpawnFroomPool(CellTag[a]);
                Cell.transform.SetParent(UiCellCountcontainer.transform);
                Cell.transform.localScale = new Vector3(0, 0, 0);
                Cell.transform.SetAsLastSibling();
                LeanTween.scale(Cell, new Vector3(1, 1, 1), 0.8f).setEase(LeanTweenType.easeOutExpo);
                Cell.TryGetComponent(out CellSlot cellslot);
                cellslot.UpdateSlot(a + 1, cellsSO, myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes[a].CellsInfos[c]);
                SlotSizes[a].ActiveCellSlots.Add(Cell);
            }
        }
    }




    public void ChangeCellType(int CellType, bool Buy = false)
    {
        //CellType -= 1;
        for (int i = 0; i < SlotSizes.Length; i++) // go through all sizes
        {
            if (SlotSizes[i].ActiveCellSlots.Count != 0) // chek if there are slots of current size
            {
                for (int o = 0; o < SlotSizes[i].ActiveCellSlots.Count; o++) //  go through all slots of current size
                {
                    SlotSizes[i].ActiveCellSlots[o].SetActive(false);
                }
                SlotSizes[i].ActiveCellSlots.Clear();

            }
        }
        CellsSO cellsSO = RedBloodCell;

        switch (CellType)
        {
            case 0:
                cellsSO = RedBloodCell;
                break;
            case 1:
                cellsSO = WhiteBloodCell;
                break;
            case 2:
                cellsSO = HelperCell;
                break;
            default:
                cellsSO = RedBloodCell;
                break;
        }

        if (myOrganManager.organTypes[myOrganManager.activeOrganType].organs.Count != 0)
        {
            Debug.Log(myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].name);
            for (int c = 0; c < myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes.Count; c++)
            {

                if (myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes[c].CellsInfos.Count != 0)
                {
                    for (int v = 0; v < myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes[c].CellsInfos.Count; v++)
                    {
                        GameObject Cell = SpawnFroomPool(CellTag[c]);
                        Cell.transform.SetParent(UiCellCountcontainer.transform);
                        Cell.transform.localScale = new Vector3(1, 1, 1);
                        Cell.transform.SetAsFirstSibling();
                        Cell.GetComponent<CellSlot>().UpdateSlot(c + 1, cellsSO, myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].cellSizes[c].CellsInfos[v]);
                        SlotSizes[c].ActiveCellSlots.Add(Cell);
                    }
                }
            }
        }
        BuyButton.color = cellsSO.MyColor;
        BuyButtonCost.color = cellsSO.MyColor;
        BuyButtonCellImage.sprite = cellsSO.Cellx1;

        if (Buy == false)
        {
            MyUILeanTween.FinishChangeCellType();
        }

        UpdateCellCost();
    }

    public float CheckCellSlotTotal(int celltype)
    {
        float CellTotal = 0;

        for (int i = 0; i < SlotSizes.Length; i++)
        {
            CellTotal += SlotSizes[i].ActiveCellSlots.Count;
        }
        return CellTotal;
    }

    public void StartCellTimer(float timer, int cellType, int cellslotID)
    {
        List<GameObject> SlotSizeList = null;
        SlotSizeList = SlotSizes[cellType].ActiveCellSlots;
        SlotSizeList[cellslotID].TryGetComponent(out CellSlot slot);
        StartCoroutine(CellDeathTimer(timer, slot));
    }

    IEnumerator CellDeathTimer(float time, CellSlot Cell)
    {
        bool Dead = true;
        float timeDead = time;
        while (Dead)
        {
            if (timeDead > 0)
            {
                timeDead -= Time.deltaTime;
                Cell.UpdateTimer(timeDead);
            }
            else
            {
                Cell.UpdateTimer(timeDead);
                Dead = false;
            }
            yield return null;
        }
    }


}








