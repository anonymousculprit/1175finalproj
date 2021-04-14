using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck
{
    public void RunUpdate(ref bool facingWall, Collider2D col)
    {
        if (col == null)
        {
            facingWall = false;
            return;
        }
            

        if (col.gameObject.tag == "Wall")
            facingWall = true;
        else
            facingWall = false;

    }
}
