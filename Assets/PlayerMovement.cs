using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerInput.PlayerMapActions playerInputAction;
    Rigidbody2D rb;
    DelgateAction updateDelegate;
    bool canJump,isJumpPressed,isJumping;
    float endYPosition;
    public float jumpPower;
    public float forwardPower;
    public float changeInY;

    public TextMeshProUGUI text;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 0;
        playerInput = new();
        playerInputAction = playerInput.PlayerMap;
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
        
        updateDelegate +=()=> { tempTime += Time.deltaTime;
            
        };
        updateDelegate +=MoveForward;

      
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
        if (updateDelegate!=null)

        {

            updateDelegate();
        }
        text.text = $"Forward Velocity = {rb.velocity.x}\nJump Velocity = {rb.velocity.y}\n";
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
        if (!canJump)return;
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


        if (transform.position.y < endYPosition) { 
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
    void NormalGravity()
    {
        tempTime = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
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
