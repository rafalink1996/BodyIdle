using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BreakInfinity;

namespace Idle
{
    public class CR_CellView_UI : MonoBehaviour
    {
        [System.Serializable]
        public class CellInfoUI
        {
            public TextMeshProUGUI countText;
            public List<CR_CellView_CellInfo> _smallCellInfoList;
            public List<CR_CellView_CellInfo> _medCellInfoList;
            public List<CR_CellView_CellInfo> _bigCellInfoList;
        }

        [Header("GENERAL ELEMENTS")]
        [SerializeField] Transform _StickyImage;
        [SerializeField] RectTransform _UIHolder;
        [SerializeField] Transform _toggleArrow;

        [Header("CELL INFO ELEMENTS")]
        [SerializeField] CanvasGroup _cellInfoObject;
        [SerializeField] Transform _cellInfoHolder;
        [SerializeField] CellInfoUI RedBloodInfos;
        [SerializeField] CellInfoUI whiteBloodInfos;
        [SerializeField] CellInfoUI HelperInfos;

        [Header("BUY BUTTON ELEMENTS")]
        [SerializeField] Button _buyButton;
        [SerializeField] TextMeshProUGUI _cellCostText;
        //[SerializeField] Image _cellImage;
        [SerializeField] Sprite[] _cellSprites;

        [Header("BUY AMOUNT ELEMENTS")]
        [SerializeField] TextMeshProUGUI _buyAmountText;
        [SerializeField] Image _textBGImage;
        [SerializeField] Image _rightButton;
        [SerializeField] Image _leftButton;


        [Header("SELECT CELL TYPE ELEMENTS")]
        [SerializeField] RectTransform _selectSizeChildObject;
        [SerializeField] RectTransform _selectSizeHolder;
        [SerializeField] Button _redBloodButton;
        [SerializeField] Button _whiteBloodButton;
        [SerializeField] Button _helperTButton;

        [Header("PATHOGEN ELEMENTS")]
        [SerializeField] TextMeshProUGUI _pathogenAmountText;
        [SerializeField] Image _pathogenMainBody;
        [SerializeField] Image _pathogenDecors;
        [SerializeField] Image _pathogenEyes;
 
        [Header("BACKGROUND ELEMENTS")]
        [SerializeField] Image _backgroundImage;
        [SerializeField] Image _borderImage;


        [Header("VARIABLES")]
        public bool _uiShown = true;
        public bool _selectCellUiShown;
        Vector2 _UIStartPos;



        private void Awake()
        {
            _UIStartPos = _UIHolder.localPosition;
        }

        public void ResetCellViewUi()
        {
        }

