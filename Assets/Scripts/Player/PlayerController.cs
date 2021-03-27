using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Move Variables")]
    public float maxSpeed;
    public float accel;

    [Header("Jump Variables")]
    public float maxJump;
    public float jumpBoost;

    [Header("Ground State Variables")]
    [SerializeField] GroundStatePair[] gStateSettings;

    GroundState gState = GroundState.NULL;

    // components
    Rigidbody2D rb;
    CapsuleCollider2D col;
    SpriteRenderer spr;
    Animator anim;

    // behaviours
    Move move;
    Jump jump;
    GroundCheck gc;
    /* - Animator
     * - WallJump
     * - Following Object
     * - Grabbable
     */

    Vector3 force = new Vector3(0,0,0);

    private void Start()
    {
        GrabComponents();
        InitBehaviours();
    }

    private void Update()
    {
        move.RunUpdate(ref force, Input.GetAxisRaw("Horizontal"), GetControl(gState));
        jump.RunUpdate(ref force, Input.GetAxisRaw("Jump"), gState);
        gc.RunUpdate(ref gState, transform.position);
    }

    private void FixedUpdate()
    {
        rb.AddForce(force);
        force = Vector3.zero;
    }

    void GrabComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void InitBehaviours() // see if we can genericize this in the future
    {
        move = new Move();
        jump = new Jump();
        gc = new GroundCheck();

        move.OnInit(maxSpeed, accel, rb);
        jump.OnInit(maxJump, jumpBoost, rb, false);
        gc.OnInit(CalculateFeet());
    }

    Vector3 CalculateFeet() => col.size / 2;
    float GetControl(GroundState gState)
    {
        for (int i = 0; i < gStateSettings.Length; i++)
            if (gStateSettings[i].state == gState)
                return gStateSettings[i].control;
        return 0;
    }

    [Serializable]
    public struct GroundStatePair
    {
        public GroundState state;
        public float control;
    }
}
