using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cell", menuName = "ScriptableObjects/Cells")]
public class CellsSO : ScriptableObject
{
    public enum CellType {RedBlood, WhiteBlood, Helper}
    public CellType MyCellType;


    public Sprite Cellx1, Cellx10, Cellx100;
    public Color MyColor;


}
