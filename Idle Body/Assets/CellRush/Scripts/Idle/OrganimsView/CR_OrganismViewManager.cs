using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BreakInfinity;

namespace Idle
{
    public class CR_OrganismViewManager : MonoBehaviour
    {

        public static CR_OrganismViewManager instance;

        [Header("REFERENCES")]
        [SerializeField] GameObject OrganismHolder;
        [SerializeField] CR_OrganismView_Anim _organismView_Anim;
        [SerializeField] CR_OrganismView_Texts _organismView_Texts;
        [SerializeField] Transform _organHolder;

        [SerializeField] int organSelected = -1;

        [Header("BUY UI")]
        [SerializeField] Image OrganImage;
        [SerializeField] TextMeshProUGUI organTitleText;
        [SerializeField] TextMeshProUGUI energyCostText;
        [SerializeField] TextMeshProUGUI complexityCostText;

        [System.Serializable]
        struct organObject
        {
            public string name;
            public CR_OrganismView_Organ MainObject;
            public TextMeshProUGUI CounterText;
            public CR_EyeAnimator Eyes;
            public CR_EyeAnimator Eyes_2;
        }
        [SerializeField] List<organObject> organObjects;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
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
            if(obj != CR_Idle_Manager.GameState.OrganismView)
            {
                OrganismHolder.SetActive(false);
                return;
            }
           
            OrganismHolder.SetActive(true);
            _organismView_Anim.GetPos();
            _organismView_Anim.HideUI();

            UpdateOrganVisuals();
            _organismView_Texts.UpdateTexts();
        }



        public void SelectOrgan(int organType)
        {
           
            if (!CR_Data.data.organTypes[organSelected].unlocked)
            {
                organObjects[organSelected].MainObject.ChangeColor(new Color(0.3f, 0.3f, 0.3f, 1));
            }
            organSelected = organType;
            organObjects[organSelected].MainObject.ChangeColor(Color.white);
            UpdateOrganInfo();
            _organismView_Anim.ShowUI();
        }
        void UpdateOrganVisuals()
        {
            var data = CR_Data.data;
            for (int i = 0; i < data.organTypes.Length; i++)
            {
                if (data.organTypes[i].unlocked)
                {
                    organObjects[i].MainObject.ChangeColor(Color.white);
                    organObjects[i].Eyes.gameObject.SetActive(true);
                    if (organObjects[i].Eyes_2 != null) { organObjects[i].Eyes_2.gameObject.SetActive(true); }
                    organObjects[i].CounterText.transform.parent.gameObject.SetActive(true);
                    organObjects[i].CounterText.text = "x" + data.organTypes[i].organs.Count.ToString();
                }
                else
                {
                    organObjects[i].MainObject.ChangeColor(new Color(0.3f,0.3f,0.3f,1));
                    organObjects[i].Eyes.gameObject.SetActive(false);
                    if (organObjects[i].Eyes_2 != null) { organObjects[i].Eyes_2.gameObject.SetActive(false); }
                    organObjects[i].CounterText.transform.parent.gameObject.SetActive(false);
                }
            }
        }


        void UpdateOrganInfo()
        {
            var data = CR_Data.data;
            OrganImage.sprite = CR_Idle_Manager.instance.organTypeAsstes[organSelected].organSprite;
            organTitleText.text = LanguageManager.instance.organismViewTexts.Buy[(int)CR_Data.data._language] + " " + LanguageManager.instance.translateOrgan(organSelected, CR_Data.data._language, true);
            int OrganNumber = data.organTypes[organSelected].organs.Count;
            if(data.organTypes[organSelected].PointCost.Length > OrganNumber)
            {
                energyCostText.text = AbbreviationUtility.AbbreviateBigDoubleNumber(data.organTypes[organSelected].PointCost[OrganNumber]);
            }
            else
            {
                energyCostText.text = "MAX";
            }
            if(data.organTypes[organSelected].ComplexityCost.Length > OrganNumber)
            {
                complexityCostText.text = AbbreviationUtility.AbbreviateNumber(data.organTypes[organSelected].ComplexityCost[OrganNumber]);
            }
            else
            {
                complexityCostText.text = "MAX";
            }
            if (CR_Data.data.organTypes[organSelected].unlocked)
            {
                _organismView_Anim.toggleSeeOrganObjects(true);
            }
            else
            {
                _organismView_Anim.toggleSeeOrganObjects(false);
            }
        }

        public void OnClickBuyOrgan()
        {
            var data = CR_Data.data;
            if(data.organTypes[organSelected].organs.Count < 6)
            {
                int OrganNumber = data.organTypes[organSelected].organs.Count;
                BigDouble energyCost = data.organTypes[organSelected].PointCost[OrganNumber];
                int ComplexityCost = data.organTypes[organSelected].ComplexityCost[OrganNumber];
                if (data._energy >= energyCost)
                {
                    if (data._complexity + ComplexityCost <= data._maxComplexity)
                    {
                        data.SetEnergy(data._energy - energyCost);
                        data.SetComplexity(data._complexity + ComplexityCost);
                        CR_Data.data.AddNewOrgan(organSelected);
                        CR_Data.data.CalculateMultiplier(organSelected);
                        organObjects[organSelected].Eyes.gameObject.SetActive(true);
                        if (organObjects[organSelected].Eyes_2 != null) { organObjects[organSelected].Eyes_2.gameObject.SetActive(true); }
                        organObjects[organSelected].CounterText.transform.parent.gameObject.SetActive(true);
                        UpdateOrganInfo();

                        var OrganCounterObj = organObjects[organSelected].CounterText.transform.parent.gameObject;
                        LeanTween.cancel(OrganCounterObj);
                        LeanTween.scale(OrganCounterObj, new Vector3(1.3f, 1.3f, 1.3f), 0.5f).setEase(LeanTweenType.easeInExpo).setOnComplete(done => {
                            UpdateOrganVisuals();
                            LeanTween.scale(OrganCounterObj, Vector2.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
                        });
                    }
                    else
                    {
                        print("Not enough complexity");
                        // not enough complexity
                    }

                }
                else
                    print("Not enough energy" + "- Energy Cost: " + energyCost + " Current Energy: " + data._energy);
                {
                    // not enough points
                }
            }
        }

        public void OnClickSeeOrgans()
        {
            if(CR_Data.data.organTypes[organSelected].organs.Count != 0)
            {
                CR_Idle_Manager.instance.CurrentOrganType = organSelected;
                StartCoroutine(CR_Idle_Manager.instance.ChangeState(CR_Idle_Manager.GameState.OrganView));
            }
            else
            {
                // Organ not bought
            }
           
        }
    }
}
