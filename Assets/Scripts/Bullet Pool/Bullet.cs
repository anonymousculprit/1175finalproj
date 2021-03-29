using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    GameObject target;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void OnEnable()
    {
        target = GameObject.FindGameObjectsWithTag("Enemy")[0];
        rb = GetComponent<Rigidbody2D>();

        System.Random random = new System.Random();
        float sx = 10 + (float)(random.NextDouble() * 12);
        float sy = 10 + (float)(random.NextDouble() * 22);

        // Aim towards our target
        float DirX = target.transform.position.x - transform.position.x;
        sx = DirX < 0 ? -sx : sx;

        // Have the bullets come out from the top or the bottom randomly
        if ((random.Next() & 1) != 0) sy = -sy;

        // set our initial velocity
        rb.velocity = new Vector2(sx, sy);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 Dir = target.transform.position - transform.position;
        Vector2 ForceDir = Dir.normalized - rb.velocity.normalized;

        rb.AddForce(ForceDir * (rb.mass * 6200 * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (null != collision.GetComponent<JumpingEnemy>())
            gameObject.SetActive(false);
    }
}
