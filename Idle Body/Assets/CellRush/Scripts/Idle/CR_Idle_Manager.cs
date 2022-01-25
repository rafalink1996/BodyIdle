using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Idle
{
    public class CR_Idle_Manager : MonoBehaviour
    {
        [Header("Instance")]
        public static CR_Idle_Manager instance;

        [Header("REFERENCES")]
        public CR_OverlayUI _overlayUI;
        public CR_CellViewManager _cellViewManager;
        public CR_OrganismViewManager _organismViewManager;


        public static event Action<GameState> onGameStateChange;
        public enum GameState
        {
            OrganismView,
            OrganView,
            CellView
        }
        public GameState gameState;

        public int currentOrganLoaded;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                //Rest of Awake code
                GetReferences();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        void GetReferences()
        {
            if (_cellViewManager == null) _cellViewManager = FindObjectOfType<CR_CellViewManager>();
            if (_overlayUI == null) _overlayUI = FindObjectOfType<CR_OverlayUI>();
            if (_organismViewManager == null) _organismViewManager = FindObjectOfType<CR_OrganismViewManager>();
        }
        void Start()
        {

            _cellViewManager.CustomStart();
            _overlayUI.CustomStart();

            // ChangeState(GameState.OrganismView);
            ChangeState(GameState.OrganView);
        }

        public void ChangeState(GameState state)
        {
            gameState = state;
            switch (state)
            {
                case GameState.OrganismView:
                    HandleOrganismView();
                    break;
                case GameState.OrganView:
                    HandleOrganView();
                    break;
                case GameState.CellView:
                    break;
                default:
                  
                    break;
            }
            onGameStateChange?.Invoke(state);
        }

        private void HandleOrganismView()
        {

        }
        private void HandleOrganView()
        {

        }



    }
}
