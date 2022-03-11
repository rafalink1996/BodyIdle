using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BreakInfinity;
using TMPro;

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
        public CR_OfflineProgress _OfflineProgress;
        [Header("ORGAN TYPE INFO")]
        public OrganTypeAsstes[] organTypeAsstes;

        CR_Data _data;
        [Range(0.1f, 1f)]
        [SerializeField] float _energyPerSecondTime = 1;
        float waitTime = 0;
        bool loadedFirstTime;


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
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            GetReferences();
        }
        void GetReferences()
        {
            if (_data == null) _data = CR_Data.data;
            if (_cellViewManager == null) _cellViewManager = FindObjectOfType<CR_CellViewManager>();
            if (_overlayUI == null) _overlayUI = FindObjectOfType<CR_OverlayUI>();
            if (_organismViewManager == null) _organismViewManager = FindObjectOfType<CR_OrganismViewManager>();
            if (_TransitionAnimation == null) _TransitionAnimation = FindObjectOfType<TransitionAnimation>();
            if (_OfflineProgress == null) _OfflineProgress = FindObjectOfType<CR_OfflineProgress>();
        }

        public void Start()
        {
            GetReferences();
            if (!_TransitionAnimation.gameObject.activeSelf) _TransitionAnimation.gameObject.SetActive(true);
            //Game should start here
            CR_SaveSystem.instance.Load(out bool saveExists);
            if (saveExists == false) return;
            if (CR_Data.data._energyPerSecond > 0)
            {
                if (_OfflineProgress != null)
                {
                    StartCoroutine(_OfflineProgress.CheckLoadFiles());
                }
                else
                {
                    Debug.Log("Offlone progress script is null");
                    GameStart();
                }
            }
            else
            {
                GameStart();
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            if (CR_SaveSystem.instance == null) return;
            if (!CR_Data.data.gameStarted) return;
            if (!focus)
            {
                Debug.Log("Change focus: " + focus);
                CR_SaveSystem.instance.save();
            }
            else
            {
                CR_SaveSystem.instance.Load(out bool saveExists);
                if (saveExists == false) return;
                if (CR_Data.data._energyPerSecond == 0) return;
                if (_OfflineProgress == null) return;
                StartCoroutine(CheckProgress());
            }
        }

        IEnumerator CheckProgress()
        {
            //yield return _TransitionAnimation.TransitionIn();
            yield return _OfflineProgress.CheckLoadFiles();
        }
        public void GameStart()
        {
            CR_Data.data.gameStarted = true;
            Debug.Log("gamestart");
            GetReferences();
            _overlayUI.CustomStart();
            StartCoroutine(ChangeState(GameState.OrganismView));
        }

        private void Update()
        {
            AddEnergyPerSecond();

        }

        void AddEnergyPerSecond()
        {

            if (_data._energyPerSecond == 0) return;
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                waitTime = _energyPerSecondTime;
                _data.SetEnergy(_data._energy + (_data._energyPerSecond * _energyPerSecondTime));
            }
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
            [SerializeField] public string[] description = new string[(int)CR_Data.Languages.NumOfLanguages];
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
