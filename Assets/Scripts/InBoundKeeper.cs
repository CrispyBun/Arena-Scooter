using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InBoundKeepType
{
    None,
    Destroy,
    Bounce
}

public class InBoundKeeper : MonoBehaviour
{
    public static Arena arena;

    private Rigidbody2D rigidBody;

    [SerializeField] private InBoundKeepType boundKeepType;

    [SerializeField] private GameObject bounceBulletSpawnProjectile;
    private float bouncebulletSpawnSpeedRequirement = 16f;
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        switch (boundKeepType)
        {
            case InBoundKeepType.Destroy:
                if (!arena.PointInSafeBounds(transform.position))
                {
                    Ship objectShipClass = GetComponent<Ship>();
                    if (objectShipClass)
                    {
                        objectShipClass.DestroySelf(true);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                break;

            case InBoundKeepType.Bounce:
                // player spawns bullets on big wall bounces
                if (bounceBulletSpawnProjectile && rigidBody.velocity.magnitude >= bouncebulletSpawnSpeedRequirement && !arena.PointInBounds(transform.position))
                {
                    int shotAmount = 32;
                    for (int i = 0; i < shotAmount; i++)
                    {
                        float angle = (Convert.ToSingle(i) / Convert.ToSingle(shotAmount - 1) - 0.5f) * 360;
                        Quaternion bulletAngle = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, angle));

                        GameObject projectileObject = Instantiate(bounceBulletSpawnProjectile, new Vector3(transform.position.x, transform.position.y, 10), bulletAngle);
                        Projectile projectileClass = projectileObject.GetComponent<Projectile>();

                        projectileClass.SetTeam(Team.Player);
                        projectileClass.SetVelocity(Vector2.up * 1000f);
                    }
                }

                Vector3 position = gameObject.transform.position;
                Vector2 bounds = arena.GetBounds() / 2f;

                if (position.x > bounds.x || position.x < -bounds.x)
                {
                    position.x = Mathf.Min(position.x, bounds.x);
                    position.x = Mathf.Max(position.x, -bounds.x);
                    rigidBody.velocity = new Vector2(-rigidBody.velocity.x, rigidBody.velocity.y);
                }

                if (position.y > bounds.y || position.y < -bounds.y)
                {
                    position.y = Mathf.Min(position.y, bounds.y);
                    position.y = Mathf.Max(position.y, -bounds.y);
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, -rigidBody.velocity.y);
                }

                gameObject.transform.position = position;

                break;
        }
    }
}
