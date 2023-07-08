using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    public int value;
    [Header("Bobbing")]
    public float rotateSpeed;
    public float bobSpeed;
    public float bobHeight;

    private Vector2 startPos;
    private bool bobbingUp;

    // Start is called before the first frame update
    void Start()
    {
        // set the start position
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // rotating
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

        // bob up and down
        Vector2 offset = (bobbingUp == true ? new Vector2(0, bobHeight / 2) : new Vector2(0, -bobHeight / 2));

        transform.position = Vector2.MoveTowards(transform.position, startPos + offset, bobSpeed * Time.deltaTime);

        
    }
}
