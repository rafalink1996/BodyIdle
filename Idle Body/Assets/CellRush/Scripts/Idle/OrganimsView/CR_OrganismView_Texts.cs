using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Idle
{
    public class CR_OrganismView_Texts : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _seeOrganTexts;

        public void UpdateTexts()
        {
           if(_seeOrganTexts != null) _seeOrganTexts.text = LanguageManager.instance.organismViewTexts.SeeOrgans[(int)CR_Data.data._language];
        }
    }
}
