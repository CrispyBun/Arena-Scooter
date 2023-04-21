using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    [SerializeField] private Team team;

    protected Rigidbody2D rigidBody;

    virtual protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        SetTeam(team);
    }
    public Team GetTeam()
    {
        return team;
    }
    public void SetTeam(Team newTeam)
    {
        team = newTeam;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (team)
        {
            default:
                spriteRenderer.color = Color.white;
                break;

            case Team.Player:
                spriteRenderer.color = new Color(0.6980392f, 0.4941177f, 0.9568628f);
                break;

            case Team.Enemy:
                spriteRenderer.color = new Color(0.8962264f, 0.2916963f, 0.3142128f);
                break;
        }
    }

    public void SetVelocity(Vector2 newVelocity)
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.AddRelativeForce(newVelocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity collidingEntity = collision.gameObject.GetComponent<Entity>();
        if (collidingEntity != null && collidingEntity.GetTeam() != team)
        {
            collidingEntity.Damage(damage);
            Destroy(gameObject);
        }
    }
}
