using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstacles;

    public Transform[] spawnPoints;

    public uint minSpawns;
    public uint maxSpawns;

    private void Start()
    {
        SpawnObstacles();
    }

    // Spawns between min and max spawn obstacles at the set transforms
    void SpawnObstacles()
    {

        // Decide on how many to spawn, limited by the number of inputted spawn points
        int count = Mathf.Min(Random.Range((int)minSpawns, (int)maxSpawns + 1), spawnPoints.Length);

        List<Transform> spPoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < count; i++)
        {
            // Select a random spawn point
            int index = Random.Range(0, spPoints.Count);

            // Create a random obstacle at the selected spawn point
            Instantiate(obstacles[Random.Range(0, obstacles.Length)], spPoints[index]);

            // Remove from the list
            spPoints.RemoveAt(index);
        }
    }
}
