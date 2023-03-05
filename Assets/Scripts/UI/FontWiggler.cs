using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FontWiggler : MonoBehaviour
{
    private RectTransform textPos;
    private float originX;
    private float originY;

    private float swayAmount = 5f;
    private float animSpeed = 6;

    // Start is called before the first frame update
    void Start()
    {
        textPos = GetComponent<RectTransform>();
        if (!textPos) Debug.LogError("No RectTransform component. Script won't work.");

        originX = textPos.anchoredPosition.x;
        originY = textPos.anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        float animProgression = Mathf.Sin(Time.unscaledTime * animSpeed);
        float sway = animProgression * swayAmount;
        textPos.anchoredPosition = new Vector2(originX - sway, originY + sway);        
    }
}
