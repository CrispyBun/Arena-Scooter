using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    protected Rigidbody2D rigidBody;

    virtual protected void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity collidingEntity = collision.gameObject.GetComponent<Entity>();
        if (collidingEntity != null)
        {
            collidingEntity.Damage(damage);
            Destroy(gameObject);
        }
    }
}
