using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idle
{
    public class CR_CellViewPathogens : MonoBehaviour
    {
        public CR_Data.OrganType.OrganInfo organ;

        [SerializeField] GameObject _pathogenSpawner;
        [SerializeField] CR_CellView_UI _cellView_UI;

        private void Start()
        {
            if (_cellView_UI == null) _cellView_UI = CR_CellViewManager.instance._cellView_UI;
            _pathogenSpawner.SetActive(false);
        }
        public void SetPathogens()
        {
            CheckOrgan();
            if (organ == null) return;
            if (!organ.infected) return;
            if (organ.infection == null) return;
            if (organ.InfectionAmount <= 0) return;
            _pathogenSpawner.SetActive(true);
            _cellView_UI.SetPathogenUI(organ.infection, organ.InfectionAmount);

        }

        void CheckOrgan()
        {
            var manager = CR_Idle_Manager.instance;
            var OrganType = manager.CurrentOrganType;
            var OrganNumber = manager.CurrentOrganNumber;
            organ = CR_Data.data.organTypes[OrganType].organs[OrganNumber];
        }
    }
}
