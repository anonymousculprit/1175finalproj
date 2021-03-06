using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour, IInit, IDamagable
{
    [Header("References")]
    public GameObject spriteGO;

    [Header("Stats")]
    public int hp;

    [Header("Move Variables")]
    public float maxSpeed;
    public float accel;

    [Header("Jump Variables")]
    public float maxJump;
    public float jumpBoost;
    public int maxExtraJumps;
    public bool wallJump;

    [Header("State Variables")]
    [SerializeField] GroundStatePair[] groundStateSettings;
    [SerializeField] GrabStatePair[] grabStateSettings;

    GroundState gState = GroundState.NULL;
    GrabState pState = GrabState.NULL;

    float control = 1f;
    bool facingWall = false;

    static int flip = 1;
    public static int Flip { get => flip; }

    static PlayerController player;
    public static PlayerController Player { get => player; }

    // components
    Rigidbody2D rb;
    CapsuleCollider2D col;
    SpriteRenderer spr;
    Animator anim;

    [HideInInspector] public Follower follower;

    // behaviours
    Move move;
    Jump jump;
    GroundCheck gc;
    Sensor sensor;
    FlipSprite flipState;
    Animation animate;
    /* - Animator
     * - WallJump
     * - Following Object
     * - Grabbable
     */
     

    Vector3 force = new Vector3(0,0,0);

    private void Start()
    {
        player = this;
        GrabComponents();
        InitBehaviours();
    }

    private void Update()
    {
        control = GetControl();
        maxSpeed = GetMaxSpeed();

        move.RunUpdate(ref force, Input.GetAxisRaw("Horizontal"), control, maxSpeed);
        jump.RunUpdate(ref force, Input.GetAxisRaw("Jump"), gState, facingWall);
        gc.RunUpdate(ref gState, transform.position);
        animate.RunUpdate_Player(gState, Input.GetAxisRaw("Horizontal"));
        sensor.RunUpdate(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Fire2"), ref pState, ref facingWall);
        flipState.RunUpdate(Input.GetAxisRaw("Horizontal"), ref flip);
    }

    private void FixedUpdate()
    {
        sensor.RunFixedUpdate(Input.GetAxisRaw("Horizontal"), pState, force);

        rb.AddForce(force);
        force = Vector3.zero;
    }

    Vector3 CalculateFeet() => col.size / 2;
    float GetControl()
    {
        float c = 0f;

        for (int i = 0; i < groundStateSettings.Length; i++)
            if (groundStateSettings[i].state == gState)
                c = groundStateSettings[i].control;

        for (int i = 0; i < grabStateSettings.Length; i++)
            if (grabStateSettings[i].state == pState)
                if (grabStateSettings[i].control < c)
                    c = grabStateSettings[i].control;

        return c;
    }

    float GetMaxSpeed()
    {
        float s = 0f;

        for (int i = 0; i < grabStateSettings.Length; i++)
            if (grabStateSettings[i].state == pState)
                s = grabStateSettings[i].maxSpeed;

        return s;
    }

    public void GrabComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        anim = spriteGO.GetComponent<Animator>();
        spr = spriteGO.GetComponent<SpriteRenderer>();
        sensor = transform.parent.GetComponentInChildren<Sensor>();
        follower = GetComponentInChildren<Follower>();
    }

    public void InitBehaviours()
    {
        move = new Move();
        jump = new Jump();
        gc = new GroundCheck();
        flipState = new FlipSprite();
        animate = new Animation();

        move.OnInit(maxSpeed, accel, rb);
        jump.OnInit(maxJump, jumpBoost, rb, false, maxExtraJumps, wallJump);
        gc.OnInit(CalculateFeet());
        flipState.OnInit(spr);
        animate.OnInit(anim, true, true);
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
    }

    [Serializable]
    public struct GroundStatePair
    {
        public GroundState state;
        public float control;
    }

    [Serializable]
    public struct GrabStatePair
    {
        public GrabState state;
        public float control, maxSpeed;
    }
}
