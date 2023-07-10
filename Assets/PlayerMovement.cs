using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement _instance;
    PlayerInput playerInput;
    PlayerInput.PlayerMapActions playerInputAction;
    Rigidbody2D rb;
    DelgateAction updateDelegate;
    bool canJump, isJumpPressed, isJumping;
    float endYPosition;
    public float jumpPower;
    public float forwardPower;
    public float changeInY;

    public TextMeshProUGUI text;

    void Awake()
    {
        _instance = this;
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 0;
        playerInput = new();
        playerInputAction = playerInput.PlayerMap;
        life = 3;
    }
    private void OnEnable()
    {
        StartMovement();
    }
    void StartMovement()
    {
        playerInputAction.Enable();
        playerInputAction.Jump.started += Jump;
        playerInputAction.Jump.canceled += JumpCancelled;

        updateDelegate += () => {
            tempTime += Time.deltaTime;

        };
        updateDelegate += MoveForward;


    }
    void MoveForward()
    {
        rb.velocity = Vector2.right * forwardPower;
    }

    private void JumpCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isJumpPressed = false;
    }

    private void Update()
    {
        if (updateDelegate != null)

        {

            updateDelegate();
        }
        text.text = $"Forward Velocity = {rb.velocity.x}\nJump Velocity = {rb.velocity.y}\n";
        // game over if we fall off the map
        if (transform.position.y < -10)
        {
            if (checkpoint && life>0)
            {
                transform.position = lastCheckpointPosition;
                life--;
            }
            else
            {

            GameOver();
            }
        }
    }
    void StopMovement()
    {
        playerInputAction.Disable();

    }
    private void Jump(UnityEngine.InputSystem.InputAction.CallbackContext obj)

    {
        isJumpPressed = true;

        Jump();
    }
    void Jump()
    {
        if (!isJumpPressed)
        {
            return;
        }
        if (!canJump) return;
        endYPosition = transform.position.y + changeInY;
        iTween.RotateBy(transform.GetChild(0).gameObject, iTween.Hash("z", -0.25, "easeType", "Linear", "time", 0.3f, "delay", 0.05));
        isJumping = true;
        updateDelegate -= MoveForward;
        updateDelegate += JumpUp;

        //rb.bodyType = RigidbodyType2D.Kinematic;
        //iTween.MoveBy(gameObject, iTween.Hash("y", 3, "easeType", "Linear", "time", 0.17f, "oncomplete", nameof(NormalGravity)));
        //iTween.RotateBy(transform.GetChild(0).gameObject, iTween.Hash("z", -0.25, "easeType", "Linear", "time", 0.3f,"delay",0.05));

    }
    void JumpUp()
    {


        if (transform.position.y < endYPosition)
        {
            MoveForward();

            rb.velocity += Vector2.up * jumpPower;

        }
        else
        {
            updateDelegate -= JumpUp;
            updateDelegate += Gravity;

        }




    }
    void Gravity()
    {
        updateDelegate -= MoveForward;

        MoveForward();
        rb.velocity += Vector2.down * jumpPower;

    }
    public void JumpDown()
    {



    }
    float tempTime;
    public bool checkpoint;
    public Vector3 lastCheckpointPosition;
    internal int life;

    void NormalGravity()
    {
        tempTime = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            Debug.LogError("check");
            checkpoint = true;
            lastCheckpointPosition = transform.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(tempTime);
        isJumping = false;
        canJump = true;
        updateDelegate -= Gravity;
        updateDelegate += MoveForward;

        if (isJumpPressed)
        {

            Jump();
        }

    }
    // called when the player hits an enemy or falls off the level
    public void GameOver()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canJump = false;
        if (!isJumping)
        {


            updateDelegate += Gravity;


        }
        else
        {
            Jump();
        }

    }

}
delegate void DelgateAction();
