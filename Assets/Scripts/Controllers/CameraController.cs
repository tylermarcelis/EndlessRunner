using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Vector2Reference currentPosition;
    public Vector2Reference travelDirection;

    [Tooltip("Changes the speed at which the camera will move to its set position")]
    public float cameraAdjustSpeed = 1;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 cameraPosition = transform.position;

        // Calculate the camera's distance in the forward direction (travel direction)
        Vector2 positionOffset = cameraPosition - currentPosition.Value;
        Vector2 forwardOffset = Vector2.Dot(positionOffset, travelDirection) * travelDirection.Value;


        // Calculate the camera's distance in the up direction (perpendicular from the travel direction)
        Vector2 upDirection = Vector2.Perpendicular(travelDirection);
        Vector2 upOffset = Vector2.Dot(positionOffset, upDirection) * upDirection;

        // Set position to current postion minus forward direction and moving towards the currentPosition in the up direction over time
        Vector2 finalPosition = cameraPosition - forwardOffset - upOffset * cameraAdjustSpeed * Time.deltaTime;
        transform.position = new Vector3(finalPosition.x, finalPosition.y, transform.position.z);
    }
}
