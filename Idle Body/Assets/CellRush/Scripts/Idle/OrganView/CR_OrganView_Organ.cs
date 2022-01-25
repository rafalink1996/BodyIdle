using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Idle {
    public class CR_OrganView_Organ : MonoBehaviour
    {
        [SerializeField] int ID;
      
        public void SetOrgan(int organNumber)
        {
            ID = organNumber;
        }


        public void OnClickLong()
        {

        }
        public void OnClickShort()
        {

        }
    }
}
