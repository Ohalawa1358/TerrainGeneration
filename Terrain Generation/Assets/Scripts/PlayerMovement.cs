using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PlayerMovement : MonoBehaviour
{

    [Range(0, 100)] public float maxSpeed;
    [Range(0, 100)] public float maxAcceleration = 10f;
    [Range(0, 10)] public float jumpHeight = 2f, maxJumpSpeed = 1f;
    [Range(1, 3)] public int maxJumps = 2;
    private int jumpPhase;

    private Vector3 velocity;

    private Rigidbody rb;

    private Vector3 desiredVelocity;

    private bool desiredJump;
    private bool onGround;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        desiredJump |= Input.GetButtonDown("Jump"); // |= because fixed update might not be called next frame so we OR basically if (desiredJump != true)
        desiredVelocity = new Vector3(playerInput.x, 0, playerInput.y) * maxSpeed;
    }

    private void FixedUpdate()
    {
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        UpdateState();

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        
        
        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }
        rb.velocity = velocity;
        
        onGround = false;
    }

    private void UpdateState()
    {
        velocity = rb.velocity;
        if (onGround)
        {
            jumpPhase = 0;
        }
    }

    private void Jump()
    {
        if (onGround || jumpPhase < maxJumps)
        {
            jumpPhase++;
            float jumpSpeed = Mathf.Sqrt(-2*Physics.gravity.y*jumpHeight);
            velocity.y += jumpSpeed;
        }
    }

    //Note: Physics methods occur after all FixedUpdate methods are invoked
    private void OnCollisionStay(Collision other)
    {
        EvaluateCollision(other);
    }
    private void OnCollisionEnter(Collision other)
    {
        EvaluateCollision(other);
    }

    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            onGround = onGround | normal.y >= 0.8f;
        }


    }
    
}
