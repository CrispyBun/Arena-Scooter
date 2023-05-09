using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Entity
{
    protected bool shipControlThrust;
    protected bool shipControlLeft;
    protected bool shipControlRight;

    [SerializeField] private float thrustForce = 1000;
    [SerializeField] private float turnForce = 2500;

    [SerializeField] private ParticleSystem thrustParticleSystem;
    private DeathParticler thrustParticleSystemClass;

    [SerializeField] private AudioSource thrustAudioSource;
    [SerializeField] private AudioSource thrustStopAudioSource;

    protected override void Start()
    {
        base.Start();
        if (thrustParticleSystem)
        {
            thrustParticleSystem.Stop();
            thrustParticleSystemClass = thrustParticleSystem.GetComponent<DeathParticler>();
            thrustParticleSystemClass.DisableTimer();
        }
    }

    public override void DestroySelf(bool noParticles = false)
    {
        if (thrustParticleSystem)
        {
            thrustParticleSystem.Stop();
            thrustParticleSystemClass.DetachParent();
            base.DestroySelf(noParticles);
        }
    }

    private void FixedUpdate()
    {
        float thrust = (shipControlThrust ? thrustForce : 0) * Time.fixedDeltaTime;
        float torque = ((shipControlLeft ? 1 : 0) - (shipControlRight ? 1 : 0)) * turnForce * Time.fixedDeltaTime;
        rigidBody.AddRelativeForce(Vector2.right * thrust, ForceMode2D.Force);
        rigidBody.AddTorque(torque, ForceMode2D.Force);

        if (shipControlThrust)
        {
            if (!thrustParticleSystem.isEmitting)
            {
                thrustParticleSystem.Play();
            }

            if (this is ShipPlayer)
            {
                if (!thrustAudioSource.isPlaying)
                {
                    thrustAudioSource.Play();
                }
            }
        }
        else
        {
            thrustParticleSystem.Stop();

            if (this is ShipPlayer)
            {
                if (thrustAudioSource.isPlaying)
                {
                    thrustAudioSource.Stop();
                    thrustStopAudioSource.Play();
                }
            }
        }
    }
}
