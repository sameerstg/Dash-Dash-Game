using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector2D : MonoBehaviour
{
    public float radius;
    public float force;

    // Update is called once per frame
    void Update()
    {
        Vector2 effectorPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(effectorPos, radius);

        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction = rb.position - effectorPos;
                rb.AddForce(direction.normalized * force * Time.deltaTime, ForceMode2D.Impulse);
            }
        }
    }
}
