using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump
{
    Rigidbody2D rb;
    float maxJump, jumpBoost, jTimer;
    bool isEnemy;

    public void OnInit(float _maxJump, float _jumpBoost, Rigidbody2D _rb, bool _enemy)
    {
        rb = _rb;
        maxJump = _maxJump;
        jumpBoost = _jumpBoost;
        isEnemy = _enemy;
    }

    public void RunUpdate(ref Vector3 v, float control, GroundState state)
    {
        if (control != 0)
        {
            if (jTimer == 0 && state == GroundState.GROUND)
            {
                v.y += control * rb.mass * jumpBoost * 0.3f;
                jTimer += Mathf.Epsilon;
            }
            if (jTimer != 0)
            {
                float t = jTimer + Time.deltaTime;

                if (jTimer >= maxJump)
                {
                    if (isEnemy && state == GroundState.GROUND)
                    {
                        jTimer = 0;
                        return;
                    }
                    
                    t = maxJump - jTimer;
                    jTimer = maxJump;
                }
                else
                {
                    jTimer = t;
                    t = Time.deltaTime;
                }

                float m = control * rb.mass * jumpBoost * t; // you'd put in movement control here if need be
                v.y += m;
            }
        }
        else
        {
            jTimer = 0;
        }
    }
}
