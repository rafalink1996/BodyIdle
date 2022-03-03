using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Idle
{
    public class CR_OrganView_Texts : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _upgradeMultiplier;
        [SerializeField] TextMeshProUGUI _buyPlatletes;
        [SerializeField] TextMeshProUGUI _boosters;
        [SerializeField] TextMeshProUGUI _energyProduction;
        [SerializeField] TextMeshProUGUI _sell;
        [SerializeField] TextMeshProUGUI _buy;

        public void UpdateTexts()
        {
            if (_upgradeMultiplier != null) _upgradeMultiplier.text = LanguageManager.instance.organViewTexts.UpgradeMultiplier[(int)CR_Data.data._language];
            if (_buyPlatletes != null) _buyPlatletes.text = LanguageManager.instance.organViewTexts.BuyPlatles[(int)CR_Data.data._language];
            if (_boosters != null) _boosters.text = LanguageManager.instance.organViewTexts.Boosters[(int)CR_Data.data._language];
            if (_energyProduction != null) _energyProduction.text = LanguageManager.instance.organViewTexts.EnergyProduction[(int)CR_Data.data._language];
            if (_sell != null) _sell.text = LanguageManager.instance.organViewTexts.Sell[(int)CR_Data.data._language];
            if (_buy != null) _buy.text = LanguageManager.instance.organismViewTexts.Buy[(int)CR_Data.data._language];

        }
    }
}
