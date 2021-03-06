﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    protected EnvironmentTile startTile;
    public List<EnvironmentTile> tiles;

    public float maxHeightOffset = 3;

    public List<EnvironmentTile> InstancedTiles
    {
        get;
        protected set;
    } = new List<EnvironmentTile>();
    
    protected int currentCameraIndex = 0;

    void Start()
    {

        foreach(EnvironmentTile tile in tiles)
        {
            tile.CreatePool();
        }

        if (startTile) {
            EnvironmentTile tile = startTile.GetInstance<EnvironmentTile>();
            InstancedTiles.Add(tile);
        }

        // Creating the first five tiles
        while (InstancedTiles.Count < 5)
        {
            CreateNewTile();
        }
    }

    private void Update()
    {
        // Detect when the camera passes over the middle generated tile (index 2) to instantiate next tile and delete oldest
        if (Vector2.Dot(Camera.main.transform.position - InstancedTiles[currentCameraIndex].transform.position, InstancedTiles[currentCameraIndex].TravelDirection) > 0)
        {
            // This is required to prevent instances where the generated level detects that the player has passed over the middle tile, because the tiles loop back upon themselves
            if (currentCameraIndex < 2)
                currentCameraIndex++;
            else
                CreateNextTile();
        }
    }

    void CreateNewTile()
    {
        EnvironmentTile previousTile = null;
        if (InstancedTiles != null && InstancedTiles.Count > 0)
            previousTile = InstancedTiles[InstancedTiles.Count - 1];

        // Create and attach new tile
        EnvironmentTile newTile = tiles[Random.Range(0, tiles.Count)].GetInstance<EnvironmentTile>();
        if (previousTile)
            newTile.AttachAt(previousTile, this);

        // Add tile to end of list
        InstancedTiles.Add(newTile);
    }

    // Destroys oldest tile and creates a new tile
    void CreateNextTile()
    {
        InstancedTiles[0].ReturnObjectToPool();
        InstancedTiles.RemoveAt(0);
        CreateNewTile();
    }


}
