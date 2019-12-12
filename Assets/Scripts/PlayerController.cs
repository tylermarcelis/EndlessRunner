using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    protected new Rigidbody2D rigidbody;

    [SerializeField]
    protected float minJumpHeight = 1;
    [SerializeField]
    protected float jumpHoldForce = 1;

    public float jumpHoldDuration = 1;

    protected bool isGrounded;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            StartCoroutine(Jump());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (Vector2.Dot(contact.normal, Physics2D.gravity.normalized) < -0.7f)
                isGrounded = true;
        }
    }


    IEnumerator Jump()
    {
        Vector2 jumpDir = -Physics2D.gravity.normalized;
        float minForce = CalculateJumpForce(minJumpHeight);
        isGrounded = false;

        rigidbody.AddForce(jumpDir * minForce, ForceMode2D.Impulse);

        yield return new WaitForFixedUpdate();

        float remainingTime = 1;

        while (Input.GetButton("Jump") && remainingTime > 0)
        {
            rigidbody.AddForce(jumpDir * jumpHoldForce * remainingTime);

            remainingTime -= Time.fixedDeltaTime / jumpHoldDuration;
            yield return new WaitForFixedUpdate();
        }
    }


    float CalculateJumpForce(float height)
    {
        float gravityStrength = Physics2D.gravity.magnitude * rigidbody.gravityScale;
        return Mathf.Sqrt(2 * gravityStrength * height);
    }
}
