using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp
{
    public void RunOnTriggerEnter(Collider2D col, System.Action action)
    {
        if (col.GetComponent<PlayerController>() != null)
        {
            action?.Invoke();
        }
    }
}
