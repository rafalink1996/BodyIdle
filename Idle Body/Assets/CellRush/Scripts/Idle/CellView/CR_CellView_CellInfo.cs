using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Idle
{
    public class CR_CellView_CellInfo : MonoBehaviour
    {
        [SerializeField] Image _cellInfoImage;
        CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo info;

        [Header ("IMAGE INFO")]
        [SerializeField] Image _cellImage;
        [SerializeField] Image _cellImageHolder;


        [Header("TIMER INFO")]
        [SerializeField] Transform _cellTimerHolder;
        [SerializeField] TextMeshProUGUI _cellTimerText;

        [Header("ASSETS")]
        [SerializeField] Sprite[] _redCellSprites;
        [SerializeField] Sprite[] _whiteCellSprites;
        [SerializeField] Sprite[] _helperCellSprites;
        [SerializeField] Color[] _cellColors;


        public void ActivateCellInfo(CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo info, CR_CellBase.CellType type, CR_CellBase.CellSize size)
        {
            SetCellInfo(info, type, size);
        }
       void SetCellInfo(CR_Data.OrganType.OrganInfo.cellsType.CellSizes.CellInfo info, CR_CellBase.CellType type, CR_CellBase.CellSize size)
        {
            this.info = info;
            Sprite[] sprites = _redCellSprites;
            Color color = Color.white;
            switch (type)
            {
                case CR_CellBase.CellType.RedBlood:
                    sprites = _redCellSprites;
                    color = _cellColors[0];
                    break;
                case CR_CellBase.CellType.White:
                    sprites = _whiteCellSprites;
                    color = _cellColors[1];
                    break;
                case CR_CellBase.CellType.Helper:
                    sprites = _helperCellSprites;
                    color = _cellColors[2];
                    break;
            }
            _cellImage.sprite = sprites[(int)size];
            _cellImageHolder.color = color;
            _cellInfoImage.color = color;
        }

        public void DeactivateCellInfo()
        {
            info = null;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            UpdateInfo();
        }

        void UpdateInfo()
        {
            if (info == null) return;
            if (info.alive)
            {
                if(!_cellImageHolder.gameObject.activeSelf)_cellImageHolder.gameObject.SetActive(true);
                if (_cellTimerHolder.gameObject.activeSelf) _cellTimerHolder.gameObject.SetActive(false);
            }
            else
            {
                if (_cellImageHolder.gameObject.activeSelf) _cellImageHolder.gameObject.SetActive(false);
                _cellTimerText.text = info.timer.ToString();
            }
        }
    }
}
    