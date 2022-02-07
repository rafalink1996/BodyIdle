using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Idle
{
    public class CR_OrganView_Manager : MonoBehaviour
    {
        public static CR_OrganView_Manager instance;
        [SerializeField] GameObject organViewHolder;

        [SerializeField] CR_OrganView_Organ[] organView_Organs;
        [SerializeField] Transform OrganParentObject;

        [Header("UI")]
        [SerializeField] TextMeshProUGUI ShadowText;
        [SerializeField] TextMeshProUGUI MainText;
        [SerializeField] TextMeshProUGUI multiplierText;
        [SerializeField] TextMeshProUGUI multiplierUpgradeCost;
        [SerializeField] TextMeshProUGUI platletAmount;
        [SerializeField] TextMeshProUGUI platletCost;

        private List<CR_OrganView_Organ> currentOrgans = new List<CR_OrganView_Organ>();
        public int DebugOrganType;

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
            if (obj != CR_Idle_Manager.GameState.OrganView)
            {
                organViewHolder.SetActive(false);
                return;
            }
            organViewHolder.SetActive(true);
            SetUI();
            SpawnOrgans();

        }

        void SetUI()
        {
            string OrganName;
            switch (CR_Idle_Manager.instance.currentOrganLoaded)
            {
                case 0:
                    OrganName = "Hearts";
                    break;
                case 1:
                    OrganName = "Stomachs";
                    break;
                case 2:
                    OrganName = "Lungs";
                    break;
                case 3:
                    OrganName = "Gills";
                    break;
                case 4:
                    OrganName = "Kidneys";
                    break;
                case 5:
                    OrganName = "Intestines";
                    break;
                case 6:
                    OrganName = "Livers";
                    break;
                case 7:
                    OrganName = "Pancreas";
                    break;
                case 8:
                    OrganName = "Bladder";
                    break;
                case 9:
                    OrganName = "Brains";
                    break;
                case 10:
                    OrganName = "Posion Glands";
                    break;
                case 11:
                    OrganName = "Bones";
                    break;
                default:
                    OrganName = "Organs";
                    break;
            }
            ShadowText.text = OrganName;
            MainText.text = OrganName;
            UpdateUI();


        }

        private void UpdateUI()
        {
            int CurrentOrganType = CR_Idle_Manager.instance.currentOrganLoaded;
            int OrganAmount = CR_Data.data.organTypes[CurrentOrganType].organs.Count;
            var data = CR_Data.data;

            multiplierText.text = data.organTypes[CurrentOrganType].pointsMultiplier.ToString("F2");
            multiplierUpgradeCost.text = AbbreviationUtility.AbbreviateNumber(data.organTypes[CurrentOrganType].pointMultiplierCost);
            platletAmount.text = data.organTypes[CurrentOrganType].platletNumber.ToString();
            platletCost.text = data.organTypes[CurrentOrganType].plateletCost.ToString();
        }

        void SpawnOrgans()
        {
            int CurrentOrganType = CR_Idle_Manager.instance.currentOrganLoaded;
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
                CR_OrganView_Organ organ = Instantiate(organView_Organs[CurrentOrganType], OrganParentObject);
                organ.SetOrgan(o);
                currentOrgans.Add(organ);
            }

        }

        public void OnClickBackToOrganism()
        {
            StartCoroutine(CR_Idle_Manager.instance.ChangeState(CR_Idle_Manager.GameState.OrganismView));
        }


    }
}
