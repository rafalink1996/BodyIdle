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
    public class cellPool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public List<cellPool> pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    string[] CellTag = new string[]{
    "SmallCellSlot",
    "MediumCellSlot",
    "BigCellSlot"
    };


    [Header("Background")]
    [SerializeField] Image Border;
    [SerializeField] Animator BorderAnimator;
    [SerializeField] Image background;
    [SerializeField] Image backButtonImage;

    [SerializeField] Sprite[] organSprites;

    private void Awake()
    {
        MyUILeanTween = GetComponent<CellView_UI_Animations>();
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (cellPool pool in pools)
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
            //Debug.Log(myOrganManager.organTypes[myOrganManager.activeOrganType].organs[myOrganManager.activeOrganID].CellTypes[CellType].name);
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

    public void UpdateBackground(int organType)
    {
        Border.sprite = myOrganManager.organTypes[organType].border;
        background.color = myOrganManager.organTypes[organType].backgroundColor;
        BorderAnimator.runtimeAnimatorController = myOrganManager.organTypes[organType].borderAnimation;
        backButtonImage.sprite = organSprites[myOrganManager.activeOrganType];
    }


}

//namespace Idle
//{
//    public class CR_CellView_UI : MonoBehaviour
//    {
//        [Header("GENERAL ELEMENTS")]
//        [SerializeField] Transform _StickyImage;
//        [SerializeField] RectTransform _UIHolder;
//        [SerializeField] Transform _toggleArrow;
//        [SerializeField] Button _toggleButton;
//        [Header("CELL INFO ELEMENTS")]
//        [SerializeField] List<CR_CellView_CellInfo> _smallCellInfoList;
//        [SerializeField] List<CR_CellView_CellInfo> _medCellInfoList;
//        [SerializeField] List<CR_CellView_CellInfo> _bigCellInfoList;
//        [SerializeField] Transform _cellInfoMask;

//        float OrginialY;


//        [Header("CELL AMOUNT ELEMENTS")]
//        [SerializeField] TextMeshProUGUI _cellAmountText;
//        [SerializeField] Image _cellamountObject;

//        [Header("BUY BUTTON ELEMENTS")]
//        [SerializeField] Button _buyButton;
//        [SerializeField] TextMeshProUGUI _cellCostText;
//        [SerializeField] Image _cellImage;
//        [SerializeField] Sprite[] _cellSprites;


//        [Header("BACKGROUND ELEMENTS")]
//        [SerializeField] Image _backgroundImage;
//        [SerializeField] Image _borderImage;


//        [Header("VARIABLES")]
//        public bool _uiShown = true;
//        Vector2 _UIStartPos;

//        [Header("POOLING VARIABLES")]
//        [SerializeField] CR_CellView_CellInfo _cellInfoPF;
//        [SerializeField] Transform _cellInfoHolder;
//        [SerializeField] int infoSizeCount;
//        public Dictionary<string, Queue<CR_CellView_CellInfo>> PoolDictionary;
//        string[] _tag = new string[] { "SInfo", "MInfo", "BInfo" };

//        private void Awake()
//        {
//            PoolDictionary = new Dictionary<string, Queue<CR_CellView_CellInfo>>();
//            _UIStartPos = _UIHolder.localPosition;
//            InsantiatePools();
//            if (_cellInfoMask.TryGetComponent(out RectTransform rt))
//            {
//                OrginialY = rt.sizeDelta.y;
//            }
//        }

//        void InsantiatePools()
//        {
//            var data = CR_Data.data;

//            for (int p = 0; p < _tag.Length; p++)
//            {
//                Queue<CR_CellView_CellInfo> ObjectPool = new Queue<CR_CellView_CellInfo>();
//                for (int i = 0; i < infoSizeCount; i++)
//                {
//                    CR_CellView_CellInfo obj = Instantiate(_cellInfoPF);
//                    obj.gameObject.SetActive(false);
//                    obj.name = (CR_CellBase.CellSize)p + " info " + i;
//                    obj.transform.SetParent(_cellInfoHolder.transform);
//                    obj.transform.localPosition = Vector3.zero;
//                    ObjectPool.Enqueue(obj);
//                }
//                Debug.Log(_tag[p]);
//                PoolDictionary.Add(_tag[p], ObjectPool);
//            }
//        }


