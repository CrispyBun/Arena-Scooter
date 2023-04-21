using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shotCooldownSeconds;
    [SerializeField] private float shotSpread;
    [SerializeField] private float shotSpeedDeviation;
    [SerializeField] private int shotAmount;

    private float shotCooldownTimer = 0;

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

    private void Shoot()
    {
        for (int i = 0; i < shotAmount; i++)
        {
            SpawnBullet();
        }
    }

    private void SpawnBullet()
    {
        float bulletAngleAddition = (Random.value - 0.5f) * shotSpread;
        Quaternion bulletAngle = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, bulletAngleAddition));

        GameObject projectileObject = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, 10), bulletAngle);
        Projectile projectileClass = projectileObject.GetComponent<Projectile>();

        float shotSpeed = (1000f * Random.Range(0f, shotSpeedDeviation)) + 1000f;
        if (team == Team.Enemy) shotSpeed /= 4f;

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
