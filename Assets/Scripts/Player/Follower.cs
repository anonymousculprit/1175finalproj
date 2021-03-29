using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour, IInit
{
    [Header("Bullet Settings")]
    public int maxBullets;
    public float firingCD;
    public ObjectPool objPool;

    [Header("Follow Settings")]
    public float distance;
    public float speed, maxDistance;

    public GameObject target;
    Rigidbody2D rb;
    CircleCollider2D col;

    // Behaviours
    Float Float;
    FollowTarget followTarget;
    FireBullet fireBullet;

    Vector3 force = Vector3.zero;

    private void Start()
    {
        GrabComponents();
        InitBehaviours();
    }

    public void Update()
    {
        if (followTarget != null) followTarget.RunUpdate(ref force, gameObject, target);
        if (fireBullet != null) fireBullet.RunUpdate(transform.position);
    }

    public void FixedUpdate()
    {
        if (Float != null) Float.RunFixedUpdate(ref force);

        rb.AddForce(force);
        force = Vector3.zero;
    }

    public void GrabComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        target = FindObjectOfType<PlayerController>().gameObject;
    }

    public void InitBehaviours()
    {
        Float = new Float();
        followTarget = new FollowTarget();
        fireBullet = new FireBullet();

        Float.OnInit(rb);
        followTarget.OnInit(speed, distance, maxDistance, rb);
        fireBullet.OnInit(maxBullets, firingCD, objPool);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, maxDistance);
    //}
}