//        CR_CellView_CellInfo SpawnFroomPool(string tag)
//        {
//            if (!PoolDictionary.ContainsKey(tag))
//            {
//                Debug.LogWarning("pool With tag" + tag + " doesn't exist");
//                return null;
//            }
//            CR_CellView_CellInfo ObjectToSpawn = PoolDictionary[tag].Dequeue();
//            ObjectToSpawn.gameObject.SetActive(true);
//            PoolDictionary[tag].Enqueue(ObjectToSpawn);
//            return ObjectToSpawn;
//        }

//        public void SpawnCellInfos(CR_CellBase.CellType type)
//        {
//            var sizes = CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].organs[CR_Idle_Manager.instance.CurrentOrganNumber].CellTypes[(int)type - 1].cellSizes;
//            for (int i = 0; i < sizes.Count; i++)
//            {
//                for (int o = 0; o < sizes[i].CellsInfos.Count; o++)
//                {
//                    switch (i)
//                    {
//                        case 0:
//                            SpawnCellInfo(type, CR_CellBase.CellSize.Small, sizes[i].CellsInfos[o]);
//                            break;
//                        case 1:
//                            SpawnCellInfo(type, CR_CellBase.CellSize.Medium, sizes[i].CellsInfos[o]);
//                            break;
//                        case 2:
//                            SpawnCellInfo(type, CR_CellBase.CellSize.Big, sizes[i].CellsInfos[o]);
//                            break;
//                    }
//                }
//            }
//        }

//        public void SpawnCellInfo(CR_CellBase.CellType type, CR_CellBase.CellSize size, CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo info)
//        {
//            Debug.Log(_tag[(int)size]);
//            var cellInfo = SpawnFroomPool(_tag[(int)size]);
//            cellInfo.transform.localScale = Vector3.one;
//            cellInfo.ActivateCellInfo(info, type, size);
//            switch (size)
//            {
//                case CR_CellBase.CellSize.Small:
//                    _smallCellInfoList.Add(cellInfo);
//                    break;
//                case CR_CellBase.CellSize.Medium:
//                    _medCellInfoList.Add(cellInfo);
//                    break;
//                case CR_CellBase.CellSize.Big:
//                    _bigCellInfoList.Add(cellInfo);
//                    break;
//            }

//        }
//        public void ClearCellInfos()
//        {
//            for (int i = 0; i < _smallCellInfoList.Count; i++)
//            {
//                _smallCellInfoList[i].DeactivateCellInfo();
//            }
//            _smallCellInfoList.Clear();
//            for (int i = 0; i < _medCellInfoList.Count; i++)
//            {
//                _medCellInfoList[i].DeactivateCellInfo();
//            }
//            _medCellInfoList.Clear();
//            for (int i = 0; i < _bigCellInfoList.Count; i++)
//            {
//                _bigCellInfoList[i].DeactivateCellInfo();
//            }
//            _bigCellInfoList.Clear();
//        }

//        public IEnumerator ClearCellInfos(CR_CellBase.CellSize size)
//        {
//            var cellInfoList = _smallCellInfoList;
//            _cellInfoHolder.localPosition = new Vector3(0, _cellInfoHolder.localPosition.y, _cellInfoHolder.localPosition.z);
//            switch (size)
//            {
//                case CR_CellBase.CellSize.Small:
//                    cellInfoList = _smallCellInfoList;
//                    break;
//                case CR_CellBase.CellSize.Medium:
//                    cellInfoList = _medCellInfoList;
//                    break;
//                case CR_CellBase.CellSize.Big:
//                    cellInfoList = _bigCellInfoList;
//                    break;
//            }
//            for (int i = cellInfoList.Count; i > 0; i--)
//            {
//                bool done = false;
//                LeanTween.scale(cellInfoList[i].gameObject, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInExpo).setOnComplete(AnimEnd => { done = true; });
//                while (!done)
//                {
//                    yield return null;
//                }
//                cellInfoList[i].transform.localScale = Vector3.one;
//                cellInfoList[i].DeactivateCellInfo();
//            }
//            cellInfoList.Clear();
//        }

//        public void ResetCellViewUi()
//        {
//            UpdateCellNumber();
//        }

//        public void ToggleUI()
//        {
//            if (_uiShown)
//            {
//                LeanTween.scaleY(_StickyImage.gameObject, .2f, 0.3f).setEase(LeanTweenType.easeOutElastic);
//                LeanTween.cancel(_toggleArrow.gameObject);
//                LeanTween.rotateZ(_toggleArrow.gameObject, 180, 1).setEase(LeanTweenType.easeOutExpo);
//                LeanTween.moveLocalY(_UIHolder.gameObject, (_UIStartPos.y - (_UIHolder.rect.height / 1.5f)), .5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(done => { _uiShown = false; });

