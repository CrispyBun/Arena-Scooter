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
                    Destroy(gameObject);
                }
                break;

            case InBoundKeepType.Bounce:
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
