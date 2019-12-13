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
        Vector2 positionOffset = cameraPosition - currentPosition.Value;
        Vector2 forwardOffset = Vector2.Dot(positionOffset, travelDirection) * travelDirection.Value;


        Vector2 upDirection = Vector2.Perpendicular(travelDirection);
        Vector2 upOffset = Vector2.Dot(positionOffset, travelDirection) * travelDirection.Value;

        Vector2 finalPosition = cameraPosition + forwardOffset + upOffset * cameraAdjustSpeed * Time.deltaTime;
    }
}
