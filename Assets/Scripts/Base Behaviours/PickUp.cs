using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp
{
    public void RunOnTriggerEnter(GameObject go, Collider2D col)
    {
        if (col.GetComponent<PlayerController>() != null)
            go.SetActive(false);
    }
}
