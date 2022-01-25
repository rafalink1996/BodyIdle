using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idle
{
    public class CR_OrganView_Manager : MonoBehaviour
    {
        public static CR_OrganView_Manager instance;
        [SerializeField] GameObject organHolder;

        [SerializeField] CR_OrganView_Organ[] organView_Organs;
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
        private void Start()
        {
            SpawnOrgans();
        }
        private void OnDestroy()
        {
            CR_Idle_Manager.onGameStateChange -= CR_Idle_Manager_onGameStateChange;
            //Rest of Awake code
        }

        private void CR_Idle_Manager_onGameStateChange(CR_Idle_Manager.GameState obj)
        {
            if(obj != CR_Idle_Manager.GameState.OrganView)
            {
                organHolder.SetActive(false);
                return;
            }

        }

        void SpawnOrgans()
        {
            for (int i = 0; i < organView_Organs.Length; i++)
            {
                for (int o = 0; o < 6; o++)
                {
                    Instantiate
                }
            }
        }

        
    }
}
