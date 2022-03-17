using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class SetGridLayoutGroup_2 : MonoBehaviour
{
    [SerializeField] RectTransform rt;
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    float _size;
    [SerializeField] float cellsX;

    private void Start()
    {
        GetReferences();
    }

    void GetReferences()
    {
        if (rt == null) rt = GetComponent<RectTransform>();
        if (gridLayoutGroup == null) gridLayoutGroup = GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount) cellsX = gridLayoutGroup.constraintCount;
    }
    void OnRectTransformDimensionsChange()
    {
        GetReferences();
        var w = rt.rect.width;
        var spacing = gridLayoutGroup.spacing.x;
        var paddingX = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;

        w -= spacing * 2f;
        w -= paddingX;
        _size = (w / cellsX);

        // Optional size limits 
        // size = size < 32f ? 32f : size;
        gridLayoutGroup.cellSize = new Vector2(_size, _size);
    }
}
