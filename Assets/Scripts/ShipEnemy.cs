using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ShipEnemy : Ship
{
    protected GameObject playerShip;

    [SerializeField] private float turnToPointAngleMargin = 5f;
    [SerializeField] private float approachPointDistanceMargin = 1f;

    protected override void Start()
    {
        base.Start();
        playerShip = GameObject.FindGameObjectWithTag("Player");
    }

    protected float GetAngleToPoint(Vector3 point)
    {
        // i hate quaternions
        // i hate quaternions
        // i hate quaternions
        // i hate quaternions
        Vector3 targetAngle = (point - transform.position).normalized;
        float angle = Vector3.SignedAngle(transform.up, targetAngle, Vector3.forward) + 90f;
        if (angle > 180f) angle -= 360f;

        return angle;
    }

    protected float GetDistanceToPoint(Vector3 point)
    {
        return Vector3.Distance(transform.position, point);
    }

    protected float GetPlayerAngle()
    {
        if (playerShip == null) return 0;

        return GetAngleToPoint(playerShip.transform.position);
    }

    protected float GetPlayerDistance()
    {
        if (playerShip == null) return 0;
        return GetDistanceToPoint(playerShip.transform.position);
    }

    protected void TurnTowardsPoint(Vector3 point)
    {
        shipControlLeft = false;
        shipControlRight = false;
        float angle = GetAngleToPoint(point);
        if (angle > turnToPointAngleMargin)
        {
            shipControlLeft = true;
        }
        else if (angle < -turnToPointAngleMargin)
        {
            shipControlRight = true;
        }
    }

    protected void ApproachPoint(Vector3 point, bool ignoreTurnBlocking = false)
    {
        shipControlThrust = false;
        if (GetDistanceToPoint(point) > approachPointDistanceMargin && ((ignoreTurnBlocking) || !(shipControlLeft || shipControlRight)))
        {
            shipControlThrust = true;
        }
    }

    protected void TurnTowardsPlayer()
    {
        if (playerShip == null)
        {
            shipControlLeft = false;
            shipControlRight = false;
        }
        else
        {
            TurnTowardsPoint(playerShip.transform.position);
        }
    }

    protected void ApproachPlayer()
    {
        if (playerShip == null)
        {
            shipControlThrust = false;
        }
        else
        {
            ApproachPoint(playerShip.transform.position);
        }
    }
}
