using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget
{
    Rigidbody2D rb;
    float speed, wSpeed, distance, maxDistance;

    public void OnInit(float _speed, float _distance, float _maxDistance, Rigidbody2D _rb)
    {
        speed = _speed;
        distance = _distance;
        maxDistance = _maxDistance;
        rb = _rb;
        wSpeed = speed;
    }

    public void RunUpdate(ref Vector3 accel, GameObject self, GameObject target)
    {
        var diff = (target.transform.position + new Vector3(0, distance, 0)) - self.transform.position;

        if (diff.magnitude > maxDistance)
            wSpeed = speed * Mathf.Pow((diff.magnitude / maxDistance), maxDistance);
        if (diff.magnitude < distance)
            wSpeed = speed;

        if (Mathf.Abs(diff.x) > 1)
        {
            if (Mathf.Abs(rb.velocity.x) < 3)
                accel += new Vector3((diff.x > 0) ? wSpeed : -wSpeed, 0)
                                * rb.mass * Time.deltaTime;
        }
        if (Mathf.Abs(diff.y) > 0.5f)
        {
            if (Mathf.Abs(rb.velocity.y) < 3)
                accel += new Vector3(0, (diff.y > 0) ? wSpeed * 3 : -wSpeed * 3)
                                * rb.mass * Time.deltaTime;
        }
    }
}
