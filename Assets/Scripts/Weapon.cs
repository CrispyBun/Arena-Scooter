using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected AudioSource shootSound;

    [SerializeField] protected Team team;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float shotCooldownSeconds;
    [SerializeField] protected float shotSpread;
    [SerializeField] protected float shotSpeedDeviation;
    [SerializeField] protected int shotAmount;
    [SerializeField] protected float shotPowerMultiplier = 1;

    protected float shotCooldownTimer = 0;

    void Start()
    {
    }

    void Update()
    {
        shotCooldownTimer -= Time.deltaTime;
        if (shotCooldownTimer <= 0)
        {
            Shoot();
            shotCooldownTimer = shotCooldownSeconds;
        }
    }

    protected virtual void Shoot()
    {
        for (int i = 0; i < shotAmount; i++)
        {
            float bulletAngle = (Random.value - 0.5f) * shotSpread;
            SpawnBullet(bulletAngle);
        }

        if (shootSound) shootSound.Play();
    }

    protected void SpawnBullet(float angle, float shotSpeedMultiplier = 1)
    {
        Quaternion bulletAngle = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, angle));

        GameObject projectileObject = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 10), bulletAngle);
        Projectile projectileClass = projectileObject.GetComponent<Projectile>();

        float shotSpeed = (1000f * Random.Range(0f, shotSpeedDeviation)) + 1000f;
        if (team == Team.Enemy) shotSpeed /= 4f;
        shotSpeed *= shotPowerMultiplier * shotSpeedMultiplier;

        projectileClass.SetTeam(team);
        projectileClass.SetVelocity(Vector2.up * shotSpeed);
    }

    public Team GetTeam()
    {
        return team;
    }

    public void SetTeam(Team newTeam)
    {
        team = newTeam;
    }
}
