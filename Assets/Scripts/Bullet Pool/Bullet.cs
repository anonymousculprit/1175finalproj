using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    public int dmg = 1;
    public float radius, minDist;

    float cTime = 0f;

    GameObject target;
    Rigidbody2D rb;
    Vector3 endPos;

    void OnEnable() => SetupBullet();
    void Update() => MoveTowardsTarget();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        JumpingEnemy t = collision.GetComponent<JumpingEnemy>();

        if (t != null)
        {
            t.Damage(dmg);
            gameObject.SetActive(false);
        }
    }


    bool GetClosestTarget(Vector3 spawnPosition, ref GameObject target)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

        if (targets == null || targets.Length == 0)
            return false;

        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].activeInHierarchy)
                continue;

            float dist = Vector3.Distance(targets[i].transform.position, spawnPosition);

            if (dist > radius)
                continue;

            target = targets[i];
            return true;
        }

        return false;
    }
    void MoveTowardsTarget()
    {
        Debug.Log(cTime);

        if (target != null)
        {
            if (!target.activeInHierarchy)
                gameObject.SetActive(false);

            endPos = target.transform.position;
        }

        Vector2 Dir = endPos - transform.position;
        Vector2 ForceDir = Dir.normalized - rb.velocity.normalized;

        rb.AddForce(ForceDir * (rb.mass * 6200 * Time.deltaTime));

            if (target == null)
                if (Dir.magnitude < minDist)
                    gameObject.SetActive(false);
    }
    void SetupBullet()
    {
        rb = GetComponent<Rigidbody2D>();

        target = null;

        System.Random random = new System.Random();
        float sx = 10 + (float)(random.NextDouble() * 12);
        float sy = 10 + (float)(random.NextDouble() * 22);

        float DirX = 0f;

        if (GetClosestTarget(transform.position, ref target))
            DirX = target.transform.position.x - transform.position.x;
        else
        {
            DirX = radius * PlayerController.Flip;
            endPos = new Vector3(transform.position.x + DirX, transform.position.y, transform.position.z);
            Debug.Log(endPos);
        }

        // Aim towards our target
        sx = DirX < 0 ? -sx : sx;

        // Have the bullets come out from the top or the bottom randomly
        if ((random.Next() & 1) != 0) sy = -sy;

        // set our initial velocity
        rb.velocity = new Vector2(sx, sy);
    }
}
