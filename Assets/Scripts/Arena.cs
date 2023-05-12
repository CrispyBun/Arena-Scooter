using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    private Vector2 bounds;
    private float safeBoundWiggleRoom = 5f;

    void Awake()
    {
        bounds = transform.lossyScale;
        InBoundKeeper.arena = this;
    }

    public Vector2 GetBounds()
    {
        return bounds;
    }
    public float GetSafeBoundWiggleRoom()
    {
        return safeBoundWiggleRoom;
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

    public Vector2 GetRandomPointInArenaCenter()
    {
        float x = Random.Range(-bounds.x / 3, bounds.x / 3);
        float y = Random.Range(-bounds.y / 3, bounds.y / 3);
        return new Vector2(x, y);
    }

    public Vector2 GetRandomPointFromSide()
    {
        int side = Mathf.RoundToInt(Mathf.Sign(Random.value - 0.5f));
        float length = Random.value - 0.5f;
        return new Vector2(side * ((bounds.x + safeBoundWiggleRoom * 2) / 2), length * (bounds.y + safeBoundWiggleRoom * 2));
    }

    public Vector2 GetRandomPointInOuterSafeBounds()
    {
        int axis = Mathf.RoundToInt(Random.value);
        int side = Mathf.RoundToInt(Mathf.Sign(Random.value - 0.5f));
        float length = Random.value - 0.5f;
        if (axis == 0)
        {
            return new Vector2(length * (bounds.x + safeBoundWiggleRoom*2), side * ((bounds.y + safeBoundWiggleRoom*2) / 2));
        }
        return new Vector2(side * ((bounds.x+ safeBoundWiggleRoom*2) / 2), length * (bounds.y + safeBoundWiggleRoom*2));
    }
}
