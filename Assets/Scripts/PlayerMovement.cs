using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This code belongs to Dave / GameDevelopment
    Source of tutorial: https://www.youtube.com/watch?v=f473C43s8nE
    The code has not been edited
    The comments were made by the game developers to show understanding of the code

    Only the values have been changed to suit our game
*/

public class PlayerMovement : MonoBehaviour
{
    //the variables are self explanatory
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //used so that the player's character would not keel over
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //the player has to be on the ground
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            //ensures that the player can't jump again while in the process of jumping (while in air)
            readyToJump = false;
            Jump();
            //ensures continous jumping (jumps in sequence) while holding down jump key
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        //the force is normalized so that moving diagonally would not be faster than moving forward or backwards or sideways
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void Jump()
    {
        //y velocity is 0 so that the jump is always the same
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //impulse because the force is applied once (no double jumps, etc)
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void SpeedControl()
    {
        //limiting the player's speed to the defined move speed
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }

    private void Update()
    {
        //the condition grounded is checked so that the drag is not applied while the player is in the air (jumping)
        grounded  = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if(grounded)
        {
            rb.drag = groundDrag;
            //Debug.Log("grounded");
        }
        else
            rb.drag = 0;
        MyInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
}
