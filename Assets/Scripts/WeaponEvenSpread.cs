using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEvenSpread : Weapon
{
    protected override void Shoot()
    {
        for (int i = 0; i < shotAmount; i++)
        {
            float bulletAngle = ((float)(i) / (float)(shotAmount-1) - 0.5f) * shotSpread;
            SpawnBullet(bulletAngle);
        }
    }
}
