using BreakInfinity;
using iconPopUp;
using UnityEngine;

namespace Idle
{
    public class CR_CellViewManager : MonoBehaviour
    {
        public static CR_CellViewManager instance;
        [Header("REFERENCES")]
        public CR_CellView_UI _cellView_UI;
        public CR_CellView_CellManager _cellManager;
        public CR_CellViewPathogens _cellViewPathogens;
        [SerializeField] CR_CellView_Translator _cellTranslator;
        [SerializeField] GameObject _cellViewHolder;
        [SerializeField] Camera _mainCamera;

        public enum cellType { RedBloodCell, WhiteBloodCell, HelperTCell }
        public cellType _cellSelected;


        // ----BUY MAX VARIABLES--- //
        public enum BuyAmount { x1, x5, x10, max };
        [Header("BUY MAX")]
        public BuyAmount buyAmount;
        public BigDouble buyMaxCost;
        public int buyMaxAmount;
        [HideInInspector] public float CachedGrowthRate;
        BigDouble cachedInitialCost;
        [HideInInspector] public int _organType;
        [HideInInspector] public int _organNumber;


        // ----BUY MAX VARIABLES--- //

        public bool canBuy;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            if (_mainCamera == null) _mainCamera = Camera.main;
            if (_cellTranslator == null) _cellTranslator = FindObjectOfType<CR_CellView_Translator>();
            if (_cellManager == null) _cellManager = FindObjectOfType<CR_CellView_CellManager>();
            if (_cellView_UI == null) _cellView_UI = FindObjectOfType<CR_CellView_UI>();
            if (_cellViewPathogens == null) _cellViewPathogens = GetComponent<CR_CellViewPathogens>();
            CR_Idle_Manager.onGameStateChange += CR_Idle_Manager_onGameStateChange;
            CR_Data.onLanguageChange += CR_Data_onLanguageChange;
        }


        private void Start()
        {
            //Debug.Log(AbbreviationUtility.AbbreviateBigDoubleNumber(debug));
        }

        private void OnDestroy()
        {
            CR_Idle_Manager.onGameStateChange -= CR_Idle_Manager_onGameStateChange;
            CR_Data.onLanguageChange -= CR_Data_onLanguageChange;
        }

        private void CR_Data_onLanguageChange()
        {
            _cellTranslator.UpdateTexts();
        }


        private void Update()
        {
            UpdateBuyMaxAmount();
        }

        void UpdateBuyMaxAmount()
        {
            if (buyAmount != BuyAmount.max) return;
            var Buymax = GetBuyMax();
            if (buyMaxAmount != Buymax)
            {

                buyMaxAmount = Buymax;
                _cellView_UI.UpdateBuyAmount(buyAmount);
                _cellView_UI.UpdateCellCost(buyAmount);
            }
            else
            {
                buyMaxAmount = Buymax;
            }
        }

        public int GetBuyMax()
        {
            var cellSelected = (int)_cellSelected;
            var cellAmount = CR_Data.data.GetTotalCells(_organType, _organNumber, cellSelected);
            var energy = CR_Data.data._energy;
            var Buymax = System.Math.Floor(BigDouble.Log(energy * (CachedGrowthRate - 1) / (cachedInitialCost * System.Math.Pow(CachedGrowthRate, cellAmount)) + 1, CachedGrowthRate));
            switch (cellSelected)
            {
                case 0:
                case 1:
                    if (Buymax + cellAmount > 1000)
                    {
                        Buymax = 1000 - cellAmount;
                    }
                    break;
                case 2:
                    if (Buymax + cellAmount > 18)
                    {
                        Buymax = 27 - cellAmount;
                    }
                    break;
            }
            
            return (int)Buymax;
        }

        public BigDouble GetBuyMaxCost(int cellAmount)
        {
            BigDouble cost = 0;

            var cellSelected = (int)_cellSelected;
            var baseCost = CR_Data.data.organTypes[_organType].organs[_organNumber].CellTypes[cellSelected].initialCellCost;
            var cellNumber = CR_Data.data.GetTotalCells(_organType, _organNumber, cellSelected);
            var amount = cellAmount;
            if (amount == 0)
            {
                amount = 1;
            }
            for (int i = 0; i < amount; i++)
            {
                cost += baseCost * Mathf.Pow(CachedGrowthRate, cellNumber + i);
            }

            return cost;
        }

