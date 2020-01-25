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

    public float rotationDegreesPerSecond = 180;

    public Vector2Reference moveDirection;

    public bool IsGrounded
    {
        get;
        protected set;
    }

    bool IsJumpPressed
    {
        get
        {
#if UNITY_IOS || UNITY_ANDROID
            foreach(Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                    return true;
            }
#else
            return Input.GetButtonDown("Jump");
#endif
        }
    }

    bool IsJumpHeld
    {
        get
        {
#if UNITY_IOS || UNITY_ANDROID
            return Input.touchCount > 0;
#else
            return Input.GetButton("Jump");
#endif
        }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();

        if (IsJumpPressed && IsGrounded)
        {
            StartCoroutine(Jump());
        }


    }

    private void LateUpdate()
    {
        RotatePlayer();
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsGrounded)
        {
            foreach (var contact in collision.contacts)
            {
                // If player collides with a "flat" surface, sets isGrounded to true
                if (Vector2.Dot(contact.normal, Physics2D.gravity.normalized) < -0.7f)
                    IsGrounded = true;
            }
        }
    }

    // Moves player based on their relative position to the camera
    void Move()
    {

        // Find if and how far the player is behind or in front of the camera position
        float speedScalar = Vector2.Dot(transform.position - Camera.main.transform.position, -moveDirection.Value);

        // Calculates direction and strength of gravity
        float gravStrength = Vector2.Dot(Physics2D.gravity.normalized, rigidbody.velocity);
        Vector2 gravVector = Physics2D.gravity.normalized * gravStrength;


        // Sets rigidbody's velocity
        rigidbody.velocity = moveDirection.Value * speedScalar + gravVector;

    }

    // Adds force so that player stays at center of camera
    void CenterPlayer()
    {
        // Adjusts players speed to attempt to keep them at the center of the camera's view,
        // If they are behind the camera, speed them up, if they are in front of the camera, slow them down


    }

    IEnumerator Jump()
    {
        // Calculate initial jump force, and get gravity direction
        Vector2 jumpDir = -Physics2D.gravity.normalized;
        float minForce = CalculateJumpForce(minJumpHeight);
        IsGrounded = false;

        rigidbody.AddForce(jumpDir * minForce, ForceMode2D.Impulse);

        yield return new WaitForEndOfFrame();

        float remainingTime = 1;

        // While holding jump, continuously add force to increase jump height
        while (IsJumpHeld && remainingTime > 0)
        {
            rigidbody.AddForce(jumpDir * jumpHoldForce * remainingTime);

            remainingTime -= Time.deltaTime / jumpHoldDuration;
            yield return new WaitForEndOfFrame();
        }
    }


    // Calculates the force needed to jump to inputted height
    float CalculateJumpForce(float height)
    {
        float gravityStrength = Physics2D.gravity.magnitude * rigidbody.gravityScale;
        return Mathf.Sqrt(2 * gravityStrength * height);
    }

    void RotatePlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, -Physics2D.gravity.normalized);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationDegreesPerSecond * Time.deltaTime);
    }


}
