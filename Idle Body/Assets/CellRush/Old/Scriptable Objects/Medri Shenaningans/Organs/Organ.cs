using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Organ", menuName = "Game/Organs")]
public class Organ : ScriptableObject
{
    [Header ("Identification")]
    public int OrganTypeID;
    public string OrganName;

    [Space(10)]
    [Header("STATS")]
    public int pointMultiplier;
    public double OrganInitialCost;
    public double OrganCellInitialCost;
    public int[] ComplexityCosts;

    [Space(10)]
    [Header("Visuals")]
    public Sprite OrganSprite;
    public AnimatorOverrideController OrganAnimator;
}
