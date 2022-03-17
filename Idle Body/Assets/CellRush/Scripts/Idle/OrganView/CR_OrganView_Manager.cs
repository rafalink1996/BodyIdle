using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using BreakInfinity;

namespace Idle
{
    public class CR_OrganView_Manager : MonoBehaviour
    {
        public static CR_OrganView_Manager instance;
        [SerializeField] GameObject _organViewHolder;

        [SerializeField] CR_OrganView_Organ[] _organView_Organs;
        [SerializeField] CR_OrganView_Texts _organView_Texts;
        [SerializeField] Transform _0rganParentObject;

        [Header("UI")]
        [SerializeField] TextMeshProUGUI _shadowText;
        [SerializeField] TextMeshProUGUI _mainText;
        [SerializeField] TextMeshProUGUI _multiplierText;
        [SerializeField] TextMeshProUGUI _platletAmount;
        [SerializeField] TextMeshProUGUI _platletCost;


        [Header("ORGAN INFO UI")]
        [SerializeField] CanvasGroup _infoCanvasGroup;
        [SerializeField] Transform _infoObject;
        [SerializeField] TextMeshProUGUI _organName;
        [SerializeField] TextMeshProUGUI _organID;
        [SerializeField] TextMeshProUGUI _redCellAmount;
        [SerializeField] TextMeshProUGUI _energyPerSecond;
        [SerializeField] TextMeshProUGUI _productionPercentage;
        [SerializeField] TextMeshProUGUI _whiteCellAmount;
        [SerializeField] TextMeshProUGUI _helperCellAmount;
        [SerializeField] TextMeshProUGUI _sellEnergyRefund;
        [SerializeField] TextMeshProUGUI _sellComplexityRefund;
        [SerializeField] Image _organImage;

        [Header("ORGAN UPGRADES")]
        [SerializeField] CanvasGroup _upgradeObject;
        [SerializeField] Transform _upgradesTitleObject;
        [SerializeField] RectTransform _upgradesBodyObject;
        [SerializeField] Transform _upgradesCloseObject;
        float _upgradesBodyObjectOriginalY;
        bool _upgradesUiShown;
        [SerializeField] public CR_OrganUpgrade[] _upgrades;

        [SerializeField] Transform _upgradesInfoObject;
        [SerializeField] TextMeshProUGUI _upgradeDescription;
        [SerializeField] TextMeshProUGUI _upgradeCost;
        [SerializeField] ButtonHold _upgradeButton;
        [SerializeField] CR_OrganUpgrade _infoUpgrade;




        [Header("REFERENCES")]
        [SerializeField] CR_OrganView_PlatletsManager _platletManager;

        private List<CR_OrganView_Organ> currentOrgans = new List<CR_OrganView_Organ>();
        public int DebugOrganType;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                _platletManager = FindObjectOfType<CR_OrganView_PlatletsManager>();
                CR_Idle_Manager.onGameStateChange += CR_Idle_Manager_onGameStateChange;
                CR_Data.onLanguageChange += CR_Data_onLanguageChange;

                //Rest of Awake code
                _upgradesBodyObjectOriginalY = _upgradesBodyObject.sizeDelta.y;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }



        private void OnDestroy()
        {
            CR_Data.onLanguageChange -= CR_Data_onLanguageChange;
            CR_Idle_Manager.onGameStateChange -= CR_Idle_Manager_onGameStateChange;
        }

        private void CR_Data_onLanguageChange()
        {
            UpdateUI();
            SetUI();
            _organView_Texts.UpdateTexts();
        }
        private void CR_Idle_Manager_onGameStateChange(CR_Idle_Manager.GameState obj)
        {
            if (obj != CR_Idle_Manager.GameState.OrganView)
            {
                _organViewHolder.SetActive(false);

                return;
            }

            SetOrganUpgrades();
            _upgradeObject.gameObject.SetActive(false);
            _platletManager.canBuy = true;
            _platletManager.ClearPlatlets(true);
            if (_platletManager.mergeObject != null) Destroy(_platletManager.mergeObject.gameObject);
            _organView_Texts.UpdateTexts();
            _infoCanvasGroup.gameObject.SetActive(false);
            _organViewHolder.SetActive(true);
            SetUI();
            SpawnOrgans();
            SetupPlatlets();



        }

        void SetUI()
        {
            string OrganName;
            OrganName = LanguageManager.instance.translateOrgan(CR_Idle_Manager.instance.CurrentOrganType, CR_Data.data._language, true);
            _shadowText.text = OrganName;
            _mainText.text = OrganName;
            UpdateUI();
        }

        public void UpdateUI()
        {
            int CurrentOrganType = CR_Idle_Manager.instance.CurrentOrganType;
            int OrganAmount = CR_Data.data.organTypes[CurrentOrganType].organs.Count;
            var data = CR_Data.data;

            _multiplierText.text = AbbreviationUtility.AbbreviateBigDoubleNumber(data.organTypes[CurrentOrganType].pointsMultiplier);
            _platletAmount.text = data.organTypes[CurrentOrganType].plateletInfo.platletNumber.ToString();
            if (data.organTypes[CurrentOrganType].plateletInfo.platletNumber == 125)
            {
                _platletCost.text = "MAX";
            }
            else
            {
                _platletCost.text = AbbreviationUtility.AbbreviateBigDoubleNumber(data.organTypes[CurrentOrganType].plateletInfo.plateletCost);
            }

        }

