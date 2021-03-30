using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePlatform : MonoBehaviour
{
    public static Vector3 RotateBy(Vector3 v, float Angle)
    {
        float ar = Mathf.Deg2Rad * Angle;
        float cs = Mathf.Cos(ar);
        float sn = Mathf.Sin(ar);
        return new Vector3(v.x * cs - v.y * sn, v.x * sn + v.y * cs, v.z);
    }

    public static Vector2 ToV2( Vector3 V )
    {
        return new Vector2(V.x, V.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_bRunning          = true;
        m_OriginalPosition  = transform.position;
        m_RB                = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //
        // Don't let platform pass 
        //
        var Distance = Vector3.Dot(transform.position - m_OriginalPosition, -Vector3.up );
        if (Distance > 0 )
        {
            if( Distance > m_Distance )
            {
                m_RB.velocity = Vector2.zero;
                m_RB.position = m_OriginalPosition - (Vector3.up * (m_Distance - 0.005f));
            }
            m_RB.AddForce( -Physics2D.gravity * (m_RB.mass + m_MassToValance) );
        }
        else
        {
            if (Distance < -0.01 )
            {
                m_RB.velocity = Vector2.zero;
                m_RB.position = m_OriginalPosition;
            }
            else
            {
                // F = m * a                      -->
                // a = F/m
                //
                // a = SumOfAllAccelerations....
                // a = F/m + gravity
                //
                // Solving the equation for V1 = 0 (We want to make our velocity zero)
                // V1                   = V0 + a            * dt    --> 
                // 0                    = V0 + (F/m + g)    * dt    -->
                // 0                    = V0 + F/m * dt + g * dt    -->
                // -V0                  =      F/m * dt + g * dt    -->
                // -V0 - g * dt         =      F/m * dt             -->
                // (-V0 - g * dt)/dt    =      F/m                  -->
                //  -V0/dt - g * dt/dt  =      F/m                  -->
                //  -V0/dt - g          =      F/m                  -->
                // (-V0/dt - g) * m     =      F                    -->
                //-( V0/dt + g) * m     =      F                    -->
                //                    F = -( V0/dt + g) * m
                m_RB.AddForce(-(m_RB.velocity / Time.fixedDeltaTime + Physics2D.gravity) * m_RB.mass );
            }
        }
    }

    void OnDrawGizmos()
    {
        var OriginalPosition = m_OriginalPosition;
        if (!m_bRunning)
        {
            OriginalPosition = transform.position;
        }

        Gizmos.DrawLine( OriginalPosition - Vector3.up * m_Distance
                       , OriginalPosition );
    }

    private bool        m_bRunning              = false;        // Tell the elevator if we are in editor mode
    private Rigidbody2D m_RB                    = null;
    private Vector3     m_OriginalPosition      = Vector3.zero;
    public  float       m_Distance              = 0.5f;         // Half distance to travel for the elevator
    public  float       m_MassToValance         = 70.0f;      // how much force to apply to elevator
}
