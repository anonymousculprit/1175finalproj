using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof (CircleCollider2D))]
public class Collectable : MonoBehaviour, IInit
{
    [Header("Collectable Settings")]
    public CollectableTypes[] types;
    public KeyTypes keyType;
    public float expirationTime = 0;
    public Rigidbody2D rb;
    public Collider2D col;

    [Header("Burst Settings")]
    public bool burst;
    public float burstForce = 10f;
    
    [Header("Drawn To Player Settings")]
    public bool isDrawnToPlayer = false;
    public float range, drawForce;

    [Header("Unique Item")]
    public bool unique = false;
    public Animator anim;

    PickUp pickup;
    ApplyForce applyForce;
    DrawnToPlayer dToPlayer;

    Vector3 force = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D col)
    {
        pickup.RunOnTriggerEnter(col, PerformCollectableAction);
    }

    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.gameObject.tag == "Collectable")
            Physics2D.IgnoreCollision(_col.collider, col, true);
    }
    private void OnCollisionStay2D(Collision2D _col)
    {
        if (_col.gameObject.tag == "Collectable")
            Physics2D.IgnoreCollision(_col.collider, col, true);
    }

    private void Start()
    {
        InitBehaviours();
    }

    private void OnEnable()
    {
        InitBehaviours();

        if (expirationTime > 0)
            StartCoroutine(WaitToExpire(expirationTime));

        if (applyForce != null)
            applyForce.RunOnce(col, Vector2.up);
    }

    private void OnDisable()
    {
        //PerformCollectableAction();
    }

    private void FixedUpdate()
    {
        if (dToPlayer != null) dToPlayer.RunFixedUpdate(gameObject, ref force);

        if (rb != null)
            rb.AddForce(force);

        force = Vector3.zero;
    }

    void PerformCollectableAction()
    {
        StopAllCoroutines();

        for(int i = 0; i < types.Length; i++)
        {
            switch (types[i])
            {
                case CollectableTypes.KEY: Blackboard.SetKey(keyType); break;
                default: break;
            }
        }

        if (unique)
            anim.SetTrigger("itemget");

        gameObject.SetActive(false);
    }

    IEnumerator WaitToExpire(float expTime)
    {
        yield return new WaitForSeconds(expTime);
        Destroy(gameObject);
    }

    public void InitBehaviours()
    {
        pickup = new PickUp();
        if (isDrawnToPlayer) dToPlayer = new DrawnToPlayer();
        if (burst) applyForce = new ApplyForce();

        if (dToPlayer != null) dToPlayer.OnInit(PlayerController.Player, range, drawForce);
        if (applyForce != null) applyForce.OnInit(burstForce);
    }

    public void GrabComponents()
    {
        throw new System.NotImplementedException();
    }
}
