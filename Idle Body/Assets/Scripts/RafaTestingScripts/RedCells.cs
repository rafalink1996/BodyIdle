using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCells : Cell_Base
{
    public override void GetNewTarget()
    {
        base.GetNewTarget();
    }
    protected override void PathogenHit(Pathogen_Base pathogen_Base)
    {
        // Take Damage
        // Update Health
        base.PathogenHit(pathogen_Base);
    }
}
