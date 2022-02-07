using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGridLayoutGroup : MonoBehaviour
{
    [SerializeField] GridLayoutGroup group;
    [SerializeField] RectTransform rect;
    [SerializeField] int Rows = 4;
    [SerializeField] float sizeAdjuster = 1;
    [SerializeField] bool UseSpacingwithAdjuster;

    private void Start()
    {
        group = GetComponent<GridLayoutGroup>();
        rect = GetComponent<RectTransform>();

    }
    private void OnEnable()
    {
        setGridLayoutGroup();
    }
    void setGridLayoutGroup()
    {

        var cellSize = group.cellSize;
        float size = ((((rect.rect.height * 2) / 3) / Rows)) * sizeAdjuster;
        cellSize.y = size;
        cellSize.x = size;
        group.cellSize = cellSize;

        //if (size * 3 > (1080 - 200)) return;
        if (UseSpacingwithAdjuster)
        {
            float SpacingAmount = (rect.rect.height * 2 / 3 / Rows) - ((rect.rect.height * 2 / 3 / Rows) * sizeAdjuster);
            var Spacing = group.spacing;
            Spacing.x = SpacingAmount;
            Spacing.y = SpacingAmount;
            group.spacing = Spacing;
        }
    }
    float GetCanvasHeight()
    {
        float S_Width = Screen.width;
        float S_Height = Screen.height;

        float Ratio = 0;
        if (S_Height < S_Width)
        {
            Ratio = S_Width / S_Height;
        }
        else
        {
            Ratio = S_Height / S_Width;
        }
        return Ratio * 1080;
    }
}