//            }
//            else
//            {
//                LeanTween.scaleY(_StickyImage.gameObject, 1, 0.3f).setEase(LeanTweenType.easeOutElastic);
//                LeanTween.cancel(_toggleArrow.gameObject);
//                LeanTween.rotateZ(_toggleArrow.gameObject, 0, 1).setEase(LeanTweenType.easeOutExpo);
//                LeanTween.moveLocalY(_UIHolder.gameObject, _UIStartPos.y, .5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(donde => { _uiShown = true; });
//            }
//        }

//        public void UpdateBackground()
//        {
//            CR_Idle_Manager.OrganTypeAsstes assets = CR_Idle_Manager.instance.organTypeAsstes[CR_Idle_Manager.instance.CurrentOrganType];
//            if (assets == null)
//            {
//                Debug.Log("Assets is null");
//                return;
//            }
//            if (assets.OrganBackgroundColor != Color.clear) _backgroundImage.color = assets.OrganBackgroundColor;
//            if (assets.OrganBorder != null) _borderImage.sprite = assets.OrganBorder;

//        }

//        public void ChangeCellTypeUI(CR_CellViewManager.cellType type)
//        {
//            CR_CellViewManager.instance.canBuy = false;
//            Color UIcolor = new Color(1, .48f, .48f, 1);
//            CR_CellBase.CellType cellBaseType = CR_CellBase.CellType.RedBlood;
//            switch (type)
//            {
//                case CR_CellViewManager.cellType.RedBloodCell:
//                    UIcolor = new Color(1, .48f, .48f, 1);
//                    cellBaseType = CR_CellBase.CellType.RedBlood;
//                    break;
//                case CR_CellViewManager.cellType.WhiteBloodCell:
//                    UIcolor = new Color(.82f, .82f, .82f, 1);
//                    cellBaseType = CR_CellBase.CellType.White;
//                    break;
//                case CR_CellViewManager.cellType.HelperTCell:
//                    UIcolor = new Color(.42f, .97f, 1, 1);
//                    cellBaseType = CR_CellBase.CellType.Helper;
//                    break;
//                default:
//                    break;
//            }

//            LeanTween.scale(_buyButton.gameObject, _buyButton.transform.localScale * 1.2f, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
//            {
//                if (_buyButton.TryGetComponent(out Image image)) image.color = UIcolor;
//                _cellImage.sprite = _cellSprites[(int)type];
//                UpdateCellCost();
//                LeanTween.scale(_buyButton.gameObject, Vector2.one, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
//                {
//                    CR_CellViewManager.instance.canBuy = true;
//                });
//            });
//            LeanTween.scale(_cellamountObject.gameObject, _cellamountObject.transform.localScale * 1.2f, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
//            {
//                _cellamountObject.color = UIcolor;
//                UpdateCellNumber();
//                LeanTween.scale(_cellamountObject.gameObject, Vector2.one, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
//                {
//                    CR_CellViewManager.instance.canBuy = true;
//                });
//            });

//            if (_cellInfoMask.TryGetComponent(out RectTransform rt))
//            {
//                LeanTween.cancel(_cellInfoMask.gameObject);
//                LeanTween.size(rt, new Vector2(rt.sizeDelta.x, 0), 0.4f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
//                {
//                    ClearCellInfos();
//                    SpawnCellInfos(cellBaseType);
//                    LeanTween.size(rt, new Vector2(rt.sizeDelta.x, OrginialY), 1.4f).setEase(LeanTweenType.easeOutExpo);
//                });
//            }
//        }

//        public void UpdateCellNumber()
//        {
//            switch (CR_CellViewManager.instance._cellSelected)
//            {
//                case CR_CellViewManager.cellType.RedBloodCell:
//                    _cellAmountText.text = CR_Data.data.GetTotalCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber, 0).ToString();
//                    break;
//                case CR_CellViewManager.cellType.WhiteBloodCell:
//                    _cellAmountText.text = CR_Data.data.GetTotalCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber, 1).ToString();
//                    break;
//                case CR_CellViewManager.cellType.HelperTCell:
//                    _cellAmountText.text = CR_Data.data.GetTotalCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber, 2).ToString();
//                    break;
//            }
//        }

//        public void UpdateCellCost()
//        {

//        }
//    }
//}








