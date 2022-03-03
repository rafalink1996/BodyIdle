using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idle
{
    public class CR_PathogenSystem : MonoBehaviour
    {
        public static CR_PathogenSystem instance;


        public enum InfectionType { Bacterial, Viral, Fungal };
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

        public class InfectionAssets
        {
            public class InfectionTypeAssets
            {
                public Sprite[] infectionBodies;
                public Sprite[] infectionEyes;
                public Sprite[] infectionDecorations;
            }
            public InfectionTypeAssets[] infectionTypeAssets;
        }

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
                    _infectionInProgress = true;
                }
            }
            _infectCheckRoutine = false;
        }

        void InfectOrgan()
        {
            var availableOrganLists = new List<CR_Data.OrganType.OrganInfo>();
            var data = CR_Data.data;
            for (int i = 0; i < data.organTypes.Length; i++)
            {
                if (data.organTypes[i].unlocked)
                {
                    for (int o = 0; o < data.organTypes[i].organs.Count; o++)
                    {
                        availableOrganLists.Add(data.organTypes[i].organs[o]);
                    }
                }
            }

            if (availableOrganLists.Count == 0) return;
            var randomOrgan = Random.Range(0, availableOrganLists.Count);
            var selectedOrgan = availableOrganLists[randomOrgan];
            selectedOrgan.infected = true;

        }




    }
}
