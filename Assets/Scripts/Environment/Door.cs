using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CapsuleCollider2D))]
public class Door : MonoBehaviour, IInit
{
    [Header("Door")]
    public KeyTypes keyType;
    public float range;
    public Animator keyLoss;

    bool opening = false;
    bool hasKey = false;

    CapsuleCollider2D sensorCol;
    EdgeCollider2D doorCol;
    SpriteRenderer spr;
    Follower follower;
    Animator anim;

    void Start()
    {
        GrabComponents();
    }

    public void DisableDoor() => gameObject.SetActive(false);

    public void GrabComponents()
    {
        sensorCol = GetComponent<CapsuleCollider2D>();
        doorCol = GetComponent<EdgeCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        if (follower == null)
            return;

        Vector3 diff = transform.position - follower.gameObject.transform.position;

        if (diff.magnitude > range || opening)
            return;

        OpenDoor();
    }

    void OpenDoor()
    {
        if (!hasKey)
            return;

        follower.ResetTarget();
        opening = true;
        anim.SetTrigger("open");
        Blackboard.RemoveKey(keyType);
        keyLoss.SetTrigger("itemloss");
        sensorCol.enabled = false;
        doorCol.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (opening)
            return;

        PlayerController p = col.GetComponent<PlayerController>();

        if (p == null)
            return;

        if (Blackboard.HasKey(keyType))
        {
            hasKey = true;
            follower = p.follower;
            follower.target = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        PlayerController p = col.GetComponent<PlayerController>();

        if (p == null)
            return;

        if (follower == null)
            return;

        follower.ResetTarget();
        follower = null;
        hasKey = false;
        opening = false;
    }

    public void InitBehaviours() { }
}
