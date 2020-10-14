using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FpsMovement : MonoBehaviour
{

    
    public CharacterController characterController;

    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float jumpHeight = 1f;
    
    private Vector3 velocity;
    private bool isGrounded;
    public float speed;
    
    public Transform harness;
 

    public float sheathSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();        
    }

    void UpdateMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // (1/2 g) t^2
            characterController.Move(velocity * Time.deltaTime); // y dir
        }
        
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        Vector3 moveVec = (transform.right * moveX + transform.forward * moveZ) * speed * Time.deltaTime;
        characterController.Move(moveVec);
        velocity.y += gravity * Time.deltaTime; // (1/2 g) t^2
        characterController.Move(velocity * Time.deltaTime); // y dir
    }

    void Jump()
    {
        float Vy = Mathf.Sqrt(jumpHeight * -2 * gravity);
        velocity.y += Vy;
    }
}
