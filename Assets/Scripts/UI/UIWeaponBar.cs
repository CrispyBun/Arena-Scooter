using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWeaponBar : MonoBehaviour
{
    private float progress = 0f;
    void Update()
    {
        float actualProgress = GameManager.GetNewWeaponProgress();
        progress = Mathf.Lerp(progress, actualProgress, 0.25f);
        transform.localScale = new Vector3(progress, 1, 1);
    }

    public void SetProgress(float newProgress)
    {
        progress = newProgress;
    }
}
