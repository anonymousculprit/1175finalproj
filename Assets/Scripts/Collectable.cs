using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof (CircleCollider2D))]
public class Collectable : MonoBehaviour, IInit
{
    [Header("Collectable Settings")]
    public CollectableTypes[] types;
    public KeyTypes type;

    PickUp pickup;

    private void OnTriggerEnter2D(Collider2D col)
    {
        pickup.RunOnTriggerEnter(gameObject, col);
    }

    private void Start()
    {
        InitBehaviours();
    }



    private void OnDisable()
    {
        PerformCollectableAction();
    }

    void PerformCollectableAction()
    {
        for(int i = 0; i < types.Length; i++)
        {
            switch (types[i])
            {
                case CollectableTypes.KEY: Blackboard.SetKey(type); break;
                default: break;
            }
        }
    }


    public void InitBehaviours()
    {
        pickup = new PickUp();
    }

    public void GrabComponents() { }
}
