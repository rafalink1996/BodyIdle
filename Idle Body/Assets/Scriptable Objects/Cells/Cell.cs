using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cell", menuName = "Game/Cells")]
public class Cell : ScriptableObject
{
    public int CellID;
    public Sprite CellSprite;
    public  Animator CellAnimator;
   
}
