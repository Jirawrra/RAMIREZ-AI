using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantSpawner : MonoBehaviour
{
    public List<GameObject> plantSpawnPoints;
    private List<GameObject> originalSpawnPoints;
    public GameObject plantPrefab;

    public void Awake()
    {
        originalSpawnPoints = new List<GameObject>(plantSpawnPoints);
    }

    public void SpawnPlant(int plantSpawns)
    {
        if (plantSpawns > plantSpawnPoints.Count)
        {
            plantSpawns = plantSpawnPoints.Count; // Limit to available spawn points
        }

        for (int i = 0; i < plantSpawns; i++)
        {
            //randomize instantiate
            int randomIndex = Random.Range(0, plantSpawnPoints.Count);
            Instantiate(plantPrefab, plantSpawnPoints[randomIndex].transform.position, Quaternion.identity);
            plantSpawnPoints.RemoveAt(randomIndex); // Remove used spawn point
        }
    }
}
