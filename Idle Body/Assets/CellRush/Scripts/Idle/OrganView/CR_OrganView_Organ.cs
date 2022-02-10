using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace Idle {
    public class CR_OrganView_Organ : MonoBehaviour
    {
        [SerializeField] int ID;
        CR_OrganView_Manager _manager;


        public void SetOrgan(int organNumber, CR_OrganView_Manager manager)
        {
            ID = organNumber;
            this._manager = manager;
        }


        public void OnClickLong()
        {
            if (_manager == null) return;
            _manager.ShowOrganInfo(ID);

        }
        public void OnClickShort()
        {
            if (_manager == null) return;
            _manager.LoadOrgan(ID);
        }
    }
}
