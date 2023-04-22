using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInvader : ShipEnemy
{
    void Update()
    {
        TurnTowardsPlayer();
        ApproachPlayer();
    }
}
