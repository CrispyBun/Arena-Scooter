using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBackAndForth : Weapon
{
    int direction = 1;
    protected override void Shoot()
    {
        for (int i = 0; i < shotAmount; i++)
        {
            float bulletAngle = 0;
            if (shotAmount > 1)
            {
                bulletAngle = ((float)(i) / (float)(shotAmount - 1) - 0.5f) * shotSpread;
            }
            SpawnBullet(bulletAngle, direction);
        }

        direction *= -1;

        if (shootSound) shootSound.Play();
    }
}
