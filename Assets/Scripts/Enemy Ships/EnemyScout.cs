using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScout : ShipEnemy
{
    private Vector3 scoutingPoint;
    private float scoutDelaySeconds = 5f;
    private float scoutingTimer = 0f;

    protected override void Start()
    {
        base.Start();
        scoutingPoint = transform.position;
    }

    void Update()
    {
        scoutingTimer -= Time.deltaTime;
        if (scoutingTimer < 0f)
        {
            scoutingTimer = scoutDelaySeconds;
            scoutingPoint = InBoundKeeper.arena.GetRandomPointInBounds();
        }

        TurnTowardsPoint(scoutingPoint);
        ApproachPoint(scoutingPoint);
    }
}
