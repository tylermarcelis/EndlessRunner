using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOffsetAndDirection : MonoBehaviour
{
    public Vector2Reference centerOffset;
    public Vector2Reference travelDirection;

    public void SetValues()
    {
        travelDirection.Value = transform.right;

        // Calculates the distance and direction from the transform to the camera, to allow for smooth movement
        Vector2 offset = Camera.main.transform.position - transform.position;
        Vector2 offsetNormalized = offset.normalized;

        centerOffset.Value = Vector2.Dot(offset, transform.up) * transform.up;
    }
}
