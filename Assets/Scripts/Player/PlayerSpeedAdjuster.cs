using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerSpeedAdjuster : MonoBehaviour
{
    public FloatReference baseSpeed;
    public float speedModifier = 1;
    PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Adjusts players speed to attempt to keep them at the center of the camera's view,
        // If they are behind the camera, speed them up, if they are in front of the camera, slow them down

        // Find if and how far the player is behind or infront of the camera
        float speedScalar = Vector2.Dot(controller.transform.position - Camera.main.transform.position, controller.moveDirection);

        // Set movespeed to base speed offset by the scalar
        controller.moveSpeed = baseSpeed - speedScalar * speedModifier;
    }
}
