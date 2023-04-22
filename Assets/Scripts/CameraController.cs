using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float viewWidth;
    float viewHeight;
    
    [SerializeField] private GameObject focusedObject;
    [SerializeField] private Arena arena;

    void Start()
    {
        viewHeight = Camera.main.orthographicSize * 2f;
        viewWidth = viewHeight * Camera.main.aspect;
    }

    void FixedUpdate()
    {
        if (focusedObject)
        {
            Vector2 objectPos = focusedObject.transform.position;
            Vector2 arenaBounds = arena.GetBounds();
            arenaBounds /= 2;

            float cameraMovementX = Mathf.Min(Mathf.Abs(objectPos.x) / arenaBounds.x, 1f);
            float cameraMovementY = Mathf.Min(Mathf.Abs(objectPos.y) / arenaBounds.y, 1f);

            transform.position = new Vector3(cameraMovementX * Mathf.Sign(objectPos.x) * (arenaBounds.x - viewWidth/2), cameraMovementY * Mathf.Sign(objectPos.y) * (arenaBounds.y - viewHeight / 2), -10);
        }
    }
}
