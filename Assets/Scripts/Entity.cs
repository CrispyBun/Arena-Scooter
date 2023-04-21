using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    None,
    Player,
    Enemy
}

public class Entity : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private Team team;

    private float bounceAwayForce = 3f;

    protected Rigidbody2D rigidBody;

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public Team GetTeam()
    {
        return team;
    }
    public void SetTeam(Team newTeam)
    {
        team = newTeam;
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void BounceAwayFrom(Transform bounceTarget)
    {
        rigidBody.AddForce((transform.position - bounceTarget.position).normalized * bounceAwayForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity collidingEntity = collision.gameObject.GetComponent<Entity>();
        if (collidingEntity != null)
        {
            BounceAwayFrom(collision.transform);
        }
    }
}
