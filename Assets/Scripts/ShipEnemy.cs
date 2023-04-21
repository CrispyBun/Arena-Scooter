using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnemy : Ship
{
    protected GameObject playerShip;

    protected override void Start()
    {
        base.Start();
        playerShip = GameObject.FindGameObjectWithTag("Player");
    }

    protected float GetPlayerAngle()
    {
        if (playerShip == null) return 0;

        // i hate quaternions
        // i hate quaternions
        // i hate quaternions
        // i hate quaternions
        Vector3 targetAngle = (playerShip.transform.position - transform.position).normalized;
        float angle = Vector3.SignedAngle(transform.up, targetAngle, Vector3.forward) + 90f;
        if (angle > 180f) angle -= 360f;

        return angle;
    }

    protected float GetPlayerDistance()
    {
        if (playerShip == null) return 0;
        return Vector3.Distance(transform.position, playerShip.transform.position);
    }
}
