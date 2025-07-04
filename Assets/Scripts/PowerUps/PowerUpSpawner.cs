using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Power-Ups")]
    [SerializeField] private GameObject[] powerUpPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;

    private bool[] isSpawnPointOccupied;
    private Dictionary<GameObject, int> activePowerUps; // Dictionary to track active power-ups and their spawn point index


    void Awake()
    {
        isSpawnPointOccupied = new bool[spawnPoints.Length];
        activePowerUps = new Dictionary<GameObject, int>();

        // Initialize all spawn points as unoccupied
        for (int i = 0; i < isSpawnPointOccupied.Length; i++)
        {
            isSpawnPointOccupied[i] = false;
        }

        StartSpawning();
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnPowerUpsCoroutine());
    }

    private IEnumerator SpawnPowerUpsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            bool isLevelActive = GameObject.Find("LevelManager").GetComponent<LevelManager>().IsLevelActive;
            if (!isLevelActive)
            {
                yield break;
            }

            SpawnPowerUp();
        }
    }

    private void SpawnPowerUp()
    {
        List<int> availableSpawnPoints = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!isSpawnPointOccupied[i])
            {
                availableSpawnPoints.Add(i);
            }

        }

        // Select one random spawn point
        int powerUpsToSpawnCount = Mathf.Min(spawnPoints.Length, availableSpawnPoints.Count);
        List<int> selectedSpawnPointIndices = new List<int>();
        for (int i = 0; i < powerUpsToSpawnCount; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                break;
            }

            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            int selectedSpawnPointIndex = availableSpawnPoints[randomIndex];
            selectedSpawnPointIndices.Add(selectedSpawnPointIndex);
            availableSpawnPoints.RemoveAt(randomIndex);
        }

        foreach (int spawnPointIndex in selectedSpawnPointIndices)
        {
            Vector3 spawnPosition = spawnPoints[spawnPointIndex].position;

            // Instantiate a random power-up prefab
            GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
            GameObject powerUpInstance = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);

            // Mark the spawn point as occupied
            isSpawnPointOccupied[spawnPointIndex] = true;
            activePowerUps[powerUpInstance] = spawnPointIndex;

            PowerUpStates powerUpStates = powerUpInstance.GetComponent<PowerUpStates>();
            PowerUpController powerUpController = powerUpInstance.GetComponent<PowerUpController>();
            powerUpController.powerUpSpawnPoint = spawnPoints[spawnPointIndex].gameObject;

            // Subscribe to the OnPowerUpExpire event to free the spawn point
            powerUpStates.OnPowerUpExpire += () => RemovePowerUp(powerUpInstance);
            powerUpStates.OnPlayerHit += () => RemovePowerUp(powerUpInstance);

        }
    }

    private void RemovePowerUp(GameObject powerUp)
    {
        if (powerUp != null && activePowerUps.ContainsKey(powerUp))
        {
            int spawnPointIndex = activePowerUps[powerUp];
            isSpawnPointOccupied[spawnPointIndex] = false;
            activePowerUps.Remove(powerUp);
        }
    }

}
