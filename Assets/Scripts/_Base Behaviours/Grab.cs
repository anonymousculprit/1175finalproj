using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// divide grab to push/pull behaviours?
public class Grab
{
    public void RunUpdate(ref Collider2D col, float control, ref GrabState state)
    {
        if (col == null)
        {
            state = GrabState.NULL;
            return;
        }

        if (col.tag == "Grabbable" && control != 0)
        {
            state = GrabState.GRAB;
        }
        if (col.tag == "Grabbable" && control == 0)
        {
            col.attachedRigidbody.AddForce(-col.attachedRigidbody.velocity);
            state = GrabState.NULL;
        }
    }

    public void RunFixedUpdate(ref Collider2D col, float hControl, Vector3 force, GrabState state)
    {
        if (col == null)
            return;

        if (state == GrabState.GRAB && hControl != 0)
        {
            col.attachedRigidbody.AddForce(force);
        }
    }
}
