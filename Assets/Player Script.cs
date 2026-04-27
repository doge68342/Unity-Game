using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using TMPro;
using TMPro.EditorUtilities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public float jumpPower = 5.0f;
    public float jumpBoostFactor = 0.75f;
    public float wallJumpPower = 5f;
    public float walljumpNormalPower = 15f;
    public float dashPower = 20f;
    public float wallJumpDownwardsForceCancelationFactor = 1f;
    public float cameraSensitivity = 1600f;
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 0.99f, 0);
    public float groundMovementSmoothingFactor = 10;
    public float airMovementSmoothingFactor = 5;
    public float maxDashCooldown = 1f;
    public TMP_Text velocityText;
    public Image dashCooldownBar;
    private Vector2 originalDashCooldownBarSize;
    public float jumpBufferTime = 0.2f;
    public float jumpBufferCounter = 0f;
    public Vector3 respawnPosition;
    public int dashCharges;

    public bool isOnGround;
    public bool isTouchingWall;
    private Rigidbody rb;
    private Dictionary<Collider, Vector3> touching = new Dictionary<Collider, Vector3>();
    private float xRotation = 0f;
    private float zRotation = 0f;
    private Vector3 inputVector;
    private Vector3 touchingSurfaceNormal;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    public float dashCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        originalDashCooldownBarSize = dashCooldownBar.rectTransform.rect.size;
    }

    // Update is called once per frame
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity / 675;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity / 675;

        xRotation -= mouseY;
        zRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, zRotation, 0f);

        float moveZ = 0f, moveX = 0f;
        if (Input.GetKey(KeyCode.W)) moveZ = 1.0f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1.0f;
        if (Input.GetKey(KeyCode.A)) moveX = -1.0f;
        if (Input.GetKey(KeyCode.D)) moveX = 1.0f;
        inputVector = new Vector3(moveX, 0f, moveZ);

        isOnGround = false;
        isTouchingWall = false;
        float closestToWallAngle = float.MaxValue;
        foreach (var item in touching)
        {
            if (Mathf.Abs(item.Value.y) < closestToWallAngle)
            {
                closestToWallAngle = item.Value.y;
                touchingSurfaceNormal = item.Value;
            }
            if (item.Value.y > Mathf.Cos(70 * Mathf.Deg2Rad))
            {
                isOnGround = true;
            } 
            else if (item.Value.y < Mathf.Cos(70 * Mathf.Deg2Rad) && item.Value.y > Mathf.Cos(90 * Mathf.Deg2Rad))
            {
                isTouchingWall = true;
            }

        }



        if (Input.GetKeyDown(KeyCode.Space))
        {   
            jumpBufferCounter = jumpBufferTime;
        }

        if (jumpBufferCounter > 0f)
        {
            if (isOnGround || isTouchingWall && isOnGround)
            {
                jumpBufferCounter = 0f;
                rb.AddForce(Vector3.up  * jumpPower + (cameraForward.normalized * inputVector.z + cameraRight.normalized * inputVector.x).normalized * jumpPower * jumpBoostFactor, ForceMode.Impulse); 
            }

            if (isTouchingWall && !isOnGround)
            {
                jumpBufferCounter = 0f;
                rb.AddForce(Vector3.up * (wallJumpPower + -rb.linearVelocity.y * wallJumpDownwardsForceCancelationFactor) + touchingSurfaceNormal * walljumpNormalPower, ForceMode.Impulse);
            }
        }



        dashCooldown = math.min(dashCooldown + Time.deltaTime, maxDashCooldown);
        jumpBufferCounter = Mathf.Max(0, jumpBufferCounter - Time.deltaTime);
        dashCooldownBar.rectTransform.sizeDelta = new Vector2(dashCooldown / maxDashCooldown * originalDashCooldownBarSize.x, originalDashCooldownBarSize.y);

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldown >= maxDashCooldown / dashCharges && inputVector != Vector3.zero)
        {
            rb.AddForce((cameraForward.normalized * inputVector.z + cameraRight.normalized * inputVector.x).normalized * dashPower, ForceMode.Impulse);
            dashCooldown -= maxDashCooldown / dashCharges;
        }

        if (transform.position.y <= -5)
        {
            transform.position = respawnPosition;
        }

    }
    void FixedUpdate()
    {
        cameraForward = cameraTransform.forward;
        cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        rb.MoveRotation(Quaternion.Euler(0f, zRotation, 0f));

        Vector3 move = (cameraForward.normalized * inputVector.z + cameraRight.normalized * inputVector.x).normalized;
        Vector3 targetVelocity = new Vector3(
        move.x * playerSpeed,
        rb.linearVelocity.y,
        move.z * playerSpeed
        );
        if (isOnGround)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, 1f - Mathf.Exp(-groundMovementSmoothingFactor * Time.fixedDeltaTime));
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, 1f - Mathf.Exp(-airMovementSmoothingFactor * Time.fixedDeltaTime));
        }
        
        float totalSpeed = rb.linearVelocity.magnitude;
        velocityText.text = Mathf.Round(totalSpeed) + "";

    }

    void LateUpdate()
    {
        cameraTransform.position = transform.position + cameraOffset;
    }

    void OnCollisionEnter(Collision collision)
    {
        touching[collision.collider] = collision.contacts[0].normal;
    }

    void OnCollisionStay(Collision collision)
    {
        touching[collision.collider] = collision.contacts[0].normal;
    }

    void OnCollisionExit(Collision collision)
    {
        touching.Remove(collision.collider);
    }
}

