using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Range(100, 10000)]
    public float bounceHeight;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject bouncer = collision.gameObject;
        Rigidbody2D rb = bouncer.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * bounceHeight);
    }
}
