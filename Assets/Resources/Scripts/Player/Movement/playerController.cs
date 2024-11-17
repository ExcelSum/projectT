using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float laneDistance = 4f; // Distance between lanes
    public float laneChangeSpeed = 10f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Move the player forward continuously
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

        // Calculate the target position
        Vector3 targetPosition = new Vector3(currentLane * laneDistance - laneDistance, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * laneChangeSpeed);

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player landed back on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}