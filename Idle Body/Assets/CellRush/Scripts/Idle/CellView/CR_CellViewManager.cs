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
        [SerializeField] CR_CellView_Translator _cellTranslator;
        [SerializeField] GameObject _cellViewHolder;
        [SerializeField] Camera _mainCamera;
 
        public enum cellType { RedBloodCell, WhiteBloodCell, HelperTCell }
        public cellType _cellSelected;

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
            CR_Idle_Manager.onGameStateChange += CR_Idle_Manager_onGameStateChange;
        }

        private void Start()
        {
            //Debug.Log(AbbreviationUtility.AbbreviateBigDoubleNumber(debug));
        }

        private void OnDestroy()
        {
            CR_Idle_Manager.onGameStateChange -= CR_Idle_Manager_onGameStateChange;
        }

        private void CR_Idle_Manager_onGameStateChange(CR_Idle_Manager.GameState state)
        {
            if (state != CR_Idle_Manager.GameState.CellView)
            {
                ResetCellView();
                _cellViewHolder.SetActive(false);
                return;
            }
            _cellTranslator.UpdateTexts();
            _cellView_UI.UpdateCellCost();
            _cellManager._merging = false;
            _cellView_UI.UpdateBackground();
            _cellView_UI.ResetCellViewUi();
            _cellViewHolder.SetActive(true);
            canBuy = true;
            _cellManager.ResetCells();
            _cellManager.SpawnCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber);
        }



        void ResetCellView()
        {

            //_cellSelected = cellType.RedBloodCell;
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
            if (_cellSelected == (cellType)cellTypeIndex) return;
            if (cellTypeIndex > 2) return;

            _cellSelected = (cellType)cellTypeIndex;
            _cellView_UI.ChangeCellTypeUI((cellType)cellTypeIndex);
        }

        public void OnClickBuyCell()
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
                    _cellView_UI.UpdateCellCost();
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


        public void OnClickViewCells()
        {
            if (_cellManager._merging) return;
            _cellView_UI.UpdateCellInfos();

        }

        public void OnClickOrganButton()
        {
            var multiplier = CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].pointsMultiplier;
            Debug.Log("+" + multiplier);
            CR_Data.data.SetEnergy(CR_Data.data._energy + multiplier);
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                IconPopUp.Create(touch.position);
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


    }
}
