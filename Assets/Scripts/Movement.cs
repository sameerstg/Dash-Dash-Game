using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{



    public enum Speeds { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4 };
    public Speeds CurrentSpeed;
    //                       0      1      2       3      4
    float[] SpeedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };

    public Transform GroundCheckTransform,forwardCheckTransform;
    public float GroundCheckRadius;
    public LayerMask GroundMask;
    public float jumpLimit,
    jumpDuration = 0.1f;
    private bool isJumping = false,gravity, isOnGround;
    PlayerInput playerInput;
    public GameObject body;
    private void Awake()
    {
        playerInput = new();
    }

    private void Start()
    {
        StartCoroutine(GravityRoutine());
    }
    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.PlayerMap.Jump.started += Jump;
    }

    private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (isOnGround)
        {
            StartCoroutine(JumpRoutine());
        }
    }
   
    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.PlayerMap.Jump.started -= Jump;

    }


    IEnumerator JumpRoutine()
    {
        //Debug.Log("jump");
        isJumping = true;
        float curY = transform.position.y;
        Vector3 curRotation = body.transform.eulerAngles,destRotation = body.transform.eulerAngles+Vector3.back*90;
        float time = 0;
        //while (transform.position.y <curY+jumpLimit)
        //{
        //    //body.transform.eulerAngles = Vector3.Lerp(curRotation, destRotation,  curY + jumpLimit/ body.transform.position.y );
        //    if (transform.position.y +  jumpLimit * Time.deltaTime > curY+jumpLimit)
        //    {
        //        transform.position += Vector3.up *(curY+jumpLimit);
        //        break;

        //    }
        //    else
        //    {
        //        transform.position += Vector3.up * jumpLimit * Time.deltaTime;

        //    }
        //    yield return null;
        //}
        while (time<=jumpDuration)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(curY, curY + jumpLimit, time / jumpDuration),transform.position.z);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, curY + jumpLimit, transform.position.z);

        //transform.position += Vector3.up *(curY+jumpLimit);

        body.transform.eulerAngles = destRotation;
        isJumping = false;
        //Debug.Log(transform.position.y - curY);
       
        //StartCoroutine(GravityRoutine());
    }
    IEnumerator GravityRoutine()
    {
        while (isJumping)
        {
            yield return null;
        }
        gravity = true;

        do
        {
            transform.position += Vector3.down * jumpLimit/jumpDuration * Time.deltaTime;
            yield return null;
        } while (!isOnGround);
        gravity = false;

    }
    void Update()
    {
        transform.position += Vector3.right * SpeedValues[(int)CurrentSpeed] * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Hurdle"))
        {
            Hit();
        }
       
       
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (ForwardHit())
        {
            Hit();
        }
        else
        {
            transform.position = new Vector3(transform.position.x, collision.contacts[0].point.y, transform.position.z);
            if (transform.eulerAngles.z % 90 != 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z % 90);
            }
            isOnGround = true;
            if (playerInput.PlayerMap.Jump.inProgress && !isJumping)
            {
                StartCoroutine(JumpRoutine());
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
            isOnGround = false;
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(GravityRoutine());

        }
    }
    void Hit()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(0);

    }
   
    bool ForwardHit()
    {
       
        return Physics2D.OverlapCircle(forwardCheckTransform.position, GroundCheckRadius,GroundMask);

    }

}
