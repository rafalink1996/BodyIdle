using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idle
{
    public class CR_CellviewCell : CR_CellBase
    {

        [SerializeField] protected Sprite[] _cellSprites;

        [SerializeField] CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo _info;

        public override void InitializeCell(CellSize cellSize, CellType cellType)
        {
            _renderer.sprite = _cellSprites[(int)cellSize];
            switch (cellSize)
            {
                case CellSize.Small:
                    transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                    break;
                case CellSize.Medium:
                    transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
                    break;
                case CellSize.Big:
                    transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
                    break;
                default:
                    break;
            }
            base.InitializeCell(cellSize, cellType);
        }

        public virtual void SetCellInfo(CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo info)
        {
            _info = info;
        }
    }
}
