using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGridLayoutGroup : MonoBehaviour
{
    [SerializeField] GridLayoutGroup group;
    [SerializeField] RectTransform rect;
    [SerializeField] bool _width;
    [SerializeField] int Rows = 4;
    [SerializeField] int Collumns = 4;

    [Header("SPACE ADJUSTER")]
    [SerializeField] float sizeAdjuster = 1;
    [SerializeField] bool UseSpacingWithAdjuster;

    [Header("CHILDREN")]
    [SerializeField] bool UseObjectChildren;


    [Header("OPTIONS")]
    [SerializeField] bool StartOnly = false;
    [SerializeField] bool ManualSet = false;

    private void Start()
    {
        group = GetComponent<GridLayoutGroup>();
        rect = GetComponent<RectTransform>();
        if (ManualSet) return;
        setGridLayoutGroup();

    }

    private void OnEnable()
    {
        if (StartOnly) return;
        if (ManualSet) return;
        setGridLayoutGroup();
    }

    public void Set()
    {
        setGridLayoutGroup();
    }
    void setGridLayoutGroup()
    {
        var cellSize = group.cellSize;
        float size = 0;
        if (!_width)
        {
            if (UseObjectChildren) Rows = transform.childCount;
            size = ((((rect.rect.height * 2) / 3) / Rows)) * sizeAdjuster;
        }
        else
        {
            if (UseObjectChildren) Collumns = transform.childCount;
            size = ((((rect.rect.width * 2) / 3) / Collumns)) * sizeAdjuster;
        }
       
        cellSize.y = size;
        cellSize.x = size;
        group.cellSize = cellSize;
        if (UseSpacingWithAdjuster)
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
