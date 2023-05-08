using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearMissRewarder : MonoBehaviour
{
    private ShipPlayer player;
    private BoxCollider2D[] colliders;

    private Projectile colliderLeftHit = null;
    private Projectile colliderRightHit = null;
    private float hitMarginSeconds = 0.5f;
    private float hitTimer = 0f;

    void Start()
    {
        player = transform.parent.gameObject.GetComponent<ShipPlayer>();
        colliders = GetComponents<BoxCollider2D>();
    }

    private void Update()
    {
        hitTimer -= Time.deltaTime;
        if (hitTimer <= 0f)
        {
            colliderLeftHit = null;
            colliderRightHit = null;
        }

        if (colliderLeftHit && colliderRightHit)
        {
            colliderLeftHit.SetTeam(Team.None);
            colliderRightHit.SetTeam(Team.None);
            player.ActivateNearMissReward();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile collidingProjectile = collision.gameObject.GetComponent<Projectile>();
        if (collidingProjectile != null && collidingProjectile.GetTeam() != Team.Player)
        {
            if (colliders[0].IsTouching(collision))
            {
                colliderLeftHit = collidingProjectile;
            }
            else
            {
                colliderRightHit = collidingProjectile;
            }
            hitTimer = hitMarginSeconds;
        }
    }
}
