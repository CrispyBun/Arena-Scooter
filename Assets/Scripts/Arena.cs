using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    private Vector2 bounds;
    private float safeBoundWiggleRoom = 5f;

    void Start()
    {
        bounds = transform.lossyScale;
        InBoundKeeper.arena = this;
    }

    public Vector2 GetBounds()
    {
        return bounds;
    }

    public bool PointInBounds(Vector2 point)
    {
        float x2 = bounds.x / 2;
        float x1 = -x2;
        float y2 = bounds.y / 2;
        float y1 = -y2;
        if (point.x < x1) return false;
        if (point.x > x2) return false;
        if (point.y < y1) return false;
        if (point.y > y2) return false;
        return true;
    }

    public bool PointInSafeBounds(Vector2 point)
    {
        float x2 = bounds.x / 2;
        float x1 = -x2;
        float y2 = bounds.y / 2;
        float y1 = -y2;
        if (point.x < x1 - safeBoundWiggleRoom) return false;
        if (point.x > x2 + safeBoundWiggleRoom) return false;
        if (point.y < y1 - safeBoundWiggleRoom) return false;
        if (point.y > y2 + safeBoundWiggleRoom) return false;
        return true;
    }

    public Vector2 GetRandomPointInBounds()
    {
        float x = Random.Range(-bounds.x / 2, bounds.x / 2);
        float y = Random.Range(-bounds.y / 2, bounds.y / 2);
        return new Vector2(x, y);
    }
}
