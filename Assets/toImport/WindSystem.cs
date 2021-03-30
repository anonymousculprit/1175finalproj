using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://github.com/mharrys/fluids-2d
// https://www.youtube.com/watch?v=alhpH6ECFvQ&ab_channel=TheCodingTrain


public class WindSystem : MonoBehaviour
{
    float TurbulenceValue(Vector2 P )
    {
        return Mathf.Cos( (P.y + P.x) * m_Time * 200 ) * 0.5f + 0.5f;
    }

    void UpdateWind( Rigidbody2D RB, Collider2D Collider )
    {
        var Extents = Collider.bounds.extents;
        var Area = Extents.x * Extents.y;

        // The faster you go relative to the ground the stronger is the wind…
        // D = Cd * r * Vw * A * 0.5
        // D = Drag Force, Cd = Drag Coefficient, r = Air density, A = Area of the Surface
        // Vw = WindVelocity - Vector3.Normalize(RigidBody.velocity) * RigidBody.velocity.sqrMagnitud
        var BaseInAirVelocity = m_WindVelocity * TurbulenceValue(RB.position) - RB.velocity.normalized * RB.velocity.sqrMagnitude;

        // BBOX Area = 2*a*b + 2*b*c + 2*c*d
        // Area of a Sphere = 4πr2
        // etc...
        Area *= 0.5f;

        // The Drag  (Drag Coefficient * Air density)
        const float TotalDrag = 1.5f * 100;

        // Return to total Air Force
        var WindForce = BaseInAirVelocity * (Area * TotalDrag) * Time.fixedDeltaTime;// Time.deltaTime;
        RB.AddForceAtPosition(WindForce, Collider.bounds.center );
    }

    void UpdateWind( Rigidbody2D RB )
    {
        var Colliders = RB.GetComponents<Collider2D>();
        if (Colliders == null) return;

        foreach( var e in Colliders)
        {
            UpdateWind(RB, e);
        }
    }

    void UpdateWithFindType()
    {
        var foundPhysicsObjects = FindObjectsOfType<Rigidbody2D>();
        foreach (var RB in foundPhysicsObjects)
        {
            UpdateWind(RB);
        }
    }

    void UpdateWithFindTags()
    {
        var foundPhysicsObjects = GameObject.FindGameObjectsWithTag("Wind");
        foreach (var MyGameObj in foundPhysicsObjects)
        {
            var RB = MyGameObj.GetComponent<Rigidbody2D>();
            if(RB != null) UpdateWind(RB);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_Time += Time.fixedDeltaTime;

        // UpdateWithFindTags();
        UpdateWithFindType();
    }

    public  Vector2 m_WindVelocity = new Vector2(-1000, 0); //new Vector2(-1000, 0); // new Vector2(-1000, 0); //Vector2.zero;
    private float   m_Time         = 0;
}
