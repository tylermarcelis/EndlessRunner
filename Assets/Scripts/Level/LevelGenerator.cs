using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    protected EnvironmentTile startTile;
    public List<EnvironmentTile> tiles;

    public float maxHeightOffset = 3;

    protected List<EnvironmentTile> instancedTiles = new List<EnvironmentTile>();

    public Vector2Reference levelOriginPoint;

    public Vector2Reference travelDirection;

    public int currentCameraIndex = 0;

    void Start()
    {
        if (startTile)
            instancedTiles.Add(startTile);

        // Creating the first five tiles
        while (instancedTiles.Count < 5)
        {
            CreateNewTile();
        }
    }

    private void Update()
    {
        // Detect when the camera passes over the middle generated tile (index 2) to instantiate next tile and delete oldest
        if (Vector2.Dot(Camera.main.transform.position - instancedTiles[currentCameraIndex].transform.position, instancedTiles[currentCameraIndex].TravelDirection) > 0)
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
        if (instancedTiles != null && instancedTiles.Count > 0)
            previousTile = instancedTiles[instancedTiles.Count - 1];

        // Create and attach new tile
        EnvironmentTile newTile = Instantiate(tiles[Random.Range(0, tiles.Count)], transform);
        if (previousTile)
            newTile.AttachAt(previousTile, this);

        // Add tile to end of list
        instancedTiles.Add(newTile);
    }

    // Destroys oldest tile and creates a new tile
    void CreateNextTile()
    {
        Destroy(instancedTiles[0].gameObject);
        instancedTiles.RemoveAt(0);
        CreateNewTile();
    }

}
