using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    Rigidbody2D rb;
    float maxSpeed, accel;

    public void OnInit(float _maxSpeed, float _accel, Rigidbody2D _rb)
    {
        rb = _rb;
        maxSpeed = _maxSpeed;
        accel = _accel;
    }

    public void RunUpdate(ref Vector3 v, float input, float control, float mSpeed)
    {
        if (input == 0) { v.x -= rb.velocity.x; return; }
        if (Mathf.Abs(rb.velocity.x) >= maxSpeed) return;
        if (maxSpeed != mSpeed) { maxSpeed = mSpeed; }
        float m = input * rb.mass * accel * control * Time.deltaTime;
        v.x += m;
    }
}
