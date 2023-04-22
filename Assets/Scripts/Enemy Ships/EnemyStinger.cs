using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStinger : ShipEnemy
{
    protected override void Start()
    {
        base.Start();
        shipControlThrust = true;
    }
}