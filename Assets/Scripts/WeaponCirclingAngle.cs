using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCirclingAngle : Weapon
{
    private float bulletAngle = 0_0;

    protected override void Shoot()
    {
        for (int i = 0; i < shotAmount; i++)
        {
            bulletAngle += 10f;
            if (bulletAngle > 360f) bulletAngle = 0f;
            SpawnBullet(bulletAngle);
        }
    }
}
