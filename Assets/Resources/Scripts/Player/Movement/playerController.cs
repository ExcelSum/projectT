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
    private Vector2 startTouchPos;
    
    // For simulating swipe threshold and sensitivity
    private const float swipeThreshold = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;  // Start at middle lane
    }

    void Update()
    {
        // Move the player forward continuously
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Handle lane change input via swipe (simulated for mouse)
        HandleSwipeInput();

        // Calculate the target position based on the current lane
        Vector3 desiredPosition = new Vector3(currentLane * laneDistance - laneDistance, transform.position.y, transform.position.z);

        // Smoothly interpolate towards the target position using Lerp
        targetPosition = Vector3.Lerp(targetPosition, desiredPosition, Time.deltaTime * laneChangeSpeed);

        // Update the player's position
        transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);

        // Handle jump input via tap (simulated for mouse click)
        HandleJumpInput();
    }

    private void HandleSwipeInput()
    {
        // Simulate swipe with mouse drag (left or right)
        if (Input.GetMouseButtonDown(0)) // Mouse button down (equivalent to touch start)
        {
            startTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) // While mouse button is held down (equivalent to touch moved)
        {
            float swipeDistance = Input.mousePosition.x - startTouchPos.x;

            if (swipeDistance > swipeThreshold && currentLane < 2)  // Swipe right
            {
                currentLane++;
                startTouchPos = Input.mousePosition;  // Reset start position after swipe
            }
            else if (swipeDistance < -swipeThreshold && currentLane > 0)  // Swipe left
            {
                currentLane--;
                startTouchPos = Input.mousePosition;  // Reset start position after swipe
            }
        }
    }

    private void HandleJumpInput()
    {
        // Simulate tap with mouse click (equivalent to touch tap)
        if (Input.GetMouseButtonDown(0) && !isJumping) // Mouse click (equivalent to tap)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player landed back on the ground
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
