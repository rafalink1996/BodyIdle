using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Infection", menuName = "Game/Infection")]
public class Infection : ScriptableObject
{
    public string InfectionName;
    public int infectionType;
        // 0 = bacteria
        // 1 = virus
        // 2 = parasyte
        // 3 = Fungi

    public float MaxHealh;
    public float Damage;

    public Sprite InfectionSprite;
    
}
