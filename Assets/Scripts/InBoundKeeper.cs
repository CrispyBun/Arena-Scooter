using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBoundKeeper : MonoBehaviour
{
    public static Arena arena;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (!arena.PointInSafeBounds(transform.position))
        {
            Destroy(gameObject);
        }
    }
}
