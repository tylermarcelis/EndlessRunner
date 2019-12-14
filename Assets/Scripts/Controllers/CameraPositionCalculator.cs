using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionCalculator : MonoBehaviour
{
    public Vector2Reference centerPoint;
    public Vector2Reference travelDirection;
    public Vector2Reference cameraPosition;
    public FloatReference speed;

    // Setting startup values
    private void Start()
    {
        centerPoint.Value = Vector2.zero;
        travelDirection.Value = Vector2.right;
        cameraPosition.Value = Vector2.zero;
    }

    private void OnEnable()
    {
        if (centerPoint.variable)
            centerPoint.variable.OnChangeValue += OnCenterPointChange;

        OnCenterPointChange(centerPoint);
    }

    private void OnDisable()
    {
        if (centerPoint.variable)
            centerPoint.variable.OnChangeValue -= OnCenterPointChange;
    }

    // Whenever center point is changed, calculates and changes camera position
    private void OnCenterPointChange(Vector2 value)
    {
        Vector2 camPosition = Camera.main.transform.position;
        // Calculates position to be cameras position, cast onto the travel direction
        Vector2 newPosition = value + travelDirection.Value * Vector2.Dot(camPosition - value, travelDirection);
        cameraPosition.Value = newPosition;
    }

    private void Update()
    {
        // Move camera position based on speed every frame
        cameraPosition.Value += travelDirection.Value * speed * Time.deltaTime;
    }
}
