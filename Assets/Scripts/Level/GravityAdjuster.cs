using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAdjuster : MonoBehaviour
{
    public Vector2 gravityPreset = new Vector2(0,-9.81f);

    [Tooltip("Should applied gravity be rotated to match object rotation")]
    public bool adjustFromRotation;

    public void ChangeGravity()
    {
        Vector2 gravity = gravityPreset;

        // If true, rotates gravity based on the transform's rotation
        if (adjustFromRotation)
        {
            float angle = Vector2.SignedAngle(Vector2.up, transform.up) * Mathf.Deg2Rad;
            gravity = RotateVector2(gravity, angle);
        }

        Physics2D.gravity = gravity;
    }

    // Rotates a Vector2 by an angle
    Vector2 RotateVector2(Vector2 vector, float angle)
    {
        return new Vector2(
            Mathf.Cos(angle) * vector.x - Mathf.Sin(angle) * vector.y,
            Mathf.Sin(angle) * vector.x + Mathf.Cos(angle) * vector.y);
    }
}
