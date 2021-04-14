using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatform : MonoBehaviour, IInit
{
    [Header("Variables")]
    public float force;

    ApplyForce applyForce;

    void Start()
    {
        InitBehaviours();
    }

    public void GrabComponents()
    {
        throw new System.NotImplementedException();
    }

    public void InitBehaviours()
    {
        applyForce = new ApplyForce();

        applyForce.OnInit(force);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController p = col.gameObject.GetComponent<PlayerController>();

        if (p == null)
            return;

        applyForce.RunOnCollision(col);
    }
}
