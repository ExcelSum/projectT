using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 10f;  // Speed at which the player moves forward
    public float laneDistance = 2f;   // Distance between lanes
    public float laneChangeSpeed = 10f; // Speed of lane change transition
    public float jumpForce = 5f;      // Jump force

    private Rigidbody rb;
    private int currentLane = 1;      // 0 = left, 1 = middle, 2 = right
    private bool isJumping = false;
    private Vector3 targetPosition;   // Target position for smooth lane transition

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Initialize target position to start at the middle lane
        targetPosition = transform.position;
    }

    void Update()
    {
        // Move the player forward continuously (without affecting the y-position)
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Handle lane change input
        if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        {
            currentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentLane < 2)
        {
            currentLane++;
        }

        // Calculate the target position based on the current lane
        Vector3 desiredPosition = new Vector3(currentLane * laneDistance - laneDistance, transform.position.y, transform.position.z);

        // Smoothly interpolate towards the target position using Lerp
        targetPosition = Vector3.Lerp(targetPosition, desiredPosition, Time.deltaTime * laneChangeSpeed);

        // Update the player's position
        transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Debug.Log("jumping is enabled");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player landed back on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Jumping is disabled");
            isJumping = false;
        }
    }
}
