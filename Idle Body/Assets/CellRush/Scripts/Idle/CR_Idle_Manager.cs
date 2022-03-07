using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BreakInfinity;

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
        public TransitionAnimation _TransitionAnimation;
        [Header("ORGAN TYPE INFO")]
        public OrganTypeAsstes[] organTypeAsstes;

        public static event Action<GameState> onGameStateChange;

        public enum GameState
        {
            OrganismView,
            OrganView,
            CellView
        }
        public GameState gameState;

        public int CurrentOrganType;
        public int CurrentOrganNumber;

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
            if (_TransitionAnimation == null) _TransitionAnimation = FindObjectOfType<TransitionAnimation>();
        }
        void Start()
        {
            _overlayUI.CustomStart();
            if (!_TransitionAnimation.gameObject.activeSelf) _TransitionAnimation.gameObject.SetActive(true);
            StartCoroutine(ChangeState(GameState.OrganismView));
        }

        public IEnumerator ChangeState(GameState state)
        {
            
            yield return _TransitionAnimation.TransitionIn();
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
            yield return _TransitionAnimation.TransitionOut();
        }

        private void HandleOrganismView()
        {

        }
        private void HandleOrganView()
        {

        }

        [System.Serializable]
        public class OrganUpgrade
        {
            [SerializeField] public enum UpgradeType { Multiply, MultiplyAndOrganPower, Power };
            [SerializeField] public UpgradeType type;
            [SerializeField] public float amount;
            [SerializeField] public BigDouble UpgradeCost;
            [SerializeField] public string[] description = new string[(int)CR_Data.Languages.NumOfLanguages ];
        }
        [System.Serializable]
        public class OrganTypeAsstes
        {
            public string name;
            public Sprite organSprite;
            public Sprite OrganBorder;
            public Animator OrganAnimator;
            public Color OrganBackgroundColor;
            public List<OrganUpgrade> upgrades;
        }

    }
}