        public void ToggleUI()
        {
            if (_uiShown)
            {
                LeanTween.scaleY(_StickyImage.gameObject, .2f, 0.3f).setEase(LeanTweenType.easeOutElastic);
                LeanTween.cancel(_toggleArrow.gameObject);
                LeanTween.rotateZ(_toggleArrow.gameObject, 180, 1).setEase(LeanTweenType.easeOutExpo);
                LeanTween.moveLocalY(_UIHolder.gameObject, (_UIStartPos.y - (_UIHolder.rect.height * 0.75f)), .5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(done => { _uiShown = false; });

            }
            else
            {
                LeanTween.scaleY(_StickyImage.gameObject, 1, 0.3f).setEase(LeanTweenType.easeOutElastic);
                LeanTween.cancel(_toggleArrow.gameObject);
                LeanTween.rotateZ(_toggleArrow.gameObject, 0, 1).setEase(LeanTweenType.easeOutExpo);
                LeanTween.moveLocalY(_UIHolder.gameObject, _UIStartPos.y, .5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(donde => { _uiShown = true; });
            }
        }

        public void UpdateBackground()
        {
            CR_Idle_Manager.OrganTypeAsstes assets = CR_Idle_Manager.instance.organTypeAsstes[CR_Idle_Manager.instance.CurrentOrganType];
            if (assets == null)
            {
                Debug.Log("Assets is null");
                return;
            }
            if (assets.OrganBackgroundColor != Color.clear) _backgroundImage.color = assets.OrganBackgroundColor;
            if (assets.OrganBorder != null) _borderImage.sprite = assets.OrganBorder;

        }

        public void ChangeCellTypeUI(CR_CellViewManager.cellType type)
        {
            CR_CellViewManager.instance.canBuy = false;
            Color UIcolor = new Color(1, .48f, .48f, 1);
            switch (type)
            {
                case CR_CellViewManager.cellType.RedBloodCell:
                    UIcolor = new Color(1, .48f, .48f, 1);
                    break;
                case CR_CellViewManager.cellType.WhiteBloodCell:
                    UIcolor = new Color(.82f, .82f, .82f, 1);
                    break;
                case CR_CellViewManager.cellType.HelperTCell:
                    UIcolor = new Color(.42f, .97f, 1, 1);
                    break;
                default:
                    break;
            }

            _textBGImage.color = UIcolor;
            _rightButton.color = UIcolor;
            _leftButton.color = UIcolor;
            LeanTween.scale(_buyButton.gameObject, _buyButton.transform.localScale * 1.2f, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
            {
                if (_buyButton.TryGetComponent(out Image image)) image.color = UIcolor;
                //_cellImage.sprite = _cellSprites[(int)type];
                UpdateCellCost(CR_CellViewManager.instance.buyAmount);
                LeanTween.scale(_buyButton.gameObject, Vector2.one, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
                {
                    CR_CellViewManager.instance.canBuy = true;
                });
            });

        }

        public void ToggleCellInfoObject(bool show)
        {
            LeanTween.cancel(_cellInfoHolder.gameObject);
            LeanTween.cancel(_cellInfoObject.gameObject);
            if (show)
            {
                _cellInfoObject.gameObject.SetActive(true);
                _cellInfoObject.alpha = 0;
                _cellInfoHolder.localScale = Vector3.zero;
                LeanTween.alphaCanvas(_cellInfoObject, 1, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(done =>
                {
                    LeanTween.scale(_cellInfoHolder.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeInExpo);
                });
            }
            else
            {

                LeanTween.scale(_cellInfoHolder.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
                {
                    LeanTween.alphaCanvas(_cellInfoObject, 0, 0.5f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(done =>
                    {
                        _cellInfoObject.gameObject.SetActive(false);
                    });
                });
            }
        }

        public void UpdateCellInfos()
        {
            CR_Data data = CR_Data.data;
            var Organ = data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].organs[CR_Idle_Manager.instance.CurrentOrganNumber];

            var redInfos = Organ.CellTypes[0].cellSizes;
            var whiteInfos = Organ.CellTypes[1].cellSizes;
            var HelperInfos = Organ.CellTypes[2].cellSizes;

            RedBloodInfos.countText.text = CR_Data.data.GetTotalCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber, 0).ToString();
            whiteBloodInfos.countText.text = CR_Data.data.GetTotalCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber, 1).ToString();
            this.HelperInfos.countText.text = CR_Data.data.GetTotalCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber, 2).ToString();

            void UpdateRedBloodInfos()
            {
                for (int i = 0; i < RedBloodInfos._smallCellInfoList.Count; i++)
                {
                    if (i >= redInfos[0].CellsInfos.Count)
                    {
                        RedBloodInfos._smallCellInfoList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        RedBloodInfos._smallCellInfoList[i].gameObject.SetActive(true);
                        RedBloodInfos._smallCellInfoList[i].ActivateCellInfo(redInfos[0].CellsInfos[i], CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Small);
                    }
                }

                for (int i = 0; i < RedBloodInfos._medCellInfoList.Count; i++)
                {
                    if (i >= redInfos[1].CellsInfos.Count)
                    {
                        RedBloodInfos._medCellInfoList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        RedBloodInfos._medCellInfoList[i].gameObject.SetActive(true);
                        RedBloodInfos._medCellInfoList[i].ActivateCellInfo(redInfos[1].CellsInfos[i], CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Medium);
                    }
                }
                for (int i = 0; i < RedBloodInfos._bigCellInfoList.Count; i++)
                {
                    if (i >= redInfos[2].CellsInfos.Count)
                    {
                        RedBloodInfos._bigCellInfoList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        RedBloodInfos._bigCellInfoList[i].gameObject.SetActive(true);
                        RedBloodInfos._bigCellInfoList[i].ActivateCellInfo(redInfos[2].CellsInfos[i], CR_CellBase.CellType.RedBlood, CR_CellBase.CellSize.Big);
                    }
                }
            }
            UpdateRedBloodInfos();

            void UpdateWhiteBloodInfos()
            {
                for (int i = 0; i < whiteBloodInfos._smallCellInfoList.Count; i++)
                {
                    if (i >= whiteInfos[0].CellsInfos.Count)
                    {
                        whiteBloodInfos._smallCellInfoList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        whiteBloodInfos._smallCellInfoList[i].gameObject.SetActive(true);
                        whiteBloodInfos._smallCellInfoList[i].ActivateCellInfo(whiteInfos[0].CellsInfos[i], CR_CellBase.CellType.White, CR_CellBase.CellSize.Small);
                    }
                }

                for (int i = 0; i < whiteBloodInfos._medCellInfoList.Count; i++)
                {
                    if (i >= whiteInfos[1].CellsInfos.Count)
                    {
                        whiteBloodInfos._medCellInfoList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        whiteBloodInfos._medCellInfoList[i].gameObject.SetActive(true);
                        whiteBloodInfos._medCellInfoList[i].ActivateCellInfo(whiteInfos[1].CellsInfos[i], CR_CellBase.CellType.White, CR_CellBase.CellSize.Medium);
                    }
                }
                for (int i = 0; i < whiteBloodInfos._bigCellInfoList.Count; i++)
                {
                    if (i >= whiteInfos[2].CellsInfos.Count)
                    {
                        whiteBloodInfos._bigCellInfoList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        whiteBloodInfos._bigCellInfoList[i].gameObject.SetActive(true);
                        whiteBloodInfos._bigCellInfoList[i].ActivateCellInfo(whiteInfos[2].CellsInfos[i], CR_CellBase.CellType.White, CR_CellBase.CellSize.Big);
                    }
                }
            }
            UpdateWhiteBloodInfos();

            void UpdateHelperBloodInfos()
            {
                for (int i = 0; i < this.HelperInfos._smallCellInfoList.Count; i++)
                {
                    if (i >= HelperInfos[0].CellsInfos.Count)
                    {
                        this.HelperInfos._smallCellInfoList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        this.HelperInfos._smallCellInfoList[i].gameObject.SetActive(true);
                        this.HelperInfos._smallCellInfoList[i].ActivateCellInfo(HelperInfos[0].CellsInfos[i], CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Small);
                    }
                }

                for (int i = 0; i < this.HelperInfos._medCellInfoList.Count; i++)
                {
                    if (i >= HelperInfos[1].CellsInfos.Count)
                    {
                        this.HelperInfos._medCellInfoList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        this.HelperInfos._medCellInfoList[i].gameObject.SetActive(true);
                        this.HelperInfos._medCellInfoList[i].ActivateCellInfo(HelperInfos[1].CellsInfos[i], CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Medium);
                    }
                }
                for (int i = 0; i < this.HelperInfos._bigCellInfoList.Count; i++)
                {
                    if (i >= HelperInfos[2].CellsInfos.Count)
                    {
                        this.HelperInfos._bigCellInfoList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        this.HelperInfos._bigCellInfoList[i].gameObject.SetActive(true);
                        this.HelperInfos._bigCellInfoList[i].ActivateCellInfo(HelperInfos[2].CellsInfos[i], CR_CellBase.CellType.Helper, CR_CellBase.CellSize.Big);
                    }
                }
            }
            UpdateHelperBloodInfos();

            ToggleCellInfoObject(true);
        }


