using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Collectable Settings")]
    public CollectableTypes[] types;

    PickUp pickup;

    private void OnTriggerEnter2D(Collider2D col)
    {
        pickup.RunOnTriggerEnter(gameObject, col);
    }

    private void OnDisable()
    {
        // add stuff based on type here
    }
}
