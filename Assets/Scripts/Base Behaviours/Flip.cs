using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip
{
    Vector3 pos;

    public void OnInit(Vector3 _pos)
    {
        Debug.Log("pos: " + _pos);
        pos = _pos;
    }

    public void RunUpdate(Transform transform, float control, float gControl)
    {
        if (gControl != 0)
            return;

        if (control > 0)
            transform.localPosition = pos;
        if (control < 0)
            transform.localPosition = -pos;
    }
}
