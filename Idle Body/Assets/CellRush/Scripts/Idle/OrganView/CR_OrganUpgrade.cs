using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Idle
{
    public class CR_OrganUpgrade : MonoBehaviour
    {
        int index;
        [SerializeField] CR_Idle_Manager.OrganUpgrade.UpgradeType type;
        [SerializeField] GameObject x2Image;
        [SerializeField] GameObject MPOImage;

        [Header("LOCKED PANEL")]
        [SerializeField] Image _blackOverlay;
        [SerializeField] Image _lockedImage;
        bool UpgradeSet;

        [Header("UTILITY")]
        [SerializeField] bool isUtility = false;

        public void SetUpgrade(CR_Idle_Manager.OrganUpgrade.UpgradeType type, int index)
        {
            gameObject.SetActive(true);
            this.type = type;
            this.index = index;
            switch (type)
            {
                case CR_Idle_Manager.OrganUpgrade.UpgradeType.Multiply:
                    x2Image.SetActive(true);
                    MPOImage.SetActive(false);
                    break;
                case CR_Idle_Manager.OrganUpgrade.UpgradeType.MultiplyAndOrganPower:
                    x2Image.SetActive(false);
                    MPOImage.SetActive(true);
                    break;
                case CR_Idle_Manager.OrganUpgrade.UpgradeType.Power:
                    x2Image.SetActive(false);
                    MPOImage.SetActive(false);
                    break;
                default:
                    break;
            }
            UpdateUpgrade();
            UpgradeSet = true;
        }

        public void UpdateUpgrade()
        {
            if (!CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].upgrades[index])
            {
                _blackOverlay.gameObject.SetActive(true);
                _blackOverlay.color = new Color(0, 0, 0, .8f);
                if (index != 0)
                {
                    if (!CR_Data.data.organTypes[CR_Idle_Manager.instance.CurrentOrganType].upgrades[index - 1])
                    {
                        _lockedImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        _lockedImage.gameObject.SetActive(false);
                    }
                }
                else
                {
                    _lockedImage.gameObject.SetActive(false);
                }
            }
            else
            {
                _lockedImage.gameObject.SetActive(false);
                _blackOverlay.gameObject.SetActive(false);
            }
        }

        public void HideUpgrade()
        {
            gameObject.SetActive(false);
        }

        public void OnClickUpgrade()
        {
            if (!UpgradeSet) return;
            if (isUtility) return;
            if (CR_OrganView_Manager.instance == null) return;
            CR_OrganView_Manager.instance.DisplayUpgradeInfo(index, type);

        }
    }
}