        public void UpdateCellCost(CR_CellViewManager.BuyAmount buyAmount)
        {
            var manager = CR_CellViewManager.instance;
            var cellSelected = (int)manager._cellSelected;
            int maxCellAmount = 1000;
            switch (cellSelected)
            {
                case 0:
                case 1:
                default:
                    maxCellAmount = 1000;
                    break;
                case 2:
                    maxCellAmount = 27;
                    break;
            }

            if (CR_Data.data.GetTotalCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber, cellSelected) >=  maxCellAmount)
            {
                _cellCostText.text = "MAX";
            }
            else
            {
                BigDouble cost = 0;
                var currentCellCost = CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].organs[CR_Idle_Manager.instance.CurrentOrganNumber].CellTypes[(int)CR_CellViewManager.instance._cellSelected].currentCellCost;
                var baseCost = CR_Data.data.organTypes[manager._organType].organs[manager._organNumber].CellTypes[cellSelected].initialCellCost;
                var cellNumber = CR_Data.data.GetTotalCells(manager._organType, manager._organNumber, cellSelected);
                switch (buyAmount)
                {
                    case CR_CellViewManager.BuyAmount.x1:
                        cost = currentCellCost;
                        break;
                    case CR_CellViewManager.BuyAmount.x5:

                        for (int i = 0; i < 5; i++)
                        {
                            cost += baseCost * Mathf.Pow(manager.CachedGrowthRate, cellNumber + i);
                        }

                        break;
                    case CR_CellViewManager.BuyAmount.x10:
                        for (int i = 0; i < 10; i++)
                        {
                            cost += baseCost * Mathf.Pow(manager.CachedGrowthRate, cellNumber + i);
                        }
                        break;
                    case CR_CellViewManager.BuyAmount.max:
                        var amount = manager.buyMaxAmount;
                        if (amount <= 0)
                        {
                            amount = 1;
                        }
                        for (int i = 0; i < amount; i++)
                        {
                            cost += baseCost * Mathf.Pow(manager.CachedGrowthRate, cellNumber + i);
                        }
                        break;
                    default:
                        break;
                }
                _cellCostText.text = AbbreviationUtility.AbbreviateBigDoubleNumber(cost);
            }
        }

        public void UpdateBuyAmount(CR_CellViewManager.BuyAmount buyAmount)
        {
            switch (buyAmount)
            {
                case CR_CellViewManager.BuyAmount.x1:
                    _buyAmountText.text = "X1";
                    break;
                case CR_CellViewManager.BuyAmount.x5:
                    _buyAmountText.text = "X5";
                    break;
                case CR_CellViewManager.BuyAmount.x10:
                    _buyAmountText.text = "X10";
                    break;
                case CR_CellViewManager.BuyAmount.max:
                    var Amount = CR_CellViewManager.instance.buyMaxAmount;
                    _buyAmountText.text = "MAX " + "(" + Amount + ")";
                    break;
                default:
                    break;
            }
        }

        public void ShowCellTypesOptions()
        {
            LeanTween.size(_selectSizeHolder, new Vector2(0, _selectSizeChildObject.rect.height * 3), 0.3f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>{ _selectCellUiShown = true; });
            
        }

        public void CloseCellTypesOptions(int ButtonSelected)
        {
            switch (ButtonSelected)
            {
                case 0:
                    _redBloodButton.transform.SetSiblingIndex(0);
                    break;
                case 1:
                    _whiteBloodButton.transform.SetSiblingIndex(0);
                    break;
                case 2:
                    _helperTButton.transform.SetSiblingIndex(0);
                    break;
                default:
                    break;
            }
            LeanTween.size(_selectSizeHolder, Vector2.zero, 0.3f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done => { _selectCellUiShown = false; });
        }



        public void SetPathogenUI(CR_PathogenSystem.Infection infection, int amount)
        {
            _pathogenAmountText.text = amount.ToString();
            _pathogenMainBody.sprite = CR_PathogenSystem.instance.infectionAssets.infectionTypeAssets[(int)infection.infectionType].infectionBodies[infection.infectionBodyID];
            _pathogenDecors.sprite = CR_PathogenSystem.instance.infectionAssets.infectionTypeAssets[(int)infection.infectionType].infectionDecorations[infection.infectionDecorationsID];
           _pathogenEyes.sprite = CR_PathogenSystem.instance.infectionAssets.infectionTypeAssets[(int)infection.infectionType].infectionEyes[infection.infectionEyesID];
        }
    }
}