        void SpawnOrgans()
        {
            int CurrentOrganType = CR_Idle_Manager.instance.CurrentOrganType;
            if (currentOrgans.Count != 0)
            {
                for (int i = 0; i < currentOrgans.Count; i++)
                {
                    Destroy(currentOrgans[i].gameObject);
                }
            }
            currentOrgans.Clear();
            for (int o = 0; o < CR_Data.data.organTypes[CurrentOrganType].organs.Count; o++)
            {
                CR_OrganView_Organ organ = Instantiate(_organView_Organs[CurrentOrganType], _0rganParentObject);
                organ.SetOrgan(o, this);
                currentOrgans.Add(organ);
            }
        }



        void SetupPlatlets()
        {
            _platletManager.InitializeOrgan();
        }

        public void OnClickBackToOrganism()
        {
            _platletManager.StopAllCoroutines();
            StartCoroutine(CR_Idle_Manager.instance.ChangeState(CR_Idle_Manager.GameState.OrganismView));
        }

        public void LoadOrgan(int OrganNumber)
        {
            _platletManager.StopAllCoroutines();
            Debug.Log("change to cell view");
            CR_Idle_Manager.instance.CurrentOrganNumber = OrganNumber;
            StartCoroutine(CR_Idle_Manager.instance.ChangeState(CR_Idle_Manager.GameState.CellView));
        }

        public void ShowOrganInfo(int OrganNumber)
        {
            _infoObject.parent.gameObject.SetActive(true);
            _infoCanvasGroup.alpha = 0;
            _infoObject.localScale = Vector3.zero;
            CR_Idle_Manager manager = CR_Idle_Manager.instance;
            int currentOrganType = manager.CurrentOrganType;
            CR_Data data = CR_Data.data;

            _organName.text = LanguageManager.instance.translateOrgan(currentOrganType, data._language, false);
            _organID.text = "#" + (OrganNumber + 1);
            _redCellAmount.text = data.GetTotalCells(currentOrganType, OrganNumber, 0).ToString();
            _energyPerSecond.text = AbbreviationUtility.AbbreviateBigDoubleNumber(data.GetEnergyPerSecond(currentOrganType, OrganNumber)) + "/s";
            float productionPercentage = (float)((data.GetEnergyPerSecond(currentOrganType, OrganNumber) * 100) / data.GetEnergyPerSecond());
            if (float.IsNaN(productionPercentage)) productionPercentage = 0;
            //Debug.Log(productionPercentage);
            productionPercentage = Mathf.FloorToInt(productionPercentage);
            _productionPercentage.text = productionPercentage + "%";
            _whiteCellAmount.text = data.GetTotalCells(currentOrganType, OrganNumber, 1).ToString();
            _helperCellAmount.text = data.GetTotalCells(currentOrganType, OrganNumber, 2).ToString();
            _sellComplexityRefund.text = data.organTypes[currentOrganType].ComplexityCost[data.organTypes[currentOrganType].organs.Count].ToString();
            _organImage.sprite = manager.organTypeAsstes[currentOrganType].organSprite;

            LeanTween.alphaCanvas(_infoCanvasGroup, 1, .2f).setOnComplete(done =>
            {
                LeanTween.scale(_infoObject.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutExpo);
            });
        }

        public void CloseOrganInfo()
        {
            LeanTween.scale(_infoObject.gameObject, Vector3.zero, 0.3f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
            {
                LeanTween.alphaCanvas(_infoCanvasGroup, 0, .2f).setOnComplete(done =>
                {
                    _infoObject.parent.gameObject.SetActive(false);
                }); ;
            });
        }

        void SetOrganUpgrades()
        {
            _upgradesInfoObject.gameObject.SetActive(false);
            var upgrades = CR_Idle_Manager.instance.organTypeAsstes[CR_Idle_Manager.instance.CurrentOrganType].upgrades;
            for (int i = 0; i < _upgrades.Length; i++)
            {
                if (i >= upgrades.Count)
                {
                    _upgrades[i].HideUpgrade();
                }
                else
                {
                    _upgrades[i].SetUpgrade(upgrades[i].type, i, upgrades[i].amount);
                }

            }
        }