        private void CR_Idle_Manager_onGameStateChange(CR_Idle_Manager.GameState state)
        {
            if (state != CR_Idle_Manager.GameState.CellView)
            {
                ResetCellView();
                _cellViewHolder.SetActive(false);
                return;
            }
            _organType = CR_Idle_Manager.instance.CurrentOrganType;
            _organNumber = CR_Idle_Manager.instance.CurrentOrganNumber;
            var cellSelected = (int)_cellSelected;
            cachedInitialCost = CR_Data.data.organTypes[_organType].organs[_organNumber].CellTypes[cellSelected].initialCellCost;
            CachedGrowthRate = CR_Data.data.organTypes[_organType].organs[_organNumber].CellTypes[cellSelected].growthRate;

            _cellTranslator.UpdateTexts();
            _cellView_UI.UpdateBuyAmount(buyAmount);
            _cellView_UI.UpdateCellCost(buyAmount);
            _cellManager._merging = false;
            _cellView_UI.UpdateBackground();
            _cellView_UI.ResetCellViewUi();
            _cellViewHolder.SetActive(true);
            canBuy = true;
            _cellManager.ResetCells();
            _cellManager.SpawnCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber);
            SpawnPathogens();
        }



        void ResetCellView()
        {

            //_cellSelected = cellType.RedBloodCell;
        }

        void BuySingleCell()
        {
            if (!canBuy) return;
            if (!_cellView_UI._uiShown) return;
            if (_cellManager._merging) return;
            var organType = CR_Idle_Manager.instance.CurrentOrganType;
            var organNumber = CR_Idle_Manager.instance.CurrentOrganNumber;
            var cellSelected = (int)_cellSelected;


            var cost = CR_Data.data.organTypes[organType].organs[organNumber].CellTypes[cellSelected].currentCellCost;
            var initialCost = CR_Data.data.organTypes[organType].organs[organNumber].CellTypes[cellSelected].initialCellCost;
            var costMultiplier = CR_Data.data.organTypes[organType].organs[organNumber].CellTypes[cellSelected].growthRate;
            var cellAmount = CR_Data.data.GetTotalCells(organType, organNumber, cellSelected);

            if (CR_Data.data._energy >= cost)
            {
                if (cellAmount < 1000)
                {
                    CR_Data.data.SetEnergy(CR_Data.data._energy - cost);
                    StartCoroutine(_cellManager.CheckCells());
                    CR_Data.data.organTypes[organType].organs[organNumber].CellTypes[cellSelected].currentCellCost = initialCost * BigDouble.Pow(costMultiplier, cellAmount + 1);
                    _cellView_UI.UpdateCellCost(buyAmount);
                    CR_Data.data.GetEnergyPerSecond();
                }
                else
                {
                    Debug.Log("max cells reached");
                }
            }
            else
            {
                Debug.Log("Not Enought Energy: " + AbbreviationUtility.AbbreviateBigDoubleNumber(CR_Data.data._energy) + "/" + AbbreviationUtility.AbbreviateBigDoubleNumber(cost));
            }
        }

        void BuyMultipleCells(int amount)
        {
            if (!canBuy) return;
            if (!_cellView_UI._uiShown) return;
            if (_cellManager._merging) return;
            if (amount == 0) return;
            if (amount == 1)
            {
                BuySingleCell();
                return;
            }
            var cellSelected = (int)_cellSelected;
            var cellAmount = CR_Data.data.GetTotalCells(_organType, _organNumber, cellSelected);
            var organType = CR_Idle_Manager.instance.CurrentOrganType;
            var organNumber = CR_Idle_Manager.instance.CurrentOrganNumber;
            var initialCost = CR_Data.data.organTypes[organType].organs[organNumber].CellTypes[cellSelected].initialCellCost;
            var costMultiplier = CR_Data.data.organTypes[organType].organs[organNumber].CellTypes[cellSelected].growthRate;
            BigDouble cost = GetBuyMaxCost(amount);


            if (CR_Data.data._energy >= cost)
            {
                switch (cellSelected)
                {
                    case 0:
                    case 1:

                        if (cellAmount < 1000)
                        {
                            CR_Data.data.SetEnergy(CR_Data.data._energy - cost);
                            StartCoroutine(_cellManager.CheckBuyMaxCells(amount));
                            CR_Data.data.organTypes[organType].organs[organNumber].CellTypes[cellSelected].currentCellCost = initialCost * BigDouble.Pow(costMultiplier, CR_Data.data.GetTotalCells(_organType, _organNumber, cellSelected));
                            _cellView_UI.UpdateCellCost(buyAmount);
                            CR_Data.data.GetEnergyPerSecond();

                        }
                        else
                        {
                            Debug.Log("max cells reached");
                        }
                        break;

                    case 2:
                        if (cellAmount < 27)
                        {
                            Debug.Log(amount);
                            CR_Data.data.SetEnergy(CR_Data.data._energy - cost);
                            StartCoroutine(_cellManager.CheckBuyMaxCells(amount));
                            CR_Data.data.organTypes[organType].organs[organNumber].CellTypes[cellSelected].currentCellCost = initialCost * BigDouble.Pow(costMultiplier, CR_Data.data.GetTotalCells(_organType, _organNumber, cellSelected));
                            _cellView_UI.UpdateCellCost(buyAmount);
                            CR_Data.data.GetEnergyPerSecond();
                        }
                        else
                        {
                            Debug.Log("max cells reached");
                        }
                        break;
                }
            }
            else
            {
                Debug.Log("Not Enought Energy: " + AbbreviationUtility.AbbreviateBigDoubleNumber(CR_Data.data._energy) + "/" + AbbreviationUtility.AbbreviateBigDoubleNumber(cost));
            }
        }

        #region OnClickMethods

        public void OnClickToggleUi()
        {
            _cellView_UI.ToggleUI();
        }
        public void OnClickBackToOrgan()
        {

            StartCoroutine(CR_Idle_Manager.instance.ChangeState(CR_Idle_Manager.GameState.OrganView));
        }

        public void OnClickSelectCellType(int cellTypeIndex)
        {
            var organType = CR_Idle_Manager.instance.CurrentOrganType;
            var organNumber = CR_Idle_Manager.instance.CurrentOrganNumber;
            var cellsTypes = CR_Data.data.organTypes[organType].organs[organNumber].CellTypes.Length;
            if (cellTypeIndex >= cellsTypes) return;
            if (_cellSelected == (cellType)cellTypeIndex)
            {
                if (!_cellView_UI._selectCellUiShown)
                {
                    _cellView_UI.ShowCellTypesOptions();
                }
                else
                {
                    _cellView_UI.CloseCellTypesOptions(cellTypeIndex);
                }
            }
            else
            {

                _cellSelected = (cellType)cellTypeIndex;
                _cellView_UI.ChangeCellTypeUI((cellType)cellTypeIndex);
                _cellView_UI.CloseCellTypesOptions(cellTypeIndex);
            }



        }

        public void OnClickBuyCell()
        {
            switch (buyAmount)
            {
                case BuyAmount.x1:
                    BuySingleCell();
                    break;
                case BuyAmount.x5:
                    BuyMultipleCells(5);
                    break;
                case BuyAmount.x10:
                    BuyMultipleCells(10);
                    break;
                case BuyAmount.max:
                    var amount = GetBuyMax();
                    BuyMultipleCells(amount);
                    break;
                default:
                    break;
            }
        }

        public void OnClickViewCells()
        {
            if (_cellManager._merging) return;
            _cellView_UI.UpdateCellInfos();

        }

        public void OnClickSelectNumAMount(bool more)
        {
            int index = (int)buyAmount;
            if (more)
            {
                if (buyAmount == BuyAmount.max) return;
                buyAmount = (BuyAmount)(index + 1);
                if (buyAmount == BuyAmount.max)
                {
                    buyMaxAmount = GetBuyMax();
                }
                _cellView_UI.UpdateCellCost(buyAmount);
                _cellView_UI.UpdateBuyAmount(buyAmount);
            }
            else
            {
                if (buyAmount == BuyAmount.x1) return;
                buyAmount = (BuyAmount)(index - 1);
                _cellView_UI.UpdateCellCost(buyAmount);
                _cellView_UI.UpdateBuyAmount(buyAmount);
            }

        }

        public void OnClickOrganButton()
        {
            var multiplier = CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].pointsMultiplier;
            Debug.Log("+" + multiplier);
            CR_Data.data.SetEnergy(CR_Data.data._energy + multiplier);
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 rawPos = _mainCamera.ScreenToWorldPoint(touch.position);
                Vector3 pos = new Vector3(rawPos.x, rawPos.y, 0);
                IconPopUp.Create(pos);
            }
            else
            {
                Debug.Log("no Touch");
                Vector3 rawPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 pos = new Vector3(rawPos.x, rawPos.y, 0);
                IconPopUp.Create(pos);
            }

        }


        #endregion OnClickMethods

        #region PathogenSpawner



        public void SpawnPathogens()
        {
            var organ = CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].organs[CR_Idle_Manager.instance.CurrentOrganNumber];
            if (!organ.infected) return;
            _cellViewPathogens.SetPathogens();


            //SpawnPathogens
        }
        #endregion PathogenSpawner


    }
}
