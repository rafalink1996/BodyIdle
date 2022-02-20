using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Idle
{
    public class CR_Platlet : CR_CellBase
    {
        [Header("Platlet ID")]
        [SerializeField] Sprite[] _platletSprites;
        float[] _platletSizes = new float[] { 0.006f, 0.008f, 0.01f };

        public override void InitializeCell(CellSize cellSize, CellType cellType)
        {
            _move = true;
            float size = _platletSizes[(int)cellSize];
            transform.localScale = new Vector3(size, size, size);
            _renderer.sprite = _platletSprites[(int)cellSize];
            base.InitializeCell(cellSize, cellType);
        }

        public override void StartCell()
        {
            Vector3 DesiredSize = transform.localScale;
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, DesiredSize, 0.3f).setEase(LeanTweenType.easeOutExpo);
            base.StartCell();
        }

    }
}
