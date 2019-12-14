using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OffCameraTest : MonoBehaviour
{
    // Calls events when this object leaves the camera's view

    [Tooltip("The limit, in viewport distance, the player is allowed off the camera before triggering")]
    public float bufferRange = 0.1f;

    [Tooltip("Called when the object first leaves the camera")]
    public UnityEvent onLeaveCamera;
    [Tooltip("Called every frame the object is off screen")]
    public UnityEvent onOffCameraStay;

    protected bool triggered;

    Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 viewPoint = cam.WorldToViewportPoint(transform.position);
        // Calculate the off screen points based off buffer range
        float min = -bufferRange;
        float max = 1 + bufferRange;

        // If point is off screen
        if (viewPoint.x < min || viewPoint.x > max || viewPoint.y < min || viewPoint.y > max)
        {

            // Call events
            onOffCameraStay.Invoke();
            if (!triggered)
            {
                triggered = true;
                onLeaveCamera.Invoke();
            }
        }

        // If back on screen, reset triggered bool
        else if (triggered)
            triggered = false;
    }
}
