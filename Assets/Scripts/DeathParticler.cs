using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticler : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticleSystem;
    private float duration;
    private float durationTimer = 0_0;
    private bool timerDisabled = false;

    private void Start()
    {
        duration = deathParticleSystem.main.startLifetime.constantMax;
    }

    void Update()
    {
        durationTimer += Time.deltaTime;
        if (durationTimer >= duration && !timerDisabled)
        {
            Destroy(gameObject);
        }
    }

    public void DisableTimer()
    {
        timerDisabled = true;
    }

    public void DetachParent()
    {
        transform.parent = null;
        durationTimer = 0_0;
        timerDisabled = false;
    }
}
