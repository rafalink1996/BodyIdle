using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Idle
{
    public class CR_OverlayUI : MonoBehaviour
    {
        [Space(10)]
        [Header("INFO")]
        [Header("ENERGY")]
        [SerializeField] GameObject _energyObject;
        [SerializeField] Button _energyButton;
        [SerializeField] TextMeshProUGUI _energyText;
        [SerializeField] TextMeshProUGUI _energyPerSecondText;
        [Header("COMPLEXITY")]
        [SerializeField] GameObject _complexityObject;
        [SerializeField] Button _complexityButton;
        [SerializeField] TextMeshProUGUI _complexityText;
        [SerializeField] TextMeshProUGUI _complexityMaxText;
        [Header("PREMIUM")]
        [SerializeField] GameObject _premiumObject;
        [SerializeField] Button _premiumButton;
        [SerializeField] TextMeshProUGUI _premiumText;


        [Space(10)]
        [Header("STORE")]
        [SerializeField] Button _storeButton;
        [SerializeField] GameObject _storeObject;

        [Space(10)]
        [Header("OPTIONS")]
        [SerializeField] Button _optionsButton;
        [SerializeField] GameObject _optionsObject;
        public void CustomStart()
        {
            UpdateEnergy();
            UpdateComplexity();
            UpdatePremium();
        }
        public void UpdateEnergy()
        {
            _energyText.text = AbbreviationUtility.AbbreviateNumber(CR_Data.data._energy);
            _energyPerSecondText.text = AbbreviationUtility.AbbreviateNumber(CR_Data.data._energyPerSecond) + "/s";
        }
        public void UpdateComplexity()
        {
            _complexityText.text = CR_Data.data._complexity.ToString();
            _complexityMaxText.text = "/" + CR_Data.data._maxComplexity;
        }
        public void UpdatePremium()
        {
            _premiumText.text = AbbreviationUtility.AbbreviateNumber(CR_Data.data._premium);
        }

    }
}
