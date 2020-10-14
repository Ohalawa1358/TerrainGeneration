using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    private Rigidbody rb;

    public Transform player;
    public float walkSpeed;
    public float rotateSpeed;
    public float visibilityRadius;
    
    public Animator animator;

    [HideInInspector]
    public bool isDead;
//     Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < visibilityRadius && !isDead)
        {
            
            HandleMovement();
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunning", true);

        }
        else if (isDead)
        {
            animator.SetBool("isDead", true);
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRunning", false);
            
        }


    }


    private void HandleMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, walkSpeed * Time.deltaTime);

        Vector3 targetPostition = new Vector3(player.position.x,
            this.transform.position.y,
            player.position.z);
        
        transform.LookAt(targetPostition);

    }
    
}
