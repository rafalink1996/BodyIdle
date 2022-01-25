using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Idle {
    public class CR_OrganView_Organ : MonoBehaviour
    {
        [SerializeField] Image organImage;
        [SerializeField] TextMeshProUGUI OrganNumber;
        [SerializeField] int ID;

        [SerializeField] CR_EyeAnimator eyes;
        [SerializeField] Sprite[] organSprites;

        public enum OrganTypesNames
        {
            Heart,
            Stomach,
            Lungs,
            Gills,
            Kidneys,
            Intestines,
            Liver,
            Pancreas,
            Bladder,
            Brain,
            Poison,
            Bones
        }

        [SerializeField] OrganTypesNames CurrentOrganType;
        public void SetOrgan(OrganTypesNames organType)
        {
            switch (organType)
            {
                case OrganTypesNames.Heart:
                    organImage.sprite = organSprites[0];
                    break;
                case OrganTypesNames.Stomach:
                    organImage.sprite = organSprites[1];
                    break;
                case OrganTypesNames.Lungs:
                    organImage.sprite = organSprites[2];
                    break;
                case OrganTypesNames.Gills:
                    organImage.sprite = organSprites[3];
                    break;
                case OrganTypesNames.Kidneys:
                    organImage.sprite = organSprites[4];
                    break;
                case OrganTypesNames.Intestines:
                    organImage.sprite = organSprites[5];
                    break;
                case OrganTypesNames.Liver:
                    organImage.sprite = organSprites[6];
                    break;
                case OrganTypesNames.Pancreas:
                    organImage.sprite = organSprites[7];
                    break;
                case OrganTypesNames.Bladder:
                    organImage.sprite = organSprites[8];
                    break;
                case OrganTypesNames.Brain:
                    organImage.sprite = organSprites[9];
                    break;
                case OrganTypesNames.Poison:
                    organImage.sprite = organSprites[10];
                    break;
                case OrganTypesNames.Bones:
                    organImage.sprite = organSprites[11];
                    break;
                default:
                    break;
            }
        }


        public void OnClickLong()
        {

        }
        public void OnClickShort()
        {

        }

    }
}
