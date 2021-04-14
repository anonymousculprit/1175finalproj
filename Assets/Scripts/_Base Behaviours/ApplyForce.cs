using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce
{
    float force;

    public void OnInit(float _force)
    {
        force = _force;
    }

    public void RunOnCollision(Collider2D col)
    {
        Debug.Log("Applying Force.");
        col.attachedRigidbody.AddForce(Vector2.up * force);
    }
}
