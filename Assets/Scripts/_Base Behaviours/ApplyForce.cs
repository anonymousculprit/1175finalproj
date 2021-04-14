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

    public void RunOnce(Collider2D col, Vector2 dir)
    {
        col.attachedRigidbody.AddForce(dir * force);
    }
}
