using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idle
{
    public class CR_CellViewManager : MonoBehaviour
    {
        public static CR_CellViewManager instance;
        [Header("REFERENCES")]
        public CR_CellView_UI _cellView_UI;
        public CR_CellView_CellManager _cellManager;
        [SerializeField] GameObject _cellViewHolder;

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
            if (_cellManager == null) _cellManager = FindObjectOfType<CR_CellView_CellManager>();
            if (_cellView_UI == null) _cellView_UI = FindObjectOfType<CR_CellView_UI>();
            CR_Idle_Manager.onGameStateChange += CR_Idle_Manager_onGameStateChange;
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
            _cellView_UI.ClearCellInfos();
            _cellView_UI.UpdateBackground();
            _cellView_UI.ResetCellViewUi();
            _cellViewHolder.SetActive(true);
            canBuy = true;
            _cellManager.ResetCells();
            _cellManager.SpawnCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber);
            switch (_cellSelected)
            {
                case cellType.RedBloodCell:
                    _cellView_UI.SpawnCellInfos(CR_CellBase.CellType.RedBlood);
                    break;
                case cellType.WhiteBloodCell:
                    _cellView_UI.SpawnCellInfos(CR_CellBase.CellType.White);
                    break;
                case cellType.HelperTCell:
                    _cellView_UI.SpawnCellInfos(CR_CellBase.CellType.Helper);
                    break;
            }
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
            StartCoroutine(_cellManager.CheckCells());
            _cellView_UI.UpdateCellNumber();
        }


        #endregion OnClickMethods


    }
}
