using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    protected new Rigidbody2D rigidbody;

    public float minJumpHeight = 1;
    [SerializeField]
    protected float jumpHoldForce = 1;
    [SerializeField]
    protected float jumpHoldDuration = 1;

    public float moveSpeed = 1;

    protected bool isGrounded;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            StartCoroutine(Jump());
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGrounded)
        {
            foreach (var contact in collision.contacts)
            {
                // If player collides with a "flat" surface, sets isGrounded to true
                if (Vector2.Dot(contact.normal, Physics2D.gravity.normalized) < -0.7f)
                    isGrounded = true;
            }
        }
    }

    void Move()
    {

        // Calculates direction and strength of gravity
        float gravStrength = Vector2.Dot(Physics2D.gravity.normalized, rigidbody.velocity);
        Vector2 gravVector = Physics2D.gravity.normalized * gravStrength;

        // Finds the direction to move (which is the perpendicular vector to gravity)
        Vector2 runDir = Vector2.Perpendicular(Physics2D.gravity.normalized);

        // Sets rigidbody's velocity
        rigidbody.velocity = runDir * moveSpeed + gravVector;
    }

    IEnumerator Jump()
    {
        // Calculate initial jump force, and get gravity direction
        Vector2 jumpDir = -Physics2D.gravity.normalized;
        float minForce = CalculateJumpForce(minJumpHeight);
        isGrounded = false;

        rigidbody.AddForce(jumpDir * minForce, ForceMode2D.Impulse);

        yield return new WaitForEndOfFrame();

        float remainingTime = 1;

        // While holding jump, continuously add force to increase jump height
        while (Input.GetButton("Jump") && remainingTime > 0)
        {
            rigidbody.AddForce(jumpDir * jumpHoldForce * remainingTime);

            remainingTime -= Time.deltaTime / jumpHoldDuration;
            yield return new WaitForEndOfFrame();
        }
    }


    float CalculateJumpForce(float height)
    {
        float gravityStrength = Physics2D.gravity.magnitude * rigidbody.gravityScale;
        return Mathf.Sqrt(2 * gravityStrength * height);
    }
}
