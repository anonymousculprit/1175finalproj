using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInit
{
    [Header("Door")]
    public KeyTypes keyType;
    public float range;
    
    bool opening = false;

    CapsuleCollider2D col;
    Follower follower;

    void Start()
    {
        GrabComponents();
    }

    public void GrabComponents()
    {
        col = GetComponent<CapsuleCollider2D>();
    }

    public void InitBehaviours()
    {

    }

    public void Update()
    {
        Vector3 diff = transform.position - follower.gameObject.transform.position;

        if (diff.magnitude > range || opening)
            return;

        OpenDoor();
    }

    void OpenDoor()
    {
        opening = true;
        // open door
        // when finished
        Blackboard.RemoveKey(keyType);
        col.enabled = false;
        enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController p = col.GetComponent<PlayerController>();

        if (p == null)
            return;

        if (Blackboard.HasKey(keyType))
        {
            follower = p.follower;
            follower.target = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        PlayerController p = col.GetComponent<PlayerController>();

        if (p == null)
            return;

        follower.ResetTarget();
        follower = null;
    }

}
