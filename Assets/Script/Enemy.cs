using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Vector2 moveDirection;
    public float moveDistance;

    private Vector2 startPos;
    private bool movingToStart;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // are we moving to the start?
        if (movingToStart)
        {
            // overtime move towards the start position
            transform.position = Vector2.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

            // have we reached our target?
            if (Vector2.Distance(transform.position, startPos) < 0.01f)
            {
                movingToStart = false;
            }
        }
        // are we moving away from the start?
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos + (moveDirection * moveDistance), speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, startPos + (moveDirection * moveDistance)) < 0.01f)
            {
                movingToStart = true;
            }
        }
    }
    ParticleSystem part;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMovement"))
        {
            //part = Instantiate(PlayerMovement._instance.particleSystem, PlayerMovement._instance.transform.position, Quaternion.identity, PlayerMovement._instance.transform);
            //PlayerMovement._instance.isStarted = false;
            Over();
        }
    }
    void Over()
    {
        //Destroy(part);
        if (PlayerMovement._instance.checkpoint && PlayerMovement._instance.life > 0)
        {
            PlayerMovement._instance.transform.position = PlayerMovement._instance.lastCheckpointPosition;
            PlayerMovement._instance.life--;
        }
        else
        {

            PlayerMovement._instance.GameOver();
        }
        //PlayerMovement._instance.isStarted = true;

    }
}
