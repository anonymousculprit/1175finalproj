using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [Header("Bullet Settings")]
    public int maxBullets;
    public float firingCD;
    public ObjectPool objPool;

    [Header("Follow Settings")]
    public float distance;
    public float speed, maxDistance;

    Rigidbody2D rb;
    CircleCollider2D col;
    GameObject target;

    // Behaviours
    Float Float;
    FollowTarget followTarget;
    FireBullet fireBullet;

    Vector3 force = Vector3.zero;

    private void Start()
    {
        GetComponents();
        InitializeBehaviours();
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

    void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        target = FindObjectOfType<PlayerController>().gameObject;
    }

    void InitializeBehaviours()
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
