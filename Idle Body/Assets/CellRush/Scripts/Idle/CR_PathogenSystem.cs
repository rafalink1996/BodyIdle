using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idle
{
    public class CR_PathogenSystem : MonoBehaviour
    {
        public static CR_PathogenSystem instance;


        public enum InfectionType { Bacterial, Viral, Fungal, InfectionNum };
        public class Infection
        {
            [Header("ID")]
            public InfectionType infectionType;
            public int infectionBodyID;
            public int infectionEyesID;
            public int infectionDecorationsID;

            [Header("STATS")]
            public int health;
            public int damage;
        }

        [System.Serializable]
        public class InfectionAssets
        {
            [System.Serializable]
            public class InfectionTypeAssets
            {
                public string name;
                public Sprite[] infectionBodies;
                public Sprite[] infectionEyes;
                public Sprite[] infectionDecorations;
            }
            public InfectionTypeAssets[] infectionTypeAssets;
        }
        public InfectionAssets infectionAssets;

        public bool _infectionInProgress;
        [SerializeField] float infectionWaitTimeCheck = 20;
        [SerializeField] float infectionChance = 5;
        bool _infectCheckRoutine;


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
        }
        private void Start()
        {
            CR_Idle_Manager.onGameStateChange += CR_Idle_Manager_onGameStateChange;
        }


        private void OnDestroy()
        {
            CR_Idle_Manager.onGameStateChange -= CR_Idle_Manager_onGameStateChange;
        }


        private void CR_Idle_Manager_onGameStateChange(CR_Idle_Manager.GameState state)
        {
            if (_infectCheckRoutine) return;
            StartCoroutine(InfectionCheck());
        }

        IEnumerator InfectionCheck()
        {
            _infectCheckRoutine = true;
            while (!_infectionInProgress)
            {
                yield return new WaitForSeconds(infectionWaitTimeCheck);
                float randomChance = Random.Range(0, 100);
                if (randomChance < infectionChance)
                {
                    InfectOrgan();
                }
            }
            _infectCheckRoutine = false;
        }

        void InfectOrgan()
        {
            var availableOrganLists = new List<CR_Data.OrganType.OrganInfo>();
            var typesList = new List<int>();
            var organNumberList = new List<int>();
            var data = CR_Data.data;
            for (int i = 0; i < data.organTypes.Length; i++)
            {
                if (data.organTypes[i].unlocked)
                {
                    for (int o = 0; o < data.organTypes[i].organs.Count; o++)
                    {
                        availableOrganLists.Add(data.organTypes[i].organs[o]);
                        typesList.Add(i);
                        organNumberList.Add(o);
                    }
                }
            }

            if (availableOrganLists.Count == 0) return;
            var randomOrgan = Random.Range(0, availableOrganLists.Count);
            var selectedOrgan = availableOrganLists[randomOrgan];
            Debug.Log("Organ Infected: " +"Type: "+ typesList[randomOrgan] + " number: " +organNumberList[randomOrgan]);
            _infectionInProgress = true;
            selectedOrgan.infection = CreateInfection();
            int amount = Random.Range(0, CR_Data.data.GetTotalCells(typesList[randomOrgan], organNumberList[randomOrgan], 0));
            selectedOrgan.InfectionAmount = amount;
            selectedOrgan.infected = true;
            //Check if we are in the newly infected organ
            if (CR_Idle_Manager.instance.gameState != CR_Idle_Manager.GameState.CellView) return;
            if (CR_Idle_Manager.instance.CurrentOrganType != typesList[randomOrgan]) return;
            if (CR_Idle_Manager.instance.CurrentOrganType != organNumberList[randomOrgan]) return;

            CR_CellViewManager.instance.SpawnPathogens();
        }

        Infection CreateInfection()
        {
            int randomType = Random.Range(0, (int)InfectionType.InfectionNum);
            int randomBodyTypeID = Random.Range(0, infectionAssets.infectionTypeAssets[randomType].infectionBodies.Length);
            int randomEyeTypeID = Random.Range(0, infectionAssets.infectionTypeAssets[randomType].infectionEyes.Length);
            int randomDecorationTypeID = Random.Range(0, infectionAssets.infectionTypeAssets[randomType].infectionDecorations.Length);

            var newInfection = new Infection
            {
                infectionType = (InfectionType)randomType,
                infectionBodyID = randomBodyTypeID,
                infectionEyesID = randomEyeTypeID,
                infectionDecorationsID = randomDecorationTypeID,

                health = 10,
                damage = 10
            };
            return newInfection;
        }
    }
}
