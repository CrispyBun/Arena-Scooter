using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInvader : ShipEnemy
{
    private float keptDistance = 12f;

    void Update()
    {
        // Turn
        shipControlLeft = false;
        shipControlRight = false;
        float angle = GetPlayerAngle();
        if (angle > 5f)
        {
            shipControlLeft = true;
        }
        else if (angle < -5f)
        {
            shipControlRight = true;
        }

        // Move
        shipControlThrust = false;
        if (GetPlayerDistance() > keptDistance && !(shipControlLeft || shipControlRight))
        {
            shipControlThrust = true;
        }
    }
}
