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

    protected Rigidbody2D rigidBody;

    virtual protected void Start()
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
}
