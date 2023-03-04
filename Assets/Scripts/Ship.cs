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

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float thrust = (shipControlThrust ? thrustForce : 0) * Time.fixedDeltaTime;
        float torque = ((shipControlLeft ? 1 : 0) - (shipControlRight ? 1 : 0)) * turnForce * Time.fixedDeltaTime;
        rigidBody.AddRelativeForce(Vector2.right * thrust, ForceMode2D.Force);
        rigidBody.AddTorque(torque, ForceMode2D.Force);
    }
}
