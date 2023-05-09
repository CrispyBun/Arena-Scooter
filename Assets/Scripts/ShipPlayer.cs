using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlayer : Ship
{
    private bool nearMissRewardActive = false;
    private float nearMissRewardTimeSeconds = 1f;
    private float nearMissRewardTimer = 0f;
    private float nearMissRewardRadius = 20f;

    [SerializeField] private AudioSource audioNearMissChance;
    [SerializeField] private AudioSource audioNearMissFail;
    [SerializeField] private AudioSource audioNearMissSuccess;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        shipControlThrust = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
        shipControlLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        shipControlRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        nearMissRewardTimer -= Time.deltaTime;
        if (nearMissRewardTimer <= 0f)
        {
            if (nearMissRewardActive)
            {
                Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, nearMissRewardRadius);
                for (int i = 0; i < collisions.Length; i++)
                {
                    Projectile projectile = collisions[i].gameObject.GetComponent<Projectile>();
                    if (projectile != null)
                    {
                        projectile.SetTeam(Team.Player);
                        projectile.SetVelocity(Vector2.up * 50f);
                    }
                }

                audioNearMissSuccess.Play();
            }
            nearMissRewardActive = false;
        }
    }

    public void ActivateNearMissReward()
    {
        if (!nearMissRewardActive) audioNearMissChance.Play();

        nearMissRewardActive = true;
        nearMissRewardTimer = nearMissRewardTimeSeconds;
    }
    public void DisableNearMissReward()
    {
        if (nearMissRewardActive) audioNearMissFail.Play();

        nearMissRewardActive = false;
    }
}
