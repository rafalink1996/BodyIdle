using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        [SerializeField] Image _cellImage;
        [SerializeField] Sprite[] _cellSprites;


        [Header("BACKGROUND ELEMENTS")]
        [SerializeField] Image _backgroundImage;
        [SerializeField] Image _borderImage;


        [Header("VARIABLES")]
        public bool _uiShown = true;
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
                LeanTween.moveLocalY(_UIHolder.gameObject, (_UIStartPos.y - (_UIHolder.rect.height / 1.5f)), .5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(done => { _uiShown = false; });

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

            LeanTween.scale(_buyButton.gameObject, _buyButton.transform.localScale * 1.2f, 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
            {
                if (_buyButton.TryGetComponent(out Image image)) image.color = UIcolor;
                _cellImage.sprite = _cellSprites[(int)type];
                UpdateCellCost();
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


        public void UpdateCellCost()
        {
            if (CR_Data.data.GetTotalCells(CR_Idle_Manager.instance.CurrentOrganType, CR_Idle_Manager.instance.CurrentOrganNumber, (int)CR_CellViewManager.instance._cellSelected) >= 1000) 
            {
                _cellCostText.text = "MAX";
            }
            else
            {
                var cost = CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].organs[CR_Idle_Manager.instance.CurrentOrganNumber].CellTypes[(int)CR_CellViewManager.instance._cellSelected].currentCellCost;
                _cellCostText.text = AbbreviationUtility.AbbreviateBigDoubleNumber(cost);
            }
           
        }
    }
}
