using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInit
{
    [Header("Door")]
    public KeyTypes keyType;

    CapsuleCollider2D col;

    public void GrabComponents()
    {
        throw new System.NotImplementedException();
    }

    public void InitBehaviours()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController p = col.GetComponent<PlayerController>();

        if (p == null)
            return;

        if (Blackboard.HasKey(keyType))
        {
            p.follower.target = gameObject;
        }
    }

}
