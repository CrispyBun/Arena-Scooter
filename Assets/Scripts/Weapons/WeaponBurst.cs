using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBurst : Weapon
{
    [SerializeField] private int burstBulletCount = 4;
    [SerializeField] private int burstGhostBullets = 2;
    private int burstBulletCounter = 0_0;
    private bool canShoot = true;

    protected override void Shoot()
    {
        for (int i = 0; i < shotAmount; i++)
        {
            if (canShoot)
            {
                if (burstBulletCounter > burstBulletCount)
                {
                    canShoot = false;
                    burstBulletCounter = 0;
                }
            }
            else
            {
                if (burstBulletCounter > burstGhostBullets)
                {
                    canShoot = true;
                    burstBulletCounter = 0;
                }
            }

            if (canShoot)
            {
                float bulletAngle = (Random.value - 0.5f) * shotSpread;
                SpawnBullet(bulletAngle);
                if (shootSound) shootSound.Play();
            }

            burstBulletCounter++;
        }
    }
}
