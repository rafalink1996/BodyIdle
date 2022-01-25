using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Idle
{
    public class CR_OrganismView_Organ : MonoBehaviour
    {
        [SerializeField] Image organImage;
        [SerializeField ]private int ID;
        

        private void Awake()
        {
            if (organImage == null) organImage = GetComponent<Image>();
        }
        public void SetOrgan(Sprite OrganSprite, int ID)
        {
            organImage.sprite = OrganSprite;
            this.ID = ID;
        }
        public void OnOrganClick()
        {
            
            CR_OrganismViewManager.instance.SelectOrgan(ID);
            // cargar pantalla de organo
        }
        public void OnOrganLongClick()
        {
           
            // cargar informacion de organo
        }

        public void ChangeColor(Color color)
        {
            organImage.color = color;
        }

    }
}
