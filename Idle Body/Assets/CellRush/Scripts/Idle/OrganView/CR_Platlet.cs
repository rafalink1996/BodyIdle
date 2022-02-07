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

        public void SetPlatlet(int platletSize)
        {
            float size = _platletSizes[(int)platletSize];
            transform.localScale = new Vector3(size, size, size);
            _renderer.sprite = _platletSprites[(int)platletSize];
        }


    }
}
