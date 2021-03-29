using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Sensor : MonoBehaviour, IInit
{


    CircleCollider2D col;
    Collider2D sensedObject;

    // Behaviours
    Flip flip;
    Grab grab;

    private void Start()
    {
        GrabComponents();
        InitBehaviours();
    }

    public void RunUpdate(float hControl, float gControl, ref GrabState state)
    {
        if (flip != null) flip.RunUpdate(transform, hControl, gControl);
        if (grab != null) grab.RunUpdate(ref sensedObject, gControl, ref state);
    }

    public void RunFixedUpdate(float hControl, GrabState state, Vector3 force)
    {
        if (grab != null) grab.RunFixedUpdate(ref sensedObject, hControl, force, state);
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        sensedObject = col;
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        sensedObject = null;
    }

    public void GrabComponents()
    {
        col = GetComponent<CircleCollider2D>();
    }

    public void InitBehaviours()
    {
        flip = new Flip();
        grab = new Grab();

        flip.OnInit(transform.localPosition);
    }
}
