using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump
{
    Rigidbody2D rb;
    float maxJump, jumpBoost, jTimer;
    int maxCount, count;
    bool isEnemy, newTap;

    public void OnInit(float _maxJump, float _jumpBoost, Rigidbody2D _rb, bool _enemy = true, int _maxCount = 0)
    {
        rb = _rb;
        maxJump = _maxJump;
        jumpBoost = _jumpBoost;
        isEnemy = _enemy;
        maxCount = _maxCount;
    }

    public void RunUpdate(ref Vector3 v, float control, GroundState state)
    {
        if (state == GroundState.GROUND)
            count = 0;

        if (control != 0)
        {
            if (count >= maxCount)
                return;

            if (jTimer == 0 && state == GroundState.GROUND)
            {
                v.y += control * rb.mass * jumpBoost * 0.3f;
                jTimer += Mathf.Epsilon;
                count++;
                newTap = false;
            }
            if (state == GroundState.AIR && count < maxCount && newTap)
            {
                v.y += control * rb.mass * jumpBoost * 0.5f;
                jTimer = Mathf.Epsilon;
                count++;
                newTap = false;
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
            newTap = true;
        }
    }
}
