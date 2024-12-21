using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    private CharacterController controller;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float gravity = -9.81f * 2;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.7f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float midpoint;
    [SerializeField] private float leftConstraint;
    [SerializeField] private float rightConstraint;
    private Vector3 velocity;
    [SerializeField] private int maxJumps = 2; // Allows single + double jump
    private int jumpCount = 0; // Tracks remaining jumps
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset vertical velocity
            jumpCount = maxJumps; // Reset jump counter
        }

        // Player Movement
        float z = Input.GetAxis("Horizontal");
        Vector3 move = transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Handle Jumping
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            PerformJump();
        }

        // Apply Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void PerformJump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        jumpCount--; // Decrease jump count on each jump
    }
}
