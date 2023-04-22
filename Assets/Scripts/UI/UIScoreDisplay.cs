using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;

    private float scale = 1f;
    private float scaleIncrease = 0.25f;
    private float scaleFalloff = 0.01f;
    private float scaleMax = 2f;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        scale = Mathf.Max(scale - scaleFalloff * Time.deltaTime * 60f, 1f);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void UpdateScore(int newScore)
    {
        text.text = newScore.ToString();
        scale = Mathf.Min(scale + scaleIncrease, scaleMax);
    }
}
