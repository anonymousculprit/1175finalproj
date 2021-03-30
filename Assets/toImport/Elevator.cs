using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
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
        m_bRunning = true;
        m_OriginalPosition = transform.position;
        m_Direction        = RotateBy( transform.up, m_AngleOfMotionDegrees);

        float Distance = m_HalfDistance * (m_StartingPercentage - 0.5f) * 2.0f;
        transform.position = transform.position + m_Direction * Distance;

        m_RB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
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
        var Force = -( m_RB.velocity / Time.fixedDeltaTime + Physics2D.gravity ) * m_RB.mass;

        //
        // Run Logic for the elevator
        //
        float SlowDownK = 1.0f;
        {
            //
            // Logic for the elevator
            //
            var ArrivalPoint = m_OriginalPosition + m_Direction * m_HalfDistance * m_MovingDir;
            if ( Vector3.Dot(transform.position - ArrivalPoint, m_OriginalPosition - ArrivalPoint) < 0 )
            {
                m_SleepTimer = m_WaitInterval;
                m_MovingDir = -m_MovingDir;
                //    Debug.Log("[Elevator] Changing Direction to -1");
            }

            //
            // Arrival/Departure Acceleration 
            //
            {
                // Arriving
                const float SlowDownDistance = 1.0f;
                float distance = (ArrivalPoint - transform.position).magnitude;
                if (distance <= SlowDownDistance)
                {
                    distance = Mathf.Pow( distance, 2 );
                    SlowDownK = (distance < m_SlowDownCutOff) ? m_SlowDownCutOff : distance;
                }
                else
                {
                    float TotalDistance = (m_HalfDistance * 2.0f) - SlowDownDistance;

                    // Departure
                    if (distance >= TotalDistance)
                    {
                        distance = SlowDownDistance - (distance - TotalDistance);
                        distance = Mathf.Pow(distance, 2 );
                        SlowDownK = (distance < m_SlowDownCutOff) ? m_SlowDownCutOff : distance;
                    }
                }
            }

            //
            // Sleep Timer
            //
            if(m_SleepTimer > 0 )
            {
                m_SleepTimer -= Time.fixedDeltaTime;
                SlowDownK = 0;
            }

        }

        //
        // Move the elevator
        //
        Force += ToV2( m_Direction * m_MovingDir * (SlowDownK * m_RB.mass * Time.fixedDeltaTime * m_AccelerationForce) );

        //
        // Add the forces
        //
        m_RB.AddForce( Force );

        // meter per second (velocity) to kilometers per second
        // 1m = 1/1000km
        // 1s = 1/3600h  ==  1 /( 60minutes * 60sec )
        //
        // velocity = 1m / sec
        // velocity = (1/1000) / (1/3600)
        // velocity =  0.001   /  0.00027777...
        // velocity = 3.6
        // velocity(Km) = 3.6 * velocity(m/s)
        // Debug.Log("[Elevator] Speed: " + m_RB.velocity.magnitude * 3.6f );
    }

    void OnDrawGizmos()
    {
        var OriginalPosition = m_OriginalPosition;
        if (!m_bRunning)
        {
            OriginalPosition = transform.position;
            m_Direction      = RotateBy( transform.up, m_AngleOfMotionDegrees);
        }

        Gizmos.DrawLine( OriginalPosition - m_Direction * m_HalfDistance
                       , OriginalPosition + m_Direction * m_HalfDistance );

        var ArrivalPoint = OriginalPosition + RotateBy( transform.up * m_MovingDir * m_HalfDistance , m_AngleOfMotionDegrees );
        Gizmos.DrawSphere(new Vector3( ArrivalPoint.x
                                     , ArrivalPoint.y
                                     , OriginalPosition.z)
                                     , 0.4f );
    }

    private bool        m_bRunning              = false;        // Tell the elevator if we are in editor mode
    private Rigidbody2D m_RB                    = null;
    private Vector3     m_OriginalPosition      = Vector3.zero;
    private float       m_SleepTimer            = 0;            // Timer use to know how long we have been sleeping
    public  float       m_HalfDistance          = 5.0f;         // Half distance to travel for the elevator
    public  float       m_StartingPercentage    = 0.5f;         // Where is the elevator going to start as a percentage of the distance
    public  float       m_AccelerationForce     = 1000.0f;      // how much force to apply to elevator
    public  float       m_MovingDir             = 1;            // Moving Direction
    public  float       m_SlowDownCutOff        = 0.3f;         // Minimum speed in percentage
    public  float       m_WaitInterval          = 1.0f;         // How long the elevator will wait in seconds
    public  float       m_AngleOfMotionDegrees  = 0.0f;
    private Vector3     m_Direction             = Vector3.zero;
}
