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

    public float groundedRayLength = 1;

    public float rotationDegreesPerSecond = 180;

    public Vector2Reference moveDirection;

    public bool IsGrounded
    {
        get
        {
            // Detect if player is moving in the direction of gravity
            // Prevents being grounded when moving "up"
            // Can't check for greater than 0 as this sometimes causes floating point or rounding errors
            if (Vector2.Dot(rigidbody.velocity, Physics2D.gravity) >= -0.01f)
            {

                // Raycasting to detect if ground is hit
                RaycastHit2D[] results = new RaycastHit2D[1];
                int hits = Physics2D.Raycast(transform.position, -transform.up, contactFilter, results, groundedRayLength);

                return hits > 0;
            }
            return false;
        }
    }

    // Variable for storing the layers that this object can collide to, for raycasting in IsGrounded (Calculated in InitializeCollisionMask())
    private LayerMask collisionMask;

    // Variable for storing contact filter for raycast data, for raycasting in IsGrounded (Calculated in InitializeContactFilter())
    private ContactFilter2D contactFilter;

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
            return false;
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
        // Initialization
        InitializeCollisionMask();
        InitializeContactFilter();


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

    IEnumerator Jump()
    {
        // Calculate initial jump force, and get gravity direction
        Vector2 jumpDir = -Physics2D.gravity.normalized;
        float minForce = CalculateJumpForce(minJumpHeight);

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

    void InitializeCollisionMask()
    {
        int objectLayer = gameObject.layer;
        collisionMask = 0;
        // Cycle through all layers to find which collide and do not collide with the objects layers
        for (int i = 0; i < 32; i++)
        {
            if (!Physics2D.GetIgnoreLayerCollision(objectLayer, i))
            {
                // If so, add to the collision mask
                collisionMask = collisionMask | 1 << i;
            }
        }
    }

    void InitializeContactFilter()
    {
        // Seting up contact filter to check for collisions
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(collisionMask);
        contactFilter.useNormalAngle = false;
        contactFilter.useDepth = false;
        contactFilter.useOutsideDepth = false;
        contactFilter.useOutsideNormalAngle = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * groundedRayLength);
    }
#endif
}
