﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class JumpingEnemy : Enemy, IDamagable
{
    [Header("Stats")]
    public int hp;
    public int dmg;

    [Header("Move Variables")]
    public float maxSpeed;
    public float accel;

    [Header("Jump Variables")]
    public float maxJump;
    public float jumpBoost;

    GroundState gState = GroundState.NULL;

    int isMoving, isJumping;
    bool alive = true;

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

    Vector3 force = new Vector3(0, 0, 0);

    private void Start()
    {
        GrabComponents();
        InitBehaviours();
        StartCoroutine(EnemyBehaviour());
    }

    private void Update()
    {
        if (move != null) move.RunUpdate(ref force, isMoving, 1, maxSpeed);
        if (jump != null) jump.RunUpdate(ref force, isJumping, gState);
        if (gc != null) gc.RunUpdate(ref gState, transform.position);
    }

    private void FixedUpdate()
    {
        if (force == Vector3.zero)
            return;

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
        foreach (EnemyMovement b in behaviours)
            switch (b)
            {
                case EnemyMovement.MOVE: move = new Move(); move.OnInit(maxSpeed, accel, rb); break;
                case EnemyMovement.JUMP:
                    gc = new GroundCheck(); gc.OnInit(CalculateFeet());
                    jump = new Jump(); jump.OnInit(maxJump, jumpBoost, rb, true); break;
                default: break;
            }
    }

    Vector3 CalculateFeet() => (col.size / 2) * transform.localScale.y;

    protected override IEnumerator EnemyBehaviour()
    {
        while (alive)
        {
            EnemyMovement state = behaviours[Random.Range(0, behaviours.Length)];
            yield return new WaitForSeconds(ControlEnemy(state));
            yield return null;
        }
    }

    protected override float ControlEnemy(EnemyMovement state)
    {
        isMoving = 0;
        isJumping = 0;

        switch (state)
        {
            case EnemyMovement.MOVEJUMP: isMoving = LeftOrRight(); isJumping = 1; break;
            case EnemyMovement.JUMP: isJumping = 1; break;
            case EnemyMovement.MOVE: isMoving = LeftOrRight(); break;
            case EnemyMovement.NULL:
            default: break;
        }
        return Random.Range(minTime, maxTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        PlayerController p = col.gameObject.GetComponent<PlayerController>();
        if (p != null) p.Damage(dmg);
    }

    public void Damage(int dmg)
    {
        hp -= dmg;
        CheckForDeath();
    }

    public void CheckForDeath()
    {
        if (hp > 0)
            return;

        Debug.Log("im dead!");
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(new Vector3(rb.position.x, rb.position.y - (CalculateFeet().y * transform.localScale.y) - 0.01f), 0.1f);
    //}
}