        public void HideUpgradeUI()
        {
            LeanTween.scale(_upgradesInfoObject.gameObject, Vector3.zero, 0.25f).setEase(LeanTweenType.easeInExpo);
            LeanTween.scale(_upgradesCloseObject.gameObject, Vector3.zero, 0.25f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
            {
                LeanTween.scale(_upgradesBodyObject, Vector3.zero, 0.25f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
                {

                    LeanTween.scale(_upgradesTitleObject.gameObject, Vector3.zero, 0.25f).setEase(LeanTweenType.easeInExpo).setOnComplete(done =>
                    {
                        LeanTween.alphaCanvas(_upgradeObject, 0, 0.5f).setOnComplete(done =>
                        {
                            _upgradesUiShown = false;
                            _upgradeObject.gameObject.SetActive(false);
                        });
                    });
                });
            });
        }
        public void ShowUpgradeUI()
        {
            _upgradeObject.gameObject.SetActive(true);
            _upgradeObject.alpha = 0;
            _upgradesTitleObject.localScale = Vector3.zero;
            _upgradesCloseObject.localScale = Vector3.zero;
            _upgradesBodyObject.transform.localScale = Vector3.zero;
            LeanTween.alphaCanvas(_upgradeObject, 1, 0.25f).setOnComplete(done =>
            {
                LeanTween.scale(_upgradesTitleObject.gameObject, Vector3.one, 0.25f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
                {
                    LeanTween.scale(_upgradesBodyObject, Vector3.one, 0.25f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
                    {

                        LeanTween.scale(_upgradesCloseObject.gameObject, Vector3.one, 0.25f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done =>
                        {
                            _upgradesUiShown = true;
                        });
                    });
                });
            });
        }

        public void DisplayUpgradeInfo(int index, CR_Idle_Manager.OrganUpgrade.UpgradeType type)
        {
            var currentOrganType = CR_Idle_Manager.instance.CurrentOrganType;
            var upgrade = CR_Idle_Manager.instance.organTypeAsstes[currentOrganType].upgrades[index];
            _upgradeDescription.text = upgrade.description[(int)CR_Data.data._language];
            _upgradeButton.OnLongClick.RemoveAllListeners();
            _upgradeButton.OnLongClick.AddListener(delegate { BuyUpgrade(index); });
            _infoUpgrade.SetUpgrade(type, index, upgrade.amount);

            UpdateBuyButtonAndCost(index);

            _upgradesInfoObject.transform.localScale = Vector3.zero;
            _upgradesInfoObject.gameObject.SetActive(true);
            LeanTween.scale(_upgradesInfoObject.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
        }

        void UpdateBuyButtonAndCost(int index)
        {
            var currentOrganType = CR_Idle_Manager.instance.CurrentOrganType;
            var upgrade = CR_Idle_Manager.instance.organTypeAsstes[currentOrganType].upgrades[index];
            if (index < CR_Data.data.organTypes[currentOrganType].upgrades.Length)
            {
                if (CR_Data.data.organTypes[currentOrganType].upgrades[index])
                {
                    _upgradeCost.text = LanguageManager.instance.organViewTexts.Owned[(int)CR_Data.data._language];
                    _upgradeButton.gameObject.SetActive(false);
                }
                else
                {
                    _upgradeCost.text = AbbreviationUtility.AbbreviateBigDoubleNumber(upgrade.UpgradeCost);
                    if (index != 0)
                    {
                        if (CR_Data.data.organTypes[currentOrganType].upgrades[index - 1])
                        {
                            _upgradeButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            _upgradeButton.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        _upgradeButton.gameObject.SetActive(true);
                    }
                }
            }
        }

        void BuyUpgrade(int index)
        {
            var currentOrganType = CR_Idle_Manager.instance.CurrentOrganType;
            var upgrade = CR_Idle_Manager.instance.organTypeAsstes[currentOrganType].upgrades[index];
            if (CR_Data.data.organTypes[currentOrganType].upgrades[index]) return;
            if (upgrade.UpgradeCost > CR_Data.data._energy) return;
            if (index != 0)
            {
                if (!CR_Data.data.organTypes[currentOrganType].upgrades[index - 1]) return;
            }

            CR_Data.data.SetEnergy(CR_Data.data._energy - upgrade.UpgradeCost);
            CR_Data.data.organTypes[currentOrganType].upgrades[index] = true;
            CR_Data.data.CalculateMultiplier(CR_Idle_Manager.instance.CurrentOrganType);
            _upgrades[index].UpdateUpgrade();
            if (_upgrades[index + 1] == null) return;
            _upgrades[index + 1].UpdateUpgrade();
            UpdateBuyButtonAndCost(index);
            UpdateUI();
        }
    }
}





//var cost = CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].pointMultiplierCost;
//if (CR_Data.data._energy < cost)
//{
//    Debug.Log("not enough energy - " + CR_Data.data._energy + "/" + cost);
//    return;
//}
//if (CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].multiplierLevel >= 5) return;

//CR_Data.data.SetEnergy(CR_Data.data._energy - cost);
//CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].pointsMultiplier *= 2;
//CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].multiplierLevel++;
//CR_Data.data.GetEnergyPerSecond();
//BigDouble newCost = CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].pointsMultiplierInitialCost * Mathf.Pow(2.3f, (CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].multiplierLevel * (CR_Idle_Manager.instance.CurrentOrganType +1)));
//CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].pointMultiplierCost = newCost;
//UpdateUI();