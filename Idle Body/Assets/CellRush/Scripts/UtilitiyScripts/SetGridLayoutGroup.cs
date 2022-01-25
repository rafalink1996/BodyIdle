using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGridLayoutGroup : MonoBehaviour
{
    [SerializeField] GridLayoutGroup group;
    [SerializeField] RectTransform rect;
    [SerializeField]int Rows = 4;

    private void Start()
    {
        group = GetComponent<GridLayoutGroup>();
        rect = GetComponent<RectTransform>();
        setGridLayoutGroup();
        Debug.Log("Width= " + rect.rect.width + ", " + "Height= " + rect.rect.height);
    }
    void setGridLayoutGroup()
    {
        var cellSize = group.cellSize;
        float size = ((rect.rect.height * 2) / 3) / Rows;
        cellSize.y = size;
        cellSize.x = size;
        group.cellSize = cellSize;
        
        //if (size * 3 > (1080 - 200)) return;
        //var Spacing = group.spacing;
        //Spacing.x = (1080 - 200) - (size * 3);
        //group.spacing = Spacing;
            
 
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
