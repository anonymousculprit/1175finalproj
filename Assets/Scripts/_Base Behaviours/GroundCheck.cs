using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck 
{
    Vector3 feet;
    LayerMask mask;

    public void OnInit(Vector3 _v)
    {
        feet = _v;
        mask = LayerMask.GetMask("Floor", "Grabbable");
    }

    public void RunUpdate(ref GroundState gState, Vector3 pos)
    {
        Vector3 p = new Vector3(pos.x, pos.y - feet.y);
        RaycastHit2D hit = Physics2D.CircleCast(p, 0.1f, Vector2.down, -0.01f, mask);

        if (hit.collider != null)
            gState = GroundState.GROUND;
        else
            gState = GroundState.AIR;
    }

    public bool IsOnGround(Vector3 pos)
    {
        Vector3 p = new Vector3(pos.x, pos.y - feet.y);
        RaycastHit2D hit = Physics2D.CircleCast(p, 0.1f, Vector2.down, -0.01f, mask);

        if (hit.collider != null)
            return true;
        return false;
    }
}
