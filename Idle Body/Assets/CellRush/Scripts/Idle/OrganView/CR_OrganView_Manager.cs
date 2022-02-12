using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        [SerializeField] TextMeshProUGUI _multiplierUpgradeCost;
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

                //Rest of Awake code
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        private void OnDestroy()
        {
            CR_Idle_Manager.onGameStateChange -= CR_Idle_Manager_onGameStateChange;
        }

        private void CR_Idle_Manager_onGameStateChange(CR_Idle_Manager.GameState obj)
        {
            if (obj != CR_Idle_Manager.GameState.OrganView)
            {
                _organViewHolder.SetActive(false);

                return;
            }
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

            _multiplierText.text = data.organTypes[CurrentOrganType].pointsMultiplier.ToString("F2");
            _multiplierUpgradeCost.text = AbbreviationUtility.AbbreviateBigDoubleNumber(data.organTypes[CurrentOrganType].pointMultiplierCost);
            _platletAmount.text = data.organTypes[CurrentOrganType].plateletInfo.platletNumber.ToString();
            if(data.organTypes[CurrentOrganType].plateletInfo.platletNumber == 125)
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
            StartCoroutine(CR_Idle_Manager.instance.ChangeState(CR_Idle_Manager.GameState.OrganismView));
        }

        public void LoadOrgan(int OrganNumber)
        {

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
            _organID.text = "#" + (OrganNumber +1);
            _redCellAmount.text = data.GetTotalCells(currentOrganType, OrganNumber, 0).ToString();
            _energyPerSecond.text = AbbreviationUtility.AbbreviateBigDoubleNumber(data.GetEnergyPerSecond(currentOrganType, OrganNumber)) + "/s";
            float productionPercentage = (float)((data.GetEnergyPerSecond(currentOrganType, OrganNumber) * 100) / data.GetEnergyPerSecond());
            if (float.IsNaN(productionPercentage)) productionPercentage = 0;
            Debug.Log(productionPercentage);
            _productionPercentage.text = productionPercentage + "%";
            _whiteCellAmount.text = data.GetTotalCells(currentOrganType, OrganNumber, 1).ToString();
            _helperCellAmount.text = data.GetTotalCells(currentOrganType, OrganNumber, 2).ToString();
            _sellComplexityRefund.text = data.organTypes[currentOrganType].ComplexityCost[data.organTypes[currentOrganType].organs.Count].ToString();
            _organImage.sprite = manager.organTypeAsstes[currentOrganType].organSprite;

            LeanTween.alphaCanvas(_infoCanvasGroup, 1, .5f).setOnComplete(done => {
                LeanTween.scale(_infoObject.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
            });
        }

        public void CloseOrganInfo()
        {
            LeanTween.scale(_infoObject.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutExpo).setOnComplete(done => {
                LeanTween.alphaCanvas(_infoCanvasGroup, 0, .5f).setOnComplete(done => {
                    _infoObject.parent.gameObject.SetActive(false);
                }); ;
            });
            
        }


        


    }
}
