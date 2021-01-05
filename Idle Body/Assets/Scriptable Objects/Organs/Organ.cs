using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Organ", menuName = "Game/Organs")]
public class Organ : ScriptableObject
{
    public int OrganID;
    public string OrganName;
    public Sprite OrganSprite;
    public AnimatorOverrideController OrganAnimator;
    public float OrganStartingPointsPerSecond;

    public int OrganPointCost;
    public int OrganCellCost;

    public int OrganLevelClearence;


}
