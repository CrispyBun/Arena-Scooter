using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    protected Rigidbody2D rigidBody;

    virtual protected void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
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
