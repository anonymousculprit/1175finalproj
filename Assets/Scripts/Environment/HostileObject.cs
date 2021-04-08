using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D), typeof(Rigidbody2D))]
public class HostileObject : MonoBehaviour, IInit
{
    [Header("Stats")]
    public int dmg;
    PlayerController p;

    DamageOnCollision dmgOnCollision;

    private void Start()
    {
        GrabComponents();
        InitBehaviours();
    }

    public void GrabComponents()
    {
        p = FindObjectOfType<PlayerController>();
    }

    public void InitBehaviours()
    {
        dmgOnCollision = new DamageOnCollision();

        dmgOnCollision.OnInit(p, dmg);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        dmgOnCollision.RunOnCollision(col);
    }
}
