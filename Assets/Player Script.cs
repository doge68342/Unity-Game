using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public float jumpPower = 5.0f;
    public bool isGrounded;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
    void FixedUpdate()
    {
        float moveZ = 0.0f;
        float moveX = 0.0f;


        if (Input.GetKey(KeyCode.W)) moveZ = 1.0f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1.0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1.0f;
        if (Input.GetKey(KeyCode.D)) moveX = 1.0f;

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        rb.linearVelocity = new Vector3(
            move.x * playerSpeed, 
            rb.linearVelocity.y, 
            move.z * playerSpeed
        );
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

