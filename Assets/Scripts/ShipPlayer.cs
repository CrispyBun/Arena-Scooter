using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlayer : Ship
{
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        shipControlThrust = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
        shipControlLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        shipControlRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }
}
