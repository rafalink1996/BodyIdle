using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idle
{
    public class CR_CellBase : MonoBehaviour
    {
        #region Variables
        public enum CellSize { Small, Medium, Big };
        public CellSize cellSize;
        public int ID;
        public int CellType;
        [SerializeField] Sprite CombineSprite = null;
        [SerializeField] protected CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo myInfo;
        [SerializeField] protected SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

       
        #endregion Variables


    }
}
