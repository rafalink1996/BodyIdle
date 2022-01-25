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

        [SerializeField] CR_OrganView_Organ organs;

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

        private void CR_Idle_Manager_onGameStateChange(CR_Idle_Manager.GameState obj)
        {
            if(obj != CR_Idle_Manager.GameState.OrganView)
            {
                organHolder.SetActive(false);
                return;
            }

        }

        
    }
}
