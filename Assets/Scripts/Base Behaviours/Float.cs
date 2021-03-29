using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float
{
    Rigidbody2D rb;

    public void OnInit(Rigidbody2D _rb)
    {
        rb = _rb;
    }

    public void RunFixedUpdate(ref Vector3 v)
    {
        Vector3 f = -(rb.velocity / Time.fixedDeltaTime + Physics2D.gravity) * rb.mass;
        f.x *= 0.02f;

        v += f;
    }
}
