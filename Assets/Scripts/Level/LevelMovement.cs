using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelGenerator))]
public class LevelMovement : MonoBehaviour
{
    public Vector2Reference travelDirection;
    public FloatReference moveSpeed;
    public Vector2Reference centerOffset;

    public FloatReference offsetCorrectionSpeed;

    LevelGenerator levelGen;

    private void Awake()
    {
        levelGen = GetComponent<LevelGenerator>();

        travelDirection.Value = Vector2.right;
    }

    void Update()
    {
        MoveTiles();
    }

    // Moves all instanced tiles in movement direction, and adjusts for offset when gravity is adjusted
    void MoveTiles()
    {
        // Calculating movement smoothing to center on camera
        // Use minimum between offsetCorrectionSpeed and the centerOffset magnitude to ensure it is not overshot
        float correctionAmount = Mathf.Min(offsetCorrectionSpeed * Time.deltaTime, centerOffset.Value.magnitude * Time.deltaTime);
        Vector2 centerCorrection = centerOffset.Value.normalized * correctionAmount;

        centerOffset.Value -= centerCorrection;

        // Calculating movement based on travel direction
        Vector2 horizontalMovement = -travelDirection.Value * moveSpeed * Time.deltaTime;
        Vector2 movement = centerCorrection + horizontalMovement;

        foreach (EnvironmentTile tile in levelGen.InstancedTiles)
        {
            tile.transform.position = (Vector2)tile.transform.position + movement;
        }
    }
}
