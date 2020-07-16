using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float acceleration;
    public float jumpForce;
    public float rotationSpeed;

    Vector2 moveInput;
    Rigidbody body;
    Animator animator;
    Transform pivot;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        pivot = transform.Find("Pivot");
    }

    void FixedUpdate()
    {
        // Get current velocity (except vertical)
        Vector2 velocity2D = new Vector2(body.velocity.x, body.velocity.z);
        // Move it toward the input order
        velocity2D = Vector2.MoveTowards(velocity2D, moveInput * speed, acceleration * Time.fixedDeltaTime);
        body.velocity = new Vector3(velocity2D.x, body.velocity.y, velocity2D.y);

        // Rotation
        if (velocity2D.magnitude > 0)
        {
            float angle = Mathf.Atan2(velocity2D.x, velocity2D.y) * Mathf.Rad2Deg;
            pivot.rotation = (Quaternion.RotateTowards(pivot.rotation, Quaternion.AngleAxis(angle, Vector3.up), rotationSpeed * Time.fixedDeltaTime));
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput.magnitude < 0.2)
            moveInput = Vector2.zero;

        animator.SetFloat("Move X", moveInput.x);
        animator.SetFloat("Move Y", moveInput.y);
        animator.SetFloat("Speed", moveInput.magnitude);
    }

    public void OnJump()
    {
        body.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
    }
}
